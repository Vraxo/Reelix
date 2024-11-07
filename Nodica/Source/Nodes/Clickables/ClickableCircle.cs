using Raylib_cs;

namespace Nodica;

public abstract class ClickableCircle : Clickable
{
    public float Radius = 10F;

    public override bool IsMouseOver()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        float distance = MathUtilities.GetDistance(mousePosition, GlobalPosition);
        bool isMouseOver = distance < Radius;

        return isMouseOver;
    }
}