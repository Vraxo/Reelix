namespace Nodica;

public partial class NumberLineEdit : LineEdit
{
    public float MinValue { get; set; } = int.MinValue;
    public float MaxValue { get; set; } = int.MaxValue;

    public float Value
    {
        get
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return float.Parse(Text);
            }

            return 0;
        }
    }

    public NumberLineEdit()
    {
        TemporaryDefaultText = true;
        RevertToDefaultText = true;
        DefaultText = "0";
        Text = "0";
        ValidCharacters = CharacterSet.Numbers;
    }

    public override void Update()
    {
        ClampValue();
        base.Update();
    }

    private void ClampValue()
    {
        if (Value < MinValue)
        {
            Text = MinValue.ToString();
        }

        if (Value > MaxValue)
        {
            Text = MaxValue.ToString();
            caret.X = Text.Length;
        }
    }
}