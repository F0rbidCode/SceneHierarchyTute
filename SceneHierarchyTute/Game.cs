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
        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;

        private float deltaTime = 0.005f;

        //create new scene and sprite objects for tank body and tank turret
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            //load the tank imiges into the sprites
            tankSprite.Load("tankBlue_outline.png");
            //rotate the sprite so it faces the right way
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //set an offset for the base of the tank so that it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);
            //tankSprite.SetPosition(50, 50);

            //load the image for the tank turret
            turretSprite.Load("barrelBlue.png");
            //rotate the sprite so it faces the right way
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            //set the scene object hierarchy
            turretObject.AddChild(turretSprite); //set the turret sprite to be a child of the turret object
            tankObject.AddChild(tankSprite); //set the tank sprite to be a child of the tank object
            tankObject.AddChild(turretObject); //set the turret object to be a child of the tank object

            //set the position of the tank to the centre of the sceen
            tankObject.SetPosition(GetScreenWidth()/2.0f, GetScreenHeight()/2.0f);          

        }

        public void Shutdown()
        {

        }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;

            timer += deltaTime;

            if(timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;

            //get user input to move the tank
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if(IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if(IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(
                    tankObject.LocalTransform.m00,
                    tankObject.LocalTransform.m01, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if(IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
                    tankObject.LocalTransform.m00,
                    tankObject.LocalTransform.m01, 1) * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);
            }

            //get user input to move the turret
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }

            //call update on tank object
            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            //call to draw the tank object
            tankObject.Draw();

            EndDrawing();
        }
    }
}
