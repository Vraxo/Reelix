namespace Nodica;

public partial class LineEdit : Button
{
    private class TextDisplayer : BaseText
    {
        public TextDisplayer(LineEdit parent) : base(parent)
        {
            this.parent = parent;
        }

        protected override string GetText()
        {
            // Ensure TextStartIndex is non-negative
            parent.TextStartIndex = Math.Max(0, parent.TextStartIndex);

            // Calculate the max length of the substring
            int displayableLength = Math.Min(parent.Text.Length - parent.TextStartIndex, parent.GetDisplayableCharactersCount());

            // Ensure that we do not attempt to get a substring with an invalid start index
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.Text.Length) :
                parent.Text.Substring(parent.TextStartIndex, Math.Max(0, displayableLength));
        }


        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}