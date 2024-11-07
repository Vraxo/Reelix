namespace Nodica;

public class CharacterSet
{
    public static readonly List<char> Numbers = [.. "0123456789".ToCharArray()];
    public static readonly List<char> Mathematics = [.. "0123456789+-x÷()".ToCharArray()];
}