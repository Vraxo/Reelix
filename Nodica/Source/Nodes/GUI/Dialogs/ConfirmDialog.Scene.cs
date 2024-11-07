namespace Nodica;

public partial class ConfirmDialog : Dialog
{
    public override void Build()
    {
        base.Build();

        AddChild(new Button
        {
            Position = new(0, 50),
            Layer = ClickableLayer.DialogButtons,
            Text = "Confirm",
        }, "ConfirmButton");
    }
}