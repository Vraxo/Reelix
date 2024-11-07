namespace Nodica;

public class TextBoxStyleState
{
    public int OutlineThickness = 1;
    public float Roundedness = 0F;
    public float Padding = 8;
    public float Spacing = 1;
    public Font Font = FontLoader.Instance.Fonts["RobotoMono 32"];
    public uint FontSize = 16;
    public Color TextColor = new(255, 255, 255, 255);
    public Color OutlineColor = new(32, 32, 255, 0);
    public Color FillColor = new(84, 84, 84, 255);
    public Color IdleFillColor = new(16, 16, 16, 255);
    public Color HoverFillColor = new(16, 16, 16, 255);
}