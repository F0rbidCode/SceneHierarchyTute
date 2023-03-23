using Raylib_cs;
using static Raylib_cs.Raylib;




namespace SceneHierarchyTute
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Raylib.SetTargetFPS(60);
            Raylib.InitWindow(1920, 1080, "Tanks for Everything!");

            

            //initialize the audio device
            InitAudioDevice();
            //load the game music
            Music music = LoadMusicStream(@"data\Casual Arcade Track #3 (looped).wav");
            //play the music
            PlayMusicStream(music);
            SetMusicVolume(music, 0.25f);

            




            game.Init();

            while (!Raylib.WindowShouldClose())
            {
                //update music stream
                UpdateMusicStream(music);              
               

                game.Update();                
                game.Draw();


            }

            game.Shutdown();
            Raylib.CloseWindow();
        }
    }
}