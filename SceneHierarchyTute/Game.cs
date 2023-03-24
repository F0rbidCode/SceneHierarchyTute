using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace SceneHierarchyTute
{
    internal class Game
    {
        Timer timer = new Timer();        
        private int fps = 1;    
        private float deltaTime = 0.005f;

        public void Init()
        {
            
        }

        public void Shutdown()
        {

        }

        public void Update()
        {
            deltaTime = timer.GetDeltaTime();
            fps = timer.UpdateFPS(deltaTime);
            

            
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);


            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            EndDrawing();
        }
    }
}
