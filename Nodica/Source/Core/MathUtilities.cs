namespace Nodica;

class MathUtilities
{
    public static float GetDistance(Vector2 point1, Vector2 point2)
    {
        float xDistance = MathF.Pow(point2.X - point1.X, 2);
        float yDistance = MathF.Pow(point2.Y - point1.Y, 2);

        float distance = MathF.Sqrt(xDistance + yDistance);

        return distance;
    }
}