using Raylib_cs;

namespace Nodica;

public partial class BaseSlider
{
    public abstract class BaseGrabber : ClickableRectangle
    {
        public ButtonThemePack Themes = new();
        public bool Pressed = false;
        public Action<BaseGrabber> OnUpdate = (button) => { };

        protected bool alreadyClicked = false;
        protected bool initialPositionSet = false;

        public BaseGrabber()
        {
            Size = new(18, 18);
            InheritPosition = false;
        }

        public override void Start()
        {
            UpdatePosition(true);
            base.Start();
        }

        public override void Update()
        {
            OnUpdate(this);
            UpdatePosition();
            CheckForClicks();
            Draw();
            base.Update();
        }

        protected abstract void UpdatePosition(bool initial = false);

        private void CheckForClicks()
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                if (!IsMouseOver())
                {
                    alreadyClicked = true;
                }
            }

            if (IsMouseOver())
            {
                Themes.Current = Themes.Hover;

                if (Raylib.IsMouseButtonDown(MouseButton.Left) && !alreadyClicked && OnTopLeft)
                {
                    OnTopLeft = false;
                    Pressed = true;
                    alreadyClicked = true;
                }

                if (Pressed)
                {
                    Themes.Current = Themes.Pressed;
                }
            }
            else
            {
                Themes.Current = Themes.Normal;
            }

            if (Pressed)
            {
                Themes.Current = Themes.Pressed;
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                Pressed = false;
                Themes.Current = Themes.Normal;
                alreadyClicked = false;
            }
        }

        protected override void Draw()
        {
            DrawBorderedRectangle(
                GlobalPosition - Origin,
                Size,
                Themes.Current);
        }
    }
}