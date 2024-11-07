using Raylib_cs;
using System.Diagnostics;
using System.IO;

namespace Nodica;

public class VideoPlayer : VisualItem
{
    public string VideoPath { get; set; } = "Resources/Video2.mp4";
    public int FramesPerSecond { get; set; } = 24;
    public bool Loop { get; set; } = false;
    public bool AutoPlay { get; set; } = true;
    public bool StayOnLastFrame { get; set; } = false;
    public bool Playing { get; private set; } = false;

    private bool paused = false; // New flag for pause state
    private int framesExtracted = 0;
    private int currentFrameIndex = 0;
    private Texture2D currentTexture = new();
    private Queue<Texture2D> textures = new();
    private List<string> texturePaths = new();
    private Music audio;
    private bool isAudioLoaded = false;
    private bool isAudioPlaying = false;
    private float videoLength = -1;

    private float lastPlayTimestamp = 0f; // Keeps track of last play timestamp
    public float TimePlayed => lastPlayTimestamp + Raylib.GetMusicTimePlayed(audio); // New property for tracking time played

    private string TempDir => $"Resources/Temporary/VideoPlayer_{Name}";
    private string FramePattern => Path.Combine(TempDir, $"{Name}_Frame_{{0}}.png");
    private string AudioPath => Path.Combine(TempDir, "Audio.mp3");

    public float VideoLength
    {
        get
        {
            if (videoLength < 0)
            {
                videoLength = GetVideoLength();
            }
            return videoLength;
        }
    }

    public override void Start()
    {
        if (AutoPlay)
        {
            Play(5);
        }
    }

    public override void Update()
    {
        Position = Window.Size / 2;

        if (Playing && !paused) // Skip update logic if paused
        {
            KeepLoadingTextures();
            bool enoughTexturesLoaded = textures.Count >= 1;
            bool videoIsNotOver = currentFrameIndex < framesExtracted - 1;

            if (enoughTexturesLoaded && (Loop || videoIsNotOver))
            {
                StartAudioIfNotStarted();
                UpdateTexture();
                StopIfOver();
            }

            UpdateAudio();
        }

        base.Update();
    }

    public override void Destroy()
    {
        Stop();
        base.Destroy();

        if (Directory.Exists(TempDir))
        {
            Directory.Delete(TempDir, true);
        }
    }

    protected override void Draw()
    {
        if (currentTexture.Id != 0 && (StayOnLastFrame || currentFrameIndex < framesExtracted - 1))
        {
            Raylib.DrawTextureV(
                currentTexture,
                GlobalPosition - new Vector2(currentTexture.Width, currentTexture.Height) / 2,
                Color.White);
        }
    }

    public async Task Play(float timestamp = 0)
    {
        ClearLoadedTextures();
        timestamp = Math.Min(timestamp, VideoLength - 1);
        Stop();
        lastPlayTimestamp = timestamp; // Update the starting timestamp

        DeleteExistingFrames();
        Playing = true;
        paused = false; // Ensure we are not paused when starting
        Directory.CreateDirectory(TempDir);

        await Task.WhenAll(ExtractAudio(timestamp), ExtractFrames(timestamp));
    }

    public void Stop()
    {
        if (Playing)
        {
            Raylib.StopMusicStream(audio);
            isAudioPlaying = false;
            Playing = false;
            currentFrameIndex = 0;
            framesExtracted = 0;
            textures.Clear();
            //ClearLoadedTextures();
            DeleteExistingFrames();

            if (isAudioLoaded)
            {
                Raylib.UnloadMusicStream(audio);
                isAudioLoaded = false;
            }
        }
    }

    private void ClearLoadedTextures()
    {
        foreach (var path in texturePaths)
        {
            TextureLoader.Instance.Remove(path);
        }
        texturePaths.Clear();
    }

    private async Task ExtractFrames(float timestamp = 0)
    {
        var ffmpegPath = Path.Combine("Resources", "ffmpeg.exe");
        var outputFilePattern = FramePattern.Replace("{0}", "%d");

        var startInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = $"-ss {timestamp} -i \"{VideoPath}\" -vf fps={FramesPerSecond} \"{outputFilePattern}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                framesExtracted = Directory.GetFiles(TempDir, $"{Name}_Frame_*.png").Length;
            }
        }
    }

    private async Task ExtractAudio(float timestamp = 0)
    {
        var ffmpegPath = Path.Combine("Resources", "ffmpeg.exe");

        if (File.Exists(AudioPath))
        {
            File.Delete(AudioPath);
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = $"-ss {timestamp} -i \"{VideoPath}\" \"{AudioPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                audio = Raylib.LoadMusicStream(AudioPath);
                isAudioLoaded = true;
                isAudioPlaying = false;
            }
        }
    }

    private float GetVideoLength()
    {
        var ffmpegPath = Path.Combine("Resources", "ffmpeg.exe");

        var startInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = $"-i \"{VideoPath}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            string output = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var match = System.Text.RegularExpressions.Regex.Match(output, @"Duration: (\d+):(\d+):(\d+\.\d+)");
            if (match.Success)
            {
                int hours = int.Parse(match.Groups[1].Value);
                int minutes = int.Parse(match.Groups[2].Value);
                float seconds = float.Parse(match.Groups[3].Value);

                return hours * 3600 + minutes * 60 + seconds;
            }
        }
        return 0;
    }

    private void DeleteExistingFrames()
    {
        if (Directory.Exists(TempDir))
        {
            foreach (var file in Directory.GetFiles(TempDir, $"{Name}_Frame_*.png"))
            {
                try { File.Delete(file); } catch { }
            }

            if (File.Exists(AudioPath))
            {
                try { File.Delete(AudioPath); } catch { }
            }
        }
    }

    private void KeepLoadingTextures()
    {
        if (framesExtracted > 0 && textures.Count < framesExtracted)
        {
            string path = string.Format(FramePattern, textures.Count + 1);

            if (File.Exists(path))
            {
                textures.Enqueue(TextureLoader.Instance.Get(path));
                texturePaths.Add(path);
            }
        }
    }

    private void StopIfOver()
    {
        if (currentFrameIndex >= framesExtracted - 1)
        {
            Stop();

            if (Loop)
            {
                Play();
            }
        }
    }

    private void StartAudioIfNotStarted()
    {
        if (!isAudioPlaying && isAudioLoaded)
        {
            Raylib.PlayMusicStream(audio);
            isAudioPlaying = true;
        }
    }

    private void UpdateAudio()
    {
        if (isAudioPlaying)
        {
            Raylib.UpdateMusicStream(audio);
        }
    }

    private void UpdateTexture()
    {
        double musicTimePlayed = Raylib.GetMusicTimePlayed(audio);
        double frameDuration = 1.0 / FramesPerSecond;
        int targetFrameIndex = (int)(musicTimePlayed / frameDuration);

        if (targetFrameIndex >= textures.Count)
        {
            targetFrameIndex = textures.Count - 1;
        }

        while (textures.Count > targetFrameIndex + 1)
        {
            textures.Dequeue();
        }

        if (targetFrameIndex < framesExtracted)
        {
            currentFrameIndex = targetFrameIndex;
            string path = string.Format(FramePattern, currentFrameIndex + 1);

            if (File.Exists(path))
            {
                currentTexture = TextureLoader.Instance.Get(path);
            }

            var previousFramePath = string.Format(FramePattern, currentFrameIndex - 1);
            if (File.Exists(previousFramePath))
            {
                TextureLoader.Instance.Remove(previousFramePath);
            }
        }
    }

    // Pause the video
    public void Pause()
    {
        if (Playing && !paused)
        {
            paused = true;
            Raylib.PauseMusicStream(audio); // Pause the audio as well
        }
    }

    // Resume the video from the paused state
    public void Resume()
    {
        if (Playing && paused)
        {
            paused = false;
            Raylib.ResumeMusicStream(audio); // Resume the audio
        }
    }

    // Toggle pause/resume state
    public void TogglePause()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}