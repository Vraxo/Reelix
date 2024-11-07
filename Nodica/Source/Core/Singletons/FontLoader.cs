using Raylib_cs;

namespace Nodica;

public class FontLoader
{
    public Dictionary<string, Font> Fonts = [];

    private static FontLoader? instance;

    public static FontLoader Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    private FontLoader()
    {
        Load("Resources/Fonts/RobotoMono.ttf", "RobotoMono", 16);
    }

    public void Load(string path, string name, int size)
    {
        // Define a range of Unicode characters to load (e.g., 32 to 255, which includes ASCII and common symbols)
        int[] codepoints = new int[255 - 32 + 1];

        // Fill the array with codepoints from space (32) to 255 (includes most symbols)
        for (int i = 0; i < codepoints.Length; i++)
        {
            codepoints[i] = 32 + i;
        }

        Font font = Raylib.LoadFontEx(path, size, codepoints, codepoints.Length);

        string fullName = $"{name} {size}";

        Fonts.Add(fullName, font);

        Texture2D texture = Fonts[fullName].Texture;
        var filter = TextureFilter.Bilinear;
        Raylib.SetTextureFilter(texture, filter);
    }

    //public void LoadTexture(string path, string name, int size)
    //{
    //    Font font = Raylib.LoadFontEx(path, size, null, 0);
    //    Fonts.Add(name, font);
    //
    //    Texture2D texture = Fonts[name].Texture;
    //    var filter = TextureFilter.Bilinear;
    //    Raylib.SetTextureFilter(texture, filter);
    //}
}