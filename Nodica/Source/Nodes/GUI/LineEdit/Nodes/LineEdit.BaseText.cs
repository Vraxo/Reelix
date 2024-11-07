using Raylib_cs;

namespace Nodica;

public partial class LineEdit : Button
{
    private abstract class BaseText
    {
        protected LineEdit parent;

        private Vector2 position = Vector2.Zero;

        private Vector2 GlobalPosition => parent.GlobalPosition + position;

        public BaseText(LineEdit parent)
        {
            this.parent = parent;
        }

        public void Update()
        {
            Draw();
        }

        protected void Draw()
        {
            if (!parent.Visible || ShouldSkipDrawing())
            {
                return;
            }

            Raylib.DrawTextEx(
                parent.Themes.Current.Font,
                GetText(),
                GetPosition(),
                parent.Themes.Current.FontSize,
                parent.Themes.Current.FontSpacing,
                parent.Themes.Current.FontColor);
        }

        protected Vector2 GetPosition()
        {
            Vector2 position = new(GetX(), GetY());
            return position;
        }

        private int GetX()
        {
            int x = (int)(GlobalPosition.X - parent.Offset.X + parent.TextOrigin.X);
            return x;
        }

        private int GetY()
        {
            int halfFontHeight = GetHalfFontHeight();
            int y = (int)(GlobalPosition.Y + (parent.Size.Y / 2) - halfFontHeight - parent.Offset.Y);
            return y;
        }

        private int GetHalfFontHeight()
        {
            Font font = parent.Themes.Current.Font;
            string text = GetText();
            uint fontSize = (uint)parent.Themes.Current.FontSize;

            int halfFontHeight = (int)(Raylib.MeasureTextEx(font, text, fontSize, 1).Y / 2);
            return halfFontHeight;
        }

        protected abstract string GetText();

        protected abstract bool ShouldSkipDrawing();
    }
}