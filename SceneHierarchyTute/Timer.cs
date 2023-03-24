using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SceneHierarchyTute
{
    internal class Timer
    {
        //variables used for Timer
        Stopwatch stopwatch = new Stopwatch();       
        
        private long currentTime = 0;
        private long lastTime = 0;

        private float deltaTime = 0.005f;

        //variables used to calculate FPS
        private int fps = 1;
        private int frames = 0;
        private float timer = 0;

        public Timer() 
        {
            stopwatch.Start();
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public float Seconds
        {
            get { return stopwatch.ElapsedMilliseconds / 1000.0f; }
        }

        public float GetDeltaTime()
        {
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            return deltaTime;
        }

        public int UpdateFPS(float deltaTime)
        {
            timer += deltaTime;
            frames++;

            if (timer >= 1)
            {
                timer -= 1;
                fps = frames;
                frames = 0;
            }
            return fps;
        }
    }
}
