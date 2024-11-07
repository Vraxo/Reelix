using Raylib_cs;

namespace Nodica;

public partial class VerticalSlider
{
    public class VerticalGrabber : BaseGrabber
    {
        protected override void UpdatePosition(bool initial = false)
        {
            BaseSlider parent = Parent as BaseSlider;

            if (Pressed)
            {
                GlobalPosition = new(parent.GlobalPosition.X, Raylib.GetMousePosition().Y);
                parent.UpdatePercentageBasedOnGrabber();
            }

            UpdatePositionVertical(parent, initial);
        }

        private void UpdatePositionVertical(BaseSlider parent, bool initial)
        {
            if (Raylib.IsWindowMinimized())
            {
                return;
            }

            float minY = parent.GlobalPosition.Y - parent.Offset.Y;
            float maxY = minY + parent.Size.Y;

            if (initial && !initialPositionSet)
            {
                GlobalPosition = new(parent.GlobalPosition.X, minY);
                initialPositionSet = true;
            }
            else
            {
                GlobalPosition = new(parent.GlobalPosition.X, Math.Clamp(GlobalPosition.Y, minY, maxY));
            }
        }
    }
}