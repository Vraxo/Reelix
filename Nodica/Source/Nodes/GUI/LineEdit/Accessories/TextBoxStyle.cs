namespace Nodica;

public class TextBoxStyle
{
    public TextBoxStyle()
    {
        Roundness = 0.5F;
        OutlineThickness = 1;
        Current = Default;
    }

    public TextBoxStyleState Current = new();

    public TextBoxStyleState Selected = new()
    {
        FillColor = new(34, 34, 34, 255),
        OutlineColor = new(71, 114, 179, 255),
    };

    public TextBoxStyleState Default = new()
    {
        OutlineColor = new(64, 64, 64, 255)
    };

    public int OutlineThickness
    {
        set
        {
            Current.OutlineThickness = value;
            Selected.OutlineThickness = value;
            Default.OutlineThickness = value;
        }
    }

    public float Roundness
    {
        set
        {
            Current.Roundedness = value;
            Selected.Roundedness = value;
            Default.Roundedness = value;
        }
    }

    public float Padding
    {
        set
        {
            Current.Padding = value;
            Selected.Padding = value;
            Default.Padding = value;
        }
    }

    public Font Font
    {
        set
        {
            Current.Font = value;
            Selected.Font = value;
            Default.Font = value;
        }
    }

    public uint FontSize
    {
        set
        {
            Current.FontSize = value;
            Selected.FontSize = value;
            Default.FontSize = value;
        }
    }

    public Color TextColor
    {
        set
        {
            Current.TextColor = value;
            Selected.TextColor = value;
            Default.TextColor = value;
        }
    }

    public Color OutlineColor
    {
        set
        {
            Current.OutlineColor = value;
            Selected.OutlineColor = value;
            Default.OutlineColor = value;
        }
    }

    public Color FillColor
    {
        set
        {
            Current.FillColor = value;
            Selected.FillColor = value;
            Default.FillColor = value;
        }
    }

    public Color IdleFillColor
    {
        set
        {
            Current.IdleFillColor = value;
            Selected.IdleFillColor = value;
            Default.IdleFillColor = value;
        }
    }

    public Color HoverFillColor
    {
        set
        {
            Current.HoverFillColor = value;
            Selected.HoverFillColor = value;
            Default.HoverFillColor = value;
        }
    }
}
