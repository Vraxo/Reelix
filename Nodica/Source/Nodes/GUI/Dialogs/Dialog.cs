namespace Nodica;

public partial class Dialog : Node2D
{
    public override void Start()
    {
        Offset = GetNode<ColorRectangle>("Background").Size / 2;
        ClickManager.Instance.MinLayer = ClickableLayer.DialogButtons;
        GetNode<Button>("CloseButton").LeftClicked += OnCloseButtonLeftClicked;
    }

    public override void Update()
    {
        UpdatePosition();
    }

    protected void Close()
    {
        ClickManager.Instance.MinLayer = 0;
        Destroy();
    }

    private void OnCloseButtonLeftClicked(object? sender, EventArgs e)
    {
        Close();
    }

    private void UpdatePosition()
    {
        Position = Window.Size / 2;
    }
}