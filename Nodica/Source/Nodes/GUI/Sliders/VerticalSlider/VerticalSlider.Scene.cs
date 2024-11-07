namespace Nodica;

public partial class VerticalSlider : BaseSlider
{
    public override void Build()
    {
        VerticalGrabber grabber = new();
        AddChild(grabber, "Grabber");

        AddChild(new Button
        {
            Position = new(0, grabber.Size.Y * -1.5f),
            Size = new(10, 10),
            Layer = ClickableLayer.PanelButtons,
            OnUpdate = (button) =>
            {
                float x = button.Position.X;
                float y = -Offset.Y - grabber.Size.Y * 1.5f;

                button.Position = new(x, y);
            }
        }, "DecrementButton");

        AddChild(new Button
        {
            Size = new(10, 10),
            Layer = ClickableLayer.PanelButtons,
            OnUpdate = (button) =>
            {
                float x = button.Position.X;
                float y = Size.Y - Offset.Y + grabber.Size.Y * 1.5f - 1;

                button.Position = new(x, y);

            },
        }, "IncrementButton");
    }
}