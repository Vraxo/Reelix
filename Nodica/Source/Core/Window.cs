using Raylib_cs;

namespace Nodica;

public static class Window
{
    public static Vector2 OriginalSize = Vector2.Zero;
    public static Vector2 PreviousSize = Vector2.Zero;

    public static Vector2 Size
    {
        get => new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        set
        {
            Raylib.SetWindowSize((int)value.X, (int)value.Y);
        }
    }

    public static bool Fullscreen => Raylib.IsWindowFullscreen();

    public static void ToggleFullscreen()
    {
        Raylib.ToggleFullscreen();
    }
}