namespace Nodica;

public partial class LineEdit : Button
{
    private class PlaceholderTextDisplayer : BaseText
    {
        public PlaceholderTextDisplayer(LineEdit parent) : base(parent)
        {
            this.parent = parent;
        }

        protected override string GetText()
        {
            return parent.PlaceholderText;
        }

        protected override bool ShouldSkipDrawing()
        {
            return parent.Text.Length > 0;
        }
    }
}