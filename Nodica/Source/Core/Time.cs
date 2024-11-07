using Raylib_cs;

namespace Nodica;

public static class Time
{
    public static float DeltaTime => Raylib.GetFrameTime();
}