namespace Nodica;

public partial class ConfirmDialog : Dialog
{
    public override void Start()
    {
        GetNode<Button>("ConfirmButton").LeftClicked += OnConfirmButtonLeftClicked;
        base.Start();
    }

    protected virtual void OnConfirmButtonLeftClicked(object? sender, EventArgs e)
    {
    }
}