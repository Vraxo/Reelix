using Raylib_cs;
using SixLabors.ImageSharp;

namespace Nodica;

public class TextureLoader
{
    public static TextureLoader Instance => instance ??= new();
    private static TextureLoader? instance;

    private Dictionary<string, Texture2D> textures = new();

    private TextureLoader() { }

    public Texture2D Get(string path)
    {
        if (textures.ContainsKey(path))
        {
            return textures[path];
        }

        string pngPath = Path.GetExtension(path).ToLower() == ".png" ? path : ConvertToPng(path);

        textures[path] = Raylib.LoadTexture(pngPath);

        if (pngPath != path)
        {
            File.Delete(pngPath);
        }

        return textures[path];
    }

    public void Remove(string path)
    {
        if (textures.ContainsKey(path))
        {
            Raylib.UnloadTexture(textures[path]);
            textures.Remove(path);

            string pngPath =
                Path.GetExtension(path).ToLower() == ".png" ?
                 path :
                 $"Resources/Temporary/{Path.GetFileNameWithoutExtension(path)}.png";

            if (pngPath != path && File.Exists(pngPath))
            {
                File.Delete(pngPath);
            }
        }
    }

    public void Clear()
    {
        foreach (var texture in textures.Values)
        {
            Raylib.UnloadTexture(texture);
        }
        textures.Clear();
    }

    private string ConvertToPng(string imagePath)
    {
        if (!Directory.Exists("Resources/Temporary"))
        {
            Directory.CreateDirectory("Resources/Temporary");
        }

        string pngPath = $"Resources/Temporary/{Path.GetFileNameWithoutExtension(imagePath)}.png";

        if (!File.Exists(pngPath))
        {
            using var image = SixLabors.ImageSharp.Image.Load(imagePath);
            image.SaveAsPng(pngPath);
        }

        return pngPath;
    }
}