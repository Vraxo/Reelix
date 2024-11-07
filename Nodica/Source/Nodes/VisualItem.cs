using Raylib_cs;

namespace Nodica;

public abstract class VisualItem : Node2D
{
    public bool Visible { get; set; } = true;
    public bool ReadyForVisibility = false;

    public override void Update()
    {
        if (Visible && ReadyForVisibility)
        {
            Draw();
        }
        base.Update();
        ReadyForVisibility = true;
    }

    protected virtual void Draw() { }

    protected void DrawBorderedRectangle(Vector2 position, Vector2 size, BoxTheme theme)
    {
        // Calculate the total border length by combining the four sides
        float top = theme.BorderLengthUp;
        float right = theme.BorderLengthRight;
        float bottom = theme.BorderLengthDown;
        float left = theme.BorderLengthLeft;

        // Adjust the outer rectangle size based on the border lengths
        Rectangle outerRectangle = new()
        {
            Position = new Vector2(position.X - left, position.Y - top),
            Size = new Vector2(size.X + left + right, size.Y + top + bottom)
        };

        // Draw the outer rectangle (border)
        if (top > 0 || right > 0 || bottom > 0 || left > 0)
        {
            Raylib.DrawRectangleRounded(
                outerRectangle,
                theme.Roundness,
                (int)size.Y,  // segments count, you can adjust it as needed
                theme.BorderColor
            );
        }

        // Draw the inner rectangle (fill) with no offset
        Rectangle innerRectangle = new()
        {
            Position = position,
            Size = size
        };

        Raylib.DrawRectangleRounded(
            innerRectangle,
            theme.Roundness,
            (int)size.Y,
            theme.FillColor
        );
    }
}