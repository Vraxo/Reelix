namespace Nodica;

public class CenterContainer : Node2D
{
    public override void Update()
    {
        foreach (Node2D child in Children.Cast<Node2D>())
        {
            child.Position = Size / 2;
        }

        base.Update();
    }
}