using Raylib_cs;

namespace Nodica;

public class GridContainer : VisualItem
{
    public bool ShowGrid { get; set; } = false;
    public Vector2 Cells { get; set; } = new(4, 5);
    public Vector2 ItemSize { get; set; } = new(80, 64);
    public Vector2 CellOrigin { get; set; } = Vector2.Zero;
    public OriginPreset CellOriginPreset { get; set; } = OriginPreset.Center;

    public Action<GridContainer> OnUpdate = (button) => { };

    public override void Update()
    {
        OnUpdate(this);
        UpdateCellOrigin();
        Draw();
        UpdateLayout();

        Size = ItemSize * Cells * Scale;
        Position = new(Window.Size.X / 2, Window.Size.Y * 0.65f);

        base.Update();
    }

    protected override void Draw()
    {
        if (!ShowGrid)
        {
            return;
        }

        DrawVerticalLines();
        DrawHorizontalLines();
    }

    private void DrawVerticalLines()
    {
        for (int i = 0; i <= Cells.X; i++)
        {
            Vector2 startPosition = GlobalPosition + new Vector2(i * ItemSize.X * Scale.X, 0) - Offset;
            Vector2 endPosition = GlobalPosition + new Vector2(i * ItemSize.X * Scale.X, Cells.Y * ItemSize.Y * Scale.Y) - Offset;

            Raylib.DrawLineV(
                startPosition,
                endPosition,
                Color.White);
        }
    }

    private void DrawHorizontalLines()
    {
        for (int j = 0; j <= Cells.Y; j++)
        {
            // Apply scaling to the grid line positions
            Vector2 startPosition = GlobalPosition + new Vector2(0, j * ItemSize.Y * Scale.Y) - Offset;
            Vector2 endPosition = GlobalPosition + new Vector2(Cells.X * ItemSize.X * Scale.X, j * ItemSize.Y * Scale.Y) - Offset;

            Raylib.DrawLineV(
                startPosition,
                endPosition,
                Color.White);
        }
    }

    private void UpdateLayout()
    {
        int index = 0;

        // Iterate through each child and position them in the grid
        foreach (Node2D child in Children.Cast<Node2D>())
        {
            child.InheritPosition = false;

            int row = index / (int)Cells.X;
            int col = index % (int)Cells.X;

            // Evaluate the position for each child in the grid, applying scaling inversely
            Vector2 childPosition = GlobalPosition - Offset + new Vector2(col * ItemSize.X * Scale.X, row * ItemSize.Y * Scale.Y) + CellOrigin;

            // Set the child's global position
            child.GlobalPosition = childPosition;

            // Increment Index to move to the next cell
            index++;

            // Stop if we run out of cells
            if (index >= Cells.X * Cells.Y)
                break;
        }
    }

    private void UpdateCellOrigin()
    {
        CellOrigin = CellOriginPreset switch
        {
            OriginPreset.Center => (ItemSize / 2) * Scale,
            OriginPreset.CenterLeft => new Vector2(0, ItemSize.Y / 2) * Scale,
            OriginPreset.CenterRight => new Vector2(ItemSize.X, ItemSize.Y / 2) * Scale,
            OriginPreset.TopLeft => new Vector2(0, 0) * Scale,
            OriginPreset.TopRight => new Vector2(ItemSize.X, 0) * Scale,
            OriginPreset.TopCenter => new Vector2(ItemSize.X / 2, 0) * Scale,
            OriginPreset.BottomLeft => new Vector2(0, ItemSize.Y) * Scale,
            OriginPreset.BottomRight => ItemSize * Scale,
            OriginPreset.BottomCenter => new Vector2(ItemSize.X / 2, ItemSize.Y) * Scale,
            OriginPreset.None => Offset * Scale,
            _ => Offset * Scale,
        };
    }
}