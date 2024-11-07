using Raylib_cs;

namespace Nodica;

public abstract class ClickableRectangle : Clickable
{
    public override bool IsMouseOver()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();

        Vector2 scaledOrigin = Scale * Origin;
        Vector2 scaledSize = Scale * Size;

        bool isMouseOver = mousePosition.X > GlobalPosition.X - scaledOrigin.X &&
                           mousePosition.X < GlobalPosition.X + scaledSize.X - scaledOrigin.X &&
                           mousePosition.Y > GlobalPosition.Y - scaledOrigin.Y &&
                           mousePosition.Y < GlobalPosition.Y + scaledSize.Y - scaledOrigin.Y;

        return isMouseOver;
    }
}