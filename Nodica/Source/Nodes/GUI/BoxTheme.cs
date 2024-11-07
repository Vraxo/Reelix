namespace Nodica;

public class BoxTheme
{
    public float Roundness { get; set; } = 0.2f;
    public Color FillColor { get; set; } = DefaultTheme.NormalFill;
    public Color BorderColor { get; set; } = DefaultTheme.NormalBorder;

    public float BorderLengthUp { get; set; } = 0;
    public float BorderLengthRight { get; set; } = 0;
    public float BorderLengthDown { get; set; } = 0;
    public float BorderLengthLeft { get; set; } = 0;

    public float BorderLength 
    { 
        set
        {
            BorderLengthUp = value;
            BorderLengthRight = value;
            BorderLengthDown = value;
            BorderLengthLeft = value;
        }
    }
}