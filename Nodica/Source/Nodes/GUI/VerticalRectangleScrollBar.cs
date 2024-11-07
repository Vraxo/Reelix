//using Raylib_cs;
//
//Nodica Nodica;
//
//class VerticalRectangleScrollBar : Node2D
//{
//    public Action<VerticalRectangleScrollBar> OnUpdate = (slider) => { };
//    public OptionButtonButton Grabber;
//    public OptionButtonButton TopButton;
//    public OptionButtonButton BottomButton;
//
//    private float _value = 0;
//
//    public float Value
//    {
//        get => _value;
//
//        set
//        {
//            _value = value;
//            _value = Math.Clamp(_value, 0, 1);
//        }
//    }
//
//    public override void Ready()
//    {
//        AddTopButton();
//        AddBottomButton();
//        AddMiddleButton();
//    }
//
//    public override void Update()
//    {
//        Value = Grabber.position.Y / (Size.Y - Grabber.Size.Y);
//        BottomButton.position.Y = Size.Y + 8;
//
//        DrawRectangle();
//        SnapMiddleButtonToClickedPosition();
//
//        OnUpdate(this);
//
//        base.Update();
//    }
//
//    private void OnTopButtonLeftClicked(object? sender, EventArgs e)
//    {
//        Grabber.position.Y -= Grabber.Size.Y;
//        Value -= BottomButton.Size.Y / Size.Y;
//    }
//
//    private void DrawRectangle()
//    {
//        Rectangle rectangle = new()
//        {
//            position = GlobalPosition - Offset,
//            Size = Size
//        };
//        
//        Raylib.DrawRectangleRounded(rectangle, 0.0F, (int)Size.X, FillColor.DarkGray);
//    }
//
//    private void AddTopButton()
//    {
//        TopButton = new()
//        {
//            position = new(0, -8),
//            Size = new(Size.X, 10),
//        };
//
//        TopButton.OnLeftClick += OnTopButtonLeftClicked;
//
//        AddChild(TopButton);
//    }
//
//    private void AddBottomButton()
//    {
//        BottomButton = new()
//        {
//            position = new(0, Size.Y),
//            Size = new(Size.X, 10),
//        };
//
//        BottomButton.OnLeftClick += OnBottomButtonLeftClicked;
//
//        AddChild(BottomButton);
//    }
//
//    private void OnBottomButtonLeftClicked(object? sender, EventArgs e)
//    {
//        Grabber.position.Y += Grabber.Size.Y;
//        Value += BottomButton.Size.Y / Size.Y;
//    }
//
//    private void AddMiddleButton()
//    {
//        Grabber = new()
//        {
//            Size = new(Size.X, Size.X),
//            Offset = new(Size.X / 2, 0),
//            OriginPreset = OriginPreset.TopCenter,
//            OnUpdate = (OptionButtonButton) =>
//            {
//                if (OptionButtonButton.PressedLeft)
//                {
//                    OptionButtonButton.position.Y = Raylib.GetMousePosition().Y - OptionButtonButton.Size.Y;
//                }
//
//                float maxY = Size.Y - OptionButtonButton.Size.Y;
//
//                if (OptionButtonButton.position.Y > maxY)
//                {
//                    OptionButtonButton.position.Y = maxY;
//                }
//
//                if (OptionButtonButton.position.Y < 0)
//                {
//                    OptionButtonButton.position.Y = 0;
//                }
//            }
//        };
//
//        AddChild(Grabber);
//
//        Grabber.FilledStyle.Normal.FillColor = new(128, 128, 128, 255);
//    }
//
//    private void SnapMiddleButtonToClickedPosition()
//    {
//        if (IsMouseOver() && Raylib.IsMouseButtonPressed(MouseButton.Left))
//        {
//            Grabber.position.Y = Raylib.GetMousePosition().Y - Grabber.Size.Y;
//        }
//    }
//
//    private bool IsMouseOver()
//    {
//        Vector2 mousePosition = Raylib.GetMousePosition();
//
//        bool matchX1 = mousePosition.X > GlobalPosition.X - Offset.X;
//        bool matchX2 = mousePosition.X < GlobalPosition.X + Size.X - Offset.X;
//
//        bool matchY1 = mousePosition.Y > GlobalPosition.Y - Offset.Y;
//        bool matchY2 = mousePosition.Y < GlobalPosition.Y + Size.Y - Offset.Y;
//
//        return matchX1 && matchX2 && matchY1 && matchY2;
//    }
//}