using Raylib_cs;

namespace SceneHierarchyTute
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Raylib.SetTargetFPS(60);
            Raylib.InitWindow(640, 480, "Tanks for Everything!");

            game.Init();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();
            Raylib.CloseWindow();
        }
    }
}