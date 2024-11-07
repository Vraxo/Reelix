namespace Nodica;

static class ExtensionMethods
{
    public static void Swap<T>(this List<T> list, int index1, int index2)
    {
        (list[index2], list[index1]) = (list[index1], list[index2]);
    }
}