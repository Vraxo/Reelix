using Raylib_cs;

namespace Nodica;

public partial class OptionButton : Button
{
    public bool Open { get; set; } = false;
    public int Choice { get; set; } = -1;
    public List<string> Options { get; set; } = [];
    public int OptionsCount => Options.Count;

    private readonly List<OptionButtonButton> optionChildren = new();

    private string originalDownControlPath;

    public OptionButton()
    {
        LeftClicked += OnLeftClicked;
    }

    public void Add(string option)
    {
        Options.Add(option);
    }

    public void Clear()
    {
        Options.Clear();
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        if (!Open)
        {
            DropDown();
        }
        else
        {
            Close();
        }
    }

    public override void Update()
    {
        if (Open)
        {
            bool isMouseOverAnyOption = IsMouseOver() || optionChildren.Any(option => option.IsMouseOver());

            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !isMouseOverAnyOption)
            {
                Close();
            }
        }

        base.Update();
    }

    private void DropDown()
    {
        if (Open)
        {
            return;
        }

        for (int i = 0; i < Options.Count; i++)
        {
            var option = new OptionButtonButton
            {
                Position = new(0, 25 * (i + 1)),
                Size = Size,
                Themes = new()
                {
                    Roundness = 0
                },
                Text = Options[i],
                Selected = i == Choice,
                Index = i
            };

            AddChild(option, $"Option{i}");
            optionChildren.Add(option);

            if (i != Options.Count - 1)
            {
                option.DownControlPath = $"{AbsolutePath}/Option{i + 1}";
            }

            if (i != 0)
            {
                option.UpControlPath = $"{AbsolutePath}/Option{i - 1}";
            }
            else
            {
                option.UpControlPath = AbsolutePath;
            }
        }

        originalDownControlPath = DownControlPath;
        DownControlPath = $"{AbsolutePath}/Option0";

        Open = true;
    }

    private void Close()
    {
        if (!Open)
        {
            return;
        }

        foreach (var option in optionChildren)
        {
            option.Destroy();
        }

        optionChildren.Clear();
        Open = false;

        DownControlPath = originalDownControlPath;
    }

    private void Select(int index)
    {
        Text = Options[index];
        Choice = index;
        Close();
    }
}