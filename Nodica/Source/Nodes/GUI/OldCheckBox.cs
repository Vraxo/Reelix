//using Raylib_cs;
//
//namespace Nodica;
//
//public class CheckBox : ClickableRectangle
//{
//    public Vector2 CheckSize = new(10, 10);
//    public ButtonThemePack BackgroundStyle = new();
//    public ButtonThemePack CheckStyle = new();
//    public bool Selected = false;
//    public Action<CheckBox> OnUpdate = (checkBox) => { };
//    public event EventHandler? Selected;
//
//    public CheckBox()
//    {
//        Size = new(20, 20);
//        OriginPreset = OriginPreset.Center;
//
//        BackgroundStyle.Roundness = 1;
//
//        CheckStyle.Normal.FillColor = new(71, 114, 179, 255);
//        CheckStyle.Current = CheckStyle.Normal;
//    }
//
//    public override void Update()
//    {
//        Draw();
//        HandleClicks();
//        OnUpdate(this);
//        base.Update();
//    }
//
//    protected override void Draw()
//    {
//        Rectangle rectangle = new()
//        {
//            Position = GlobalPosition - Offset,
//            Size = Size
//        };
//
//        DrawInside(rectangle);
//        DrawOutline(rectangle);
//        DrawCheck();
//    }
//
//    private void DrawInside(Rectangle rectangle)
//    {
//        //Raylib.DrawRectangleRounded(
//        //    rectangle,
//        //    BackgroundStyle.Current.Roundness,
//        //    (int)Size.Y,
//        //    BackgroundStyle.Current.FillColor);
//    }
//
//    private void DrawOutline(Rectangle rectangle)
//    {
//        //if (BackgroundStyle.Current.BorderLength > 0)
//        //{
//        //    Raylib.DrawRectangleRoundedLines(
//        //        rectangle,
//        //        BackgroundStyle.Current.Roundness,
//        //        (int)Size.Y,
//        //        BackgroundStyle.Current.BorderLength,
//        //        BackgroundStyle.Current.BorderColor);
//        //}
//    }
//
//    private void DrawCheck()
//    {
//        if (!Selected)
//        {
//            return;
//        }
//
//        Rectangle rectangle = new()
//        {
//            Position = GlobalPosition - Offset / 2,
//            Size = CheckSize
//        };
//
//        Raylib.DrawRectangleRounded(
//            rectangle,
//            BackgroundStyle.Current.Roundness,
//            (int)CheckSize.Y,
//            CheckStyle.Current.FillColor);
//    }
//
//    private void HandleClicks()
//    {
//        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
//        {
//            if (IsMouseOver() && OnTopLeft)
//            {
//                Selected = !Selected;
//                Selected?.Invoke(this, EventArgs.Empty);
//            }
//        }
//    }
//}