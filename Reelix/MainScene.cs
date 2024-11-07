using Nodica;
using Raylib_cs;

namespace Reelix;

public class MainScene : Node2D
{
    private VideoPlayer videoPlayer;
    private HorizontalSlider slider;

    public override void Start()
    {
        GetNode<HorizontalSlider>("Slider").Released += OnSliderReleased;

        videoPlayer = GetNode<VideoPlayer>("VideoPlayer");
        slider = GetNode<HorizontalSlider>("Slider");

        base.Start();
    }

    public override void Update()
    {
        if (videoPlayer.Playing)
        {
            if (!GetNode<HorizontalSlider>("Slider").Grabber.Pressed)
            {
                slider.Percentage = videoPlayer.TimePlayed / videoPlayer.VideoLength;
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            videoPlayer.TogglePause();
        }

        base.Update();
    }

    private void OnSliderReleased(object? sender, float e)
    {
        videoPlayer.Play(e * videoPlayer.VideoLength);
    }
}