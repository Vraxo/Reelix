using Raylib_cs;

namespace Nodica;

public partial class LineEdit : Button
{
    #region [ - - - Properties & Fields - - - ]

    public static readonly Vector2 DefaultSize = new(300, 25);

    public string Text { get; set; } = "";
    public string DefaultText { get; set; } = "";
    public string PlaceholderText { get; set; } = "";
    public Vector2 TextOrigin { get; set; } = new(8, 0);
    public int MaxCharacters { get; set; } = int.MaxValue;
    public int MinCharacters { get; set; } = 0;
    public List<char> ValidCharacters { get; set; } = [];
    public bool Selected { get; set; } = false;
    public bool Editable { get; set; } = true;
    public bool ExpandWidthToText { get; set; } = false;
    public bool RevertToDefaultText { get; set; } = true;
    public bool TemporaryDefaultText { get; set; } = true;
    public bool Secret { get; set; } = false;
    public char SecretCharacter { get; set; } = '*';

    //public ButtonThemePack Themes { get; set; } = new()
    //{
    //    Pressed = new()
    //    {
    //        BorderLength = 1,
    //        FillColor = DefaultTheme.TextBoxPressedFill,
    //        BorderColor = DefaultTheme.Accent
    //    },
    //};

    public int TextStartIndex = 0;

    public Action<LineEdit> OnUpdate = (textBox) => { };

    public event EventHandler? FirstCharacterEntered;
    public event EventHandler? Cleared;
    public event EventHandler<string>? TextChanged;
    public event EventHandler<string>? Confirmed;

    protected Caret caret;
    private Shape shape;
    private TextDisplayer textDisplayer;
    private PlaceholderTextDisplayer placeholderTextDisplayer;

    private const int minAscii = 32;
    private const int maxAscii = 125;
    private const float backspaceDelay = 0.5f;
    private const float backspaceSpeed = 0.05f;

    private float backspaceTimer = 0f;
    private bool backspaceHeld = false;

    private float previousWidth = 0;

    #endregion

    public LineEdit()
    {
        Size = DefaultSize;
        FocusOnClick = true;
        FocusChanged += LineEdit_FocusChanged;

        LeftClicked += LineEdit_LeftClicked;
        ClickedOutside += LineEdit_ClickedOutisde;
        
        Themes.Focused.BorderLength = 0;
        Themes.Focused.BorderLengthDown = 1;
    }
    
    private void LineEdit_ClickedOutisde(object? sender, EventArgs e)
    {
        Selected = false;
    }

    private void LineEdit_LeftClicked(object? sender, EventArgs e)
    {
        Selected = true;
    }

    private void LineEdit_FocusChanged(object sender, bool e)
    {
        Selected = e;
    }

    public override void Start()
    {
        shape = new(this);
        caret = new(this);
        textDisplayer = new(this);
        placeholderTextDisplayer = new(this);

        SizeChanged += OnSizeChanged;
        
        base.Start();
    }

    public override void Update()
    {
        HandleInput();
        PasteText();
        UpdateSizeToFitText();

        base.Update();
        //shape.Update();
        caret.Update();
        textDisplayer.Update();
        placeholderTextDisplayer.Update();
    }

    protected override void OnEnterPressed()
    {
        Selected = false;
        Focused = false;
        Themes.Current = base.Themes.Normal;

        base.OnEnterPressed();
    }

    private void OnSizeChanged(object? sender, Vector2 e)
    {
        if (previousWidth != e.X)
        {
            previousWidth = e.X;
            TextStartIndex = 0;
        }
    }

    private void UpdateSizeToFitText()
    {
        if (!ExpandWidthToText)
        {
            return;
        }

        int textWidth = (int)Raylib.MeasureTextEx(
            Themes.Current.Font,
            Text,
            Themes.Current.FontSize,
            Themes.Current.FontSpacing).X;

        Size = new(textWidth + TextOrigin.X * 2, Size.Y);
    }

    public void Insert(string input)
    {
        if (!Editable)
        {
            return;
        }

        InsertTextAtCaret(input);
    }

    private void HandleInput()
    {
        if (!Editable)
        {
            return;
        }

        HandleClicks();

        if (!Selected)
        {
            return;
        }

        GetTypedCharacters();
        HandleBackspace();
        Confirm();
    }

    private void HandleClicks()
    {
        //if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        //{
        //    if (!IsMouseOver())
        //    {
        //        Selected = false;
        //        Themes.Current = Themes.Normal;
        //    }
        //}
        //
        //if (IsMouseOver())
        //{
        //    if (Raylib.IsMouseButtonDown(MouseButton.Left))
        //    {
        //        if (OnTopLeft)
        //        {
        //            Selected = true;
        //            Themes.Current = Themes.Pressed;
        //        }
        //    }
        //}
        //else
        //{
        //    if (Raylib.IsMouseButtonDown(MouseButton.Left))
        //    {
        //        Selected = false;
        //        Themes.Current = Themes.Normal;
        //    }
        //}
    }

    private void GetTypedCharacters()
    {
        int key = Raylib.GetCharPressed();

        while (key > 0)
        {
            InsertCharacter(key);
            key = Raylib.GetCharPressed();
        }
    }

    private void InsertCharacter(int key)
    {
        bool isKeyInRange = key >= minAscii && key <= maxAscii;
        bool isSpaceLeft = Text.Length < MaxCharacters;

        if (isKeyInRange && isSpaceLeft)
        {
            if (ValidCharacters.Count > 0 && !ValidCharacters.Contains((char)key))
            {
                return;
            }

            if (TemporaryDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X + TextStartIndex, ((char)key).ToString());

            // Check if caret is out of view, and adjust TextStartIndex
            if (caret.X >= GetDisplayableCharactersCount())
            {
                TextStartIndex++;
            }
            else
            {
                caret.X++;
            }

            TextChanged?.Invoke(this, Text);

            if (Text.Length == 1)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void InsertTextAtCaret(string text)
    {
        bool isSpaceLeft = Text.Length + text.Length <= MaxCharacters;

        if (isSpaceLeft)
        {
            if (TemporaryDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X + TextStartIndex, text);
            caret.X += text.Length;

            // Shift text if caret moves out of view
            if (caret.X > GetDisplayableCharactersCount())
            {
                TextStartIndex = caret.X - GetDisplayableCharactersCount();
            }

            TextChanged?.Invoke(this, Text);

            if (Text.Length == text.Length)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void HandleBackspace()
    {
        bool ctrlHeld = Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl);
        bool backspacePressed = Raylib.IsKeyPressed(KeyboardKey.Backspace);

        if (backspacePressed)
        {
            backspaceHeld = true;
            backspaceTimer = 0f;

            if (ctrlHeld)
            {
                DeletePreviousWord();
            }
            else
            {
                DeleteLastCharacter();
            }
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Backspace) && backspaceHeld)
        {
            backspaceTimer += Raylib.GetFrameTime();

            if (backspaceTimer >= backspaceDelay)
            {
                if (backspaceTimer % backspaceSpeed < Raylib.GetFrameTime())
                {
                    if (ctrlHeld)
                    {
                        DeletePreviousWord();
                    }
                    else
                    {
                        DeleteLastCharacter();
                    }
                }
            }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.Backspace))
        {
            backspaceHeld = false;
        }
    }

    private void DeletePreviousWord()
    {
        if (Text.Length == 0 || (caret.X == 0 && TextStartIndex == 0)) return;

        // Calculate the actual index within the full text based on caret position and TextStartIndex
        int removeIndex = caret.X + TextStartIndex - 1;

        // If we're at the very start of the text that's displayed but there is more hidden to the left
        if (caret.X == 0 && TextStartIndex > 0)
        {
            // Shift TextStartIndex left, and move removeIndex accordingly
            TextStartIndex--;
            removeIndex = TextStartIndex;
        }

        // Find the start of the previous word by moving back from removeIndex
        int wordStartIndex = removeIndex;
        while (wordStartIndex > 0 && Text[wordStartIndex - 1] != ' ')
        {
            wordStartIndex--;
        }

        // Calculate the number of characters to delete
        int lengthToDelete = removeIndex - wordStartIndex + 1;
        Text = Text.Remove(wordStartIndex, lengthToDelete);

        // Adjust TextStartIndex if characters were deleted from the hidden portion
        if (wordStartIndex < TextStartIndex)
        {
            int charactersRemoved = removeIndex - wordStartIndex + 1;
            TextStartIndex = Math.Max(0, TextStartIndex - charactersRemoved);
        }

        // Reposition the caret
        caret.X = Math.Clamp(wordStartIndex - TextStartIndex, 0, GetDisplayableCharactersCount());

        RevertTextToDefaultIfEmpty();
        TextChanged?.Invoke(this, Text);

        // Trigger Cleared event if the text is now empty
        if (Text.Length == 0)
        {
            Cleared?.Invoke(this, EventArgs.Empty);
        }
    }

    private void DeleteLastCharacter()
    {
        int textLengthBeforeDeletion = Text.Length;

        if (Text.Length > 0)
        {
            if (caret.X == 0 && TextStartIndex > 0)
            {
                TextStartIndex--;
                Text = Text.Remove(TextStartIndex, 1);
            }
            else if (caret.X > 0)
            {
                int removeIndex = caret.X - 1 + TextStartIndex;
                if (removeIndex >= TextStartIndex && removeIndex < Text.Length)
                {
                    Text = Text.Remove(removeIndex, 1);
                    caret.X = Math.Clamp(caret.X - 1, 0, Math.Min(Text.Length, GetDisplayableCharactersCount()));
                }
            }
        }

        RevertTextToDefaultIfEmpty();
        TextChanged?.Invoke(this, Text);

        if (Text.Length == 0 && textLengthBeforeDeletion != 0)
        {
            Cleared?.Invoke(this, EventArgs.Empty);
        }
    }

    //private void DeleteLastCharacter()
    //{
    //    int textLengthBeforeDeletion = Text.Length;
    //
    //    if (Text.Length > 0 && caret.X > 0)
    //    {
    //        Text = Text.Remove(caret.X - 1 + TextStartIndex, 1);
    //
    //        if (Text.Length % GetDisplayableCharactersCount() < GetDisplayableCharactersCount())
    //        {
    //            if (TextStartIndex > 0)
    //            {
    //                TextStartIndex--;
    //            }
    //        }
    //
    //        //if (caret.X != GetDisplayableCharactersCount() || Text.Length < GetDisplayableCharactersCount())
    //        //{
    //        //    caret.X--;
    //        //}
    //
    //        if (caret.X != GetDisplayableCharactersCount() && TextStartIndex == 0)
    //        {
    //            caret.X--;
    //        }
    //    }
    //
    //    RevertTextToDefaultIfEmpty();
    //
    //    TextChanged?.Invoke(this, Text);
    //
    //    if (Text.Length == 0 && textLengthBeforeDeletion != 0)
    //    {
    //        Cleared?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    private void PasteText()
    {
        bool pressedLeftControl = Raylib.IsKeyDown(KeyboardKey.LeftControl);
        bool pressedV = Raylib.IsKeyPressed(KeyboardKey.V);

        if (pressedLeftControl && pressedV)
        {
            string clipboardText = Raylib.GetClipboardText_();

            if (!string.IsNullOrEmpty(clipboardText))
            {
                char[] clipboardContent = [..clipboardText];

                foreach (char character in clipboardContent)
                {
                    InsertCharacter(character);
                }
            }
        }
    }

    private void Confirm()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Enter))
        {
            Selected = false;
            Themes.Current = Themes.Normal;
            Confirmed?.Invoke(this, Text);
        }
    }

    private void RevertTextToDefaultIfEmpty()
    {
        if (Text.Length == 0)
        {
            Text = DefaultText;
        }
    }

    private int GetDisplayableCharactersCount()
    {
        float availableWidth = Size.X - TextOrigin.X * 2;

        float oneCharacterWidth = Raylib.MeasureTextEx(
            Themes.Current.Font,
            ".",
            Themes.Current.FontSize,
            Themes.Current.FontSpacing).X;

        int displayableCharactersCount = (int)(availableWidth / oneCharacterWidth);

        return displayableCharactersCount;
    }
}