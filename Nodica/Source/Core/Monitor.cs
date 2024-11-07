using Raylib_cs;

namespace Nodica;

public static class Monitor
{
    public static Vector2 Size
    {
        get
        {
            int currentMonitor = Raylib.GetCurrentMonitor();
            int width = Raylib.GetMonitorWidth(currentMonitor);
            int height = Raylib.GetMonitorHeight(currentMonitor);

            return new(width, height);
        }
    }
}