using Raylib_cs;

namespace Nodica.Input;

public static class Input
{
    public static bool IsKeyPressed(KeyboardKey keyboardKey)
    {
        return Raylib.IsKeyPressed((Raylib_cs.KeyboardKey)keyboardKey);
    }

    public static bool IsKeyReleased(KeyboardKey keyboardKey)
    {
        return Raylib.IsKeyReleased((Raylib_cs.KeyboardKey)keyboardKey);
    }
}