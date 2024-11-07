using Raylib_cs;
using System.Reflection;

namespace Nodica;

public class App
{
    public static App Instance => instance ??= new();
    private static App? instance;

    public Node RootNode;

    private App() { }

    public void Initialize(int width, int height, string title)
    {
        Window.OriginalSize = new(width, height);

        SetCurrentDirectory();
        SetWindowFlags();

        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowMinSize(width, height);
        Raylib.InitAudioDevice();
        Raylib.SetWindowIcon(Raylib.LoadImage("Resources/Icon/Icon.png"));
    }

    public void SetRootNode(Node node, bool packedScene = false)
    {
        RootNode = node;

        if (!packedScene)
        {
            RootNode.Build();
        }

        //RootNode.Start();
    }

    public void Run()
    {
        //RootNode.Build();
        //RootNode.Start();

        RunLoop();
    }

    private static void SetCurrentDirectory()
    {
        string assemblyLocation = Assembly.GetEntryAssembly().Location;
        Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);
    }

    private static void SetWindowFlags()
    {
        Raylib.SetConfigFlags(
            ConfigFlags.VSyncHint |
            ConfigFlags.Msaa4xHint |
            ConfigFlags.HighDpiWindow |
            ConfigFlags.ResizableWindow |
            ConfigFlags.AlwaysRunWindow);
    }

    private void RunLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(DefaultTheme.Background);
            UpdateSingletons();
            RootNode.Process();
            Raylib.EndDrawing();

            PrintTree();
        }
    }

    private void UpdateSingletons()
    {
        ClickManager.Instance.Update();
    }

    private void PrintTree()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Console.Clear();
            RootNode.PrintChildren();

            //Random random = new();
            //int r = random.Next(1000);
            //Raylib.TakeScreenshot($"Screenshot{r}.png");
        }
    }
}