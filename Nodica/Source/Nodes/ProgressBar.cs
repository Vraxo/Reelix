namespace Nodica;

public class ProgressBar : VisualItem
{
    public BoxTheme EmptyTheme = new();

    public BoxTheme FilledStyle = new()
    { 
        FillColor = DefaultTheme.Accent
    };

    private float _percentage = 0;
    public float Percentage
    {
        get => _percentage;

        set
        {
            _percentage = Math.Clamp(value, 0, 1);
        }
    }

    public ProgressBar()
    {
        Size = new(250, 10);
    }

    protected override void Draw()
    {
        DrawEmpty();
        DrawFilled();
    }

    private void DrawEmpty()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            EmptyTheme);
    }

    private void DrawFilled()
    {
        if (Percentage == 0)
        {
            return;
        }

        Vector2 filledSize = new(Size.X * Percentage, Size.Y);
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            filledSize,
            FilledStyle);
    }
}