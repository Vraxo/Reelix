namespace Nodica;

public class AspectRatioContainer : Node2D
{
    public override void Update()
    {
        foreach (Node2D child in Children.Cast<Node2D>())
        {
            Vector2 center = Size / 2;

            Vector2 textureSize = child.Size;

            Vector2 totalRatio = Size / textureSize;
            float ratio = totalRatio.X < totalRatio.Y ? totalRatio.X : totalRatio.Y;

            Vector2 newSize = textureSize * ratio;

            child.Size = newSize;
            //child.position = center;
        }

        base.Update();
    }
}