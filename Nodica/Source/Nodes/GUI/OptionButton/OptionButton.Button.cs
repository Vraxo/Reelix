namespace Nodica;

public partial class OptionButton : Button
{
    public class OptionButtonButton : Button
    {
        public int Index = 0;
        public bool Selected = false;
        public BoxTheme CheckTheme = new();

        private OptionButton parent;

        public OptionButtonButton()
        {
            CheckTheme.Roundness = 1;
            CheckTheme.FillColor = DefaultTheme.Accent;
        }

        public override void Start()
        {
            TextAlignment.Horizontal = HorizontalAlignment.Right;
            TextOffset = new(-4, 0);

            parent = GetParent<OptionButton>();

            LeftClicked += OnLeftClicked;

            base.Start();
        }

        protected override void Draw()
        {
            base.Draw();

            if (!Selected)
            {
                return;
            }

            DrawBorderedRectangle(
                GlobalPosition - Origin + new Vector2(10, 7.5f),
                new(10, 10),
                CheckTheme);
        }

        private void OnLeftClicked(object? sender, EventArgs e)
        {
            parent.Select(Index);

            if (Focused)
            {
                parent.Focused = true;
            }
        }
    }
}