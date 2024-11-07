using Nodica;

public class CheckBoxThemePack
{
    // States

    public BoxTheme Current { get; set; } = new();

    public BoxTheme Normal { get; set; } = new();

    public BoxTheme Hover { get; set; } = new()
    {
        FillColor = DefaultTheme.HoverFill
    };

    public BoxTheme Pressed { get; set; } = new()
    {
        FillColor = DefaultTheme.Accent
    };

    public BoxTheme Disabled { get; set; } = new()
    {
        FillColor = DefaultTheme.DisabledFill,
        BorderColor = DefaultTheme.DisabledBorder,
    };

    public BoxTheme Focused { get; set; } = new()
    {
        BorderColor = DefaultTheme.FocusBorder,
        BorderLength = 1
    };

    // Setters
    
    public float Roundness
    {
        set
        {
            Current.Roundness = value;
            Normal.Roundness = value;
            Hover.Roundness = value;
            Pressed.Roundness = value;
            Focused.Roundness = value;
            Disabled.Roundness = value;
        }
    }

    public float OutlineThickness
    {
        set
        {
            Current.BorderLength = value;
            Normal.BorderLength = value;
            Hover.BorderLength = value;
            Pressed.BorderLength = value;
        }
    }

    public Color FillColor
    {
        set
        {
            Current.FillColor = value;
            Normal.FillColor = value;
            Hover.FillColor = value;
            Pressed.FillColor = value;
        }
    }

    public Color BorderColor
    {
        set
        {
            Current.BorderColor = value;
            Normal.BorderColor = value;
            Hover.BorderColor = value;
            Pressed.BorderColor = value;
        }
    }

    public float BorderLengthUp
    {
        set
        {
            Current.BorderLengthUp = value;
            Normal.BorderLengthUp = value;
            Hover.BorderLengthUp = value;
            Pressed.BorderLengthUp = value;
        }
    }
}