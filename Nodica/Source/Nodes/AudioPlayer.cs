using Raylib_cs;

namespace Nodica;

public class AudioPlayer : Node
{
    public Music Audio { get; set; }
    public bool HasAudio = false;
    public bool AutoPlay { get; set; } = false;
    public bool Loop { get; set; } = false;
    public bool Playing => Raylib.IsMusicStreamPlaying(Audio);
    public float TimePlayed => Raylib.GetMusicTimePlayed(Audio);

    public float AudioLength => HasAudio ?
                                Raylib.GetMusicTimeLength(Audio) :
                                0;

    private float _volume = 1;
    public float Volume
    {
        get => _volume;

        set
        {
            _volume = value;

            if (!HasAudio)
            {
                return;
            }

            Raylib.SetMusicVolume(Audio, _volume);
        }
    }

    private float _pitch = 1;
    public float Pitch
    {
        get => _pitch;

        set
        {
            _pitch = value;

            if (!HasAudio)
            {
                return;
            }

            Raylib.SetMusicPitch(Audio, _pitch);
        }
    }

    private float _pan = 0.5f;
    public float Pan
    {
        get => _pan;

        set
        {
            if (!HasAudio)
            {
                return;
            }

            _pan = value;
            Raylib.SetMusicPan(Audio, _pan);
        }
    }

    public event EventHandler? AudioFinished;

    public override void Ready()
    {
        if (!HasAudio)
        {
            return;
        }

        if (AutoPlay)
        {
            Play();
        }
    }

    public override void Update()
    {
        Raylib.UpdateMusicStream(Audio);

        if (TimePlayed >= AudioLength - 0.1 && Playing)
        {
            if (Loop)
            {
                Play();
                AudioFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Stop();
            }
        }
    }

    public void Load(string path)
    {
        Audio = Raylib.LoadMusicStream(path);
        HasAudio = true;
        Volume = Volume;
        Pitch = Pitch;
        Pan = Pan;
    }

    public void Play(float timestamp = 0.1f)
    {
        if (!HasAudio)
        {
            return;
        }

        timestamp = Math.Clamp(timestamp, 0.1f, AudioLength);

        if (timestamp >= AudioLength - 0.1f)
        {
            Stop();
        }
        else
        {
            Raylib.SeekMusicStream(Audio, timestamp);
            Raylib.PlayMusicStream(Audio);
        }
    }

    public void Resume()
    {
        if (!HasAudio)
        {
            return;
        }

        Raylib.ResumeMusicStream(Audio);
    }

    public void Pause()
    {
        if (!HasAudio)
        {
            return;
        }

        Raylib.PauseMusicStream(Audio);
    }

    public void Stop()
    {
        if (!HasAudio)
        {
            return;
        }

        Raylib.StopMusicStream(Audio);
        AudioFinished?.Invoke(this, EventArgs.Empty);
    }

    public void Seek(float timestamp)
    {
        if (!HasAudio)
        {
            return;
        }

        timestamp = Math.Clamp(timestamp, 0.1f, AudioLength);

        Raylib.SeekMusicStream(Audio, timestamp);
    }
}