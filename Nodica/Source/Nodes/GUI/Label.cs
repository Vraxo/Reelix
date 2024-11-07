using Raylib_cs;

namespace Nodica;

public class Label : VisualItem
{
    public class LabelTheme
    {
        public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 16"];
        public Color FontColor { get; set; } = DefaultTheme.Text;
        public uint FontSize { get; set; } = 16;
        public int FontSpacing { get; set; } = 0;
        public bool EnableShadow { get; set; } = false;
        public Color ShadowColor { get; set; } = Color.Black;
        public Vector2 ShadowOffset { get; set; } = new Vector2(1);
    }

    public enum TextCase
    {
        Upper,
        Lower,
        Both
    }

    public LabelTheme Theme { get; set; } = new();
    public bool Clip { get; set; } = false;
    public string Ellipsis { get; set; } = "...";
    public TextCase Case { get; set; } = TextCase.Both;

    private int _visibleCharacters = -1;
    public int VisibleCharacters
    {
        get => _visibleCharacters;

        set
        {
            _visibleCharacters = value;
            UpdateVisibleRatio();
        }
    }

    private float _visibleRatio = 1.0f;
    public float VisibleRatio
    {
        get => _visibleRatio;

        set
        {
            _visibleRatio = value;
            UpdateVisibleCharacters();
        }
    }

    private string displayedText = "";

    private string _text = "";
    public string Text
    {
        get => _text;

        set
        {
            _text = value;
            UpdateVisibleCharacters();
        }
    }

    public Label()
    {
        OriginPreset = OriginPreset.CenterLeft;
    }

    public override void Update()
    {
        ClipDisplayedText();
        ApplyCase();
        base.Update();
    }

    // Drawing

    protected override void Draw()
    {
        DrawShadow();
        DrawText();
    }

    private void DrawShadow()
    {
        if (!Theme.EnableShadow)
        {
            return;
        }

        Raylib.DrawTextEx(
            Theme.Font,
            displayedText,
            GlobalPosition - Offset + Theme.ShadowOffset,
            Theme.FontSize,
            Theme.FontSpacing,
            Theme.ShadowColor);
    }

    private void DrawText()
    {
        Raylib.DrawTextEx(
            Theme.Font,
            displayedText,
            GlobalPosition - Offset - new Vector2(0, Theme.FontSize / 2),
            Theme.FontSize,
            Theme.FontSpacing,
            Theme.FontColor);
    }

    // Text modification

    private void ClipDisplayedText()
    {
        string textToConsider = VisibleCharacters == -1 ? 
                                _text : 
                                _text[..Math.Min(VisibleCharacters, _text.Length)];

        if (!Clip)
        {
            displayedText = textToConsider;
            return;
        }

        int numFittingCharacters = (int)(Size.X / (GetCharacterWidth() + Theme.FontSpacing));

        if (VisibleCharacters != -1)
        {
            numFittingCharacters = Math.Min(numFittingCharacters, VisibleCharacters);
        }

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < textToConsider.Length)
        {
            string trimmedText = textToConsider[..numFittingCharacters];
            displayedText = ClipTextWithEllipsis(trimmedText);
        }
        else
        {
            displayedText = textToConsider;
        }
    }

    private float GetCharacterWidth()
    {
        float width = Raylib.MeasureTextEx(
            Theme.Font,
            " ",
            Theme.FontSize,
            Theme.FontSpacing).X;

        return width;
    }

    private string ClipTextWithEllipsis(string input)
    {
        if (input.Length > Ellipsis.Length)
        {
            string trimmedText = input[..^Ellipsis.Length];
            return trimmedText + Ellipsis;
        }
        else
        {
            return input;
        }
    }

    private void ApplyCase()
    {
        displayedText = Case switch
        {
            TextCase.Upper => displayedText.ToUpper(),
            TextCase.Lower => displayedText.ToLower(),
            _ => displayedText
        };
    }

    // Visibility control

    private void UpdateVisibleCharacters()
    {
        if (_text.Length > 0)
        {
            _visibleCharacters = (int)(_text.Length * _visibleRatio);
        }
        else
        {
            _visibleCharacters = -1; // Show all characters by default
        }
    }

    private void UpdateVisibleRatio()
    {
        if (_text.Length > 0)
        {
            _visibleRatio = _visibleCharacters / (float)_text.Length;
        }
        else
        {
            _visibleRatio = 1.0f; // Show all characters by default
        }
    }
}