using Raylib_cs;

namespace Nodica;

public partial class LineEdit : Button
{
    private class Shape : VisualItem
    {
        private LineEdit parent;

        public Shape(LineEdit parent)
        {
            this.parent = parent;
        }

        public void Update()
        {
            Draw();
        }

        private void Draw()
        {
            if (!(parent.Visible && parent.ReadyForVisibility))
            {
                return;
            }

            DrawBorderedRectangle(
                parent.GlobalPosition - parent.Offset,
                parent.Size,
                parent.Themes.Current);

            //DrawOutline();
            //DrawInside();
        }

        //private void DrawOutline()
        //{
        //    if (parent.Themes.Current.BorderLength <= 0)
        //    {
        //        return;
        //    }
        //
        //    for (int i = 1; i <= parent.Themes.Current.BorderLength; i++)
        //    {
        //        Vector2 offset = new(i / 2f, i / 2f);
        //
        //        Rectangle rectangle = new()
        //        {
        //            Position = parent.GlobalPosition - parent.Offset - offset,
        //            Size = new(parent.Size.X + i, parent.Size.Y + i)
        //        };
        //
        //        Raylib.DrawRectangleRounded(
        //            rectangle,
        //            parent.Themes.Current.Roundness,
        //            (int)parent.Size.Y,
        //            parent.Themes.Current.BorderColor);
        //    }
        //}
        //
        //private void DrawInside()
        //{
        //    Rectangle rectangle = new()
        //    {
        //        Position = parent.GlobalPosition - parent.Offset,
        //        Size = parent.Size
        //    };
        //
        //    Raylib.DrawRectangleRounded(
        //        rectangle,
        //        parent.Themes.Current.Roundness,
        //        (int)parent.Size.Y,
        //        parent.Themes.Current.FillColor);
        //}
    }
}