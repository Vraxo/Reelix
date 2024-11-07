namespace Nodica;

public static class DefaultTheme
{
    public static readonly Color Background = new(16, 16, 16, 255);

    public static readonly Color NormalFill = new(42, 42, 42, 255);
    public static readonly Color NormalBorder = new(61, 61, 61, 255);

    public static readonly Color HoverFill = new(50, 50, 50, 255);
    public static readonly Color HoverOutline = new(71, 71, 71, 255);

    public static readonly Color Accent = new(71, 114, 179, 255);
    public static readonly Color AccentLighter = new(91, 134, 199, 255);
    public static readonly Color AccentDarker = new(51, 94, 159, 235);

    public static readonly Color PressedOutline = new(61, 61, 61, 255);
    public static readonly Color TextBoxPressedFill = new(68, 68, 68, 255);

    public static readonly Color SliderEmptyFill = new(101, 101, 101, 255);
    public static readonly Color SliderFillColor = new(71, 114, 179, 255);

    public static readonly Color Text = new(255, 255, 255, 255);

    // Disabled Colors
    public static readonly Color DisabledFill = new(30, 30, 30, 255);
    public static readonly Color DisabledBorder = new(50, 50, 50, 255);
    public static readonly Color DisabledText = new(150, 150, 150, 255);

    public static readonly Color FocusBorder = new(71, 114, 179, 255); // Slightly lighter border for focus
}