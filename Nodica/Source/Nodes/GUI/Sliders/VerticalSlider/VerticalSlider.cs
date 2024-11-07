using Raylib_cs;

namespace Nodica;

public partial class VerticalSlider : BaseSlider
{
    public VerticalSlider()
    {
        Size = new(10, 100);
        OriginPreset = OriginPreset.TopCenter;
    }

    protected override void UpdatePercentage()
    {
        float currentPosition = Grabber.GlobalPosition.Y;
        float minPos = GlobalPosition.Y - Offset.Y;
        float maxPos = minPos + Size.Y;

        // Evaluate and clamp the percentage
        Percentage = Math.Clamp((currentPosition - minPos) / (maxPos - minPos), 0, 1);
    }

    public override void MoveGrabber(int direction)
    {
        if (direction == 0 || MaxExternalValue == 0)
        {
            return;
        }

        float x = Grabber.GlobalPosition.X;
        float movementUnit = Size.Y / MathF.Abs(MaxExternalValue);
        float y = Grabber.GlobalPosition.Y + direction * movementUnit;

        Grabber.GlobalPosition = new(x, y);
        UpdatePercentageBasedOnGrabber();
    }

    protected override void HandleClicks()
    {
        if (IsMouseOver())
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && OnTopLeft)
            {
                float x = Grabber.GlobalPosition.X;
                float y = Raylib.GetMousePosition().Y;

                Grabber.GlobalPosition = new(x, y);
                Grabber.Pressed = true;

                OnTopLeft = false;
            }
        }
    }

    protected override void MoveGrabberToPercentage(float percentage)
    {

    }

    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Offset,
            Size,
            EmptyStyle);

        Vector2 filledSize = new(Size.X, Size.Y * Percentage);

        DrawBorderedRectangle(
            GlobalPosition - Offset,
            filledSize,
            FilledStyle);
    }
}