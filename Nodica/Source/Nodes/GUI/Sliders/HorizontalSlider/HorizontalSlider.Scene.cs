namespace Nodica;

public partial class HorizontalSlider : BaseSlider
{
    public override void Build()
    {
        HorizontalGrabber middleButton = new();
        AddChild(middleButton, "Grabber");

        AddChild(new Button
        {
            Position = new(-17.5F, 0),
            Size = new(10, 10),
            Layer = ClickableLayer.PanelButtons,
            OnUpdate = (button) =>
            {
                float x = -button.Size.X * 2;
                float y = button.Position.Y;

                button.Position = new(x, y);
            }
        }, "DecrementButton");

        AddChild(new Button
        {
            Size = new(10, 10),
            Layer = ClickableLayer.PanelButtons,
            OnUpdate = (button) =>
            {
                float x = Size.X + (Grabber.Size.X * 1.25F);
                float y = button.Position.Y;

                button.Position = new(x, y);
            },
        }, "IncrementButton");
    }
}