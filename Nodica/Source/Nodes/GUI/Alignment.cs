namespace Nodica;

public class Alignment
{
    public HorizontalAlignment Horizontal { get; set; } = HorizontalAlignment.Center;
    public VerticalAlignment Vertical { get; set; } = VerticalAlignment.Center;

    public Alignment() { }

    public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
    {
        Horizontal = horizontal;
        Vertical = vertical;
    }
}