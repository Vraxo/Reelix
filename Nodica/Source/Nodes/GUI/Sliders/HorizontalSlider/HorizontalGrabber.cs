using Raylib_cs;

namespace Nodica;

public partial class HorizontalSlider
{
    public class HorizontalGrabber : BaseGrabber
    {
        private bool previouslyPressed = false;

        protected override void UpdatePosition(bool initial = false)
        {
            var parent = Parent as BaseSlider;

            if (Pressed)
            {
                float x = Raylib.GetMousePosition().X;
                float y = parent.GlobalPosition.Y;

                GlobalPosition = new(x, y);

                parent.UpdatePercentageBasedOnGrabber();

                previouslyPressed = true;
            }
            else
            {
                if (previouslyPressed)
                {
                    float x = Raylib.GetMousePosition().X;
                    float y = parent.GlobalPosition.Y;

                    GlobalPosition = new(x, y);

                    parent.UpdatePercentageBasedOnGrabber();

                    previouslyPressed = false;
                }
            }

            UpdatePosition(parent, initial);
        }

        private void UpdatePosition(BaseSlider parent, bool initial)
        {
            if (Raylib.IsWindowMinimized())
            {
                return;
            }

            float minX = parent.GlobalPosition.X;
            float maxX = minX + parent.Size.X;

            if (initial && !initialPositionSet)
            {
                GlobalPosition = new(minX, parent.GlobalPosition.Y);
                initialPositionSet = true;
            }
            else
            {
                if (!Pressed)
                {
                    //float _x = maxX * (mainScene.ExternalValue / mainScene.MaxExternalValue);
                    //float _y = GlobalPosition.Y;
                    //
                    //GlobalPosition = new(_x, _y);
                }

                float x = Math.Clamp(GlobalPosition.X, minX, maxX);
                float y = MathF.Ceiling(parent.GlobalPosition.Y);

                GlobalPosition = new(x, y);
            }
        }
    }
}