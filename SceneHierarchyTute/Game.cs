﻿using System;
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

        //create a variable to know if shot has occurd
        bool shot = false;
        

        
       

        //create new scene and sprite objects for tank body and tank turret
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        //create new Scene and Sprite objects for bullet
        SceneObject bulletObject = new SceneObject();
        SpriteObject bulletSprite = new SpriteObject();
       

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;


            //load the tank imiges into the sprites
            tankSprite.Load(@"data\tankBlue_outline.png");
            //rotate the sprite so it faces the right way
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            //set an offset for the base of the tank so that it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);
            

            //load the image for the tank turret
            turretSprite.Load(@"data\barrelBlue.png");
            //rotate the sprite so it faces the right way
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // set the turret offset from the tank base
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            

            //set the scene object hierarchy
            turretObject.AddChild(turretSprite); //set the turret sprite to be a child of the turret object
            tankObject.AddChild(tankSprite); //set the tank sprite to be a child of the tank object
            tankObject.AddChild(turretObject); //set the turret object to be a child of the tank object

            bulletObject.AddChild(bulletSprite);
            //turretObject.AddChild(bulletObject);

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

            //get user inpuit to fire bullet
            if (IsKeyDown (KeyboardKey.KEY_SPACE))
            {
                if (!shot)
                {
                    Shoot();
                    
                    

                    shot = true;
                }
               
            }

            //call update on tank object
            tankObject.Update(deltaTime);
            //check if bullet has been shot
            if (shot)
            {
                //update bullets position
                Vector3 facing = new Vector3(
                    bulletObject.LocalTransform.m00,
                    bulletObject.LocalTransform.m01, 1) * deltaTime * 100;
                bulletObject.Translate(facing.x, facing.y);
                bulletObject.Update(deltaTime);

                //check if bullet has hit a wall
                if (bulletSprite.GlobalTransform.m20 < 0 + bulletSprite.Height || bulletSprite.GlobalTransform.m20 > GetScreenWidth() - bulletSprite.Height )
                {    
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                    shot = false;//set shot to false so gun can be fired again                  
                }

                if (bulletSprite.GlobalTransform.m21 < 0 + bulletSprite.Height  || bulletSprite.GlobalTransform.m21 > GetScreenHeight() - bulletSprite.Height)
                {
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    shot = false;//set shot to false so gun can be fired again
                }
                
            }

            lastTime = currentTime;
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.BLACK);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            //call to draw the tank object
            tankObject.Draw();
            
            //draw bullet
            bulletObject.Draw();
            
            
            EndDrawing();
        }

        //create a function to spawn and shoot a bullet when the space bar is pressed
        public void Shoot()
        {
            //load the image for the bullet sprite
            bulletSprite.Load(@"data\bulletBlueSilver_outline.png");
           //set positions and rotation for bullet sprite
            bulletSprite.SetRotate(90 * (float)(Math.PI / 180.0f));
            bulletSprite.SetPosition(bulletSprite.Height + turretSprite.Height, bulletSprite.Width / 2 - turretSprite.Width);
             

            //create a variable to store rotation of turret
            float rad = (float)Math.Atan2(turretObject.GlobalTransform.m01, turretObject.GlobalTransform.m11);
            //set rotation of bullet object to be the same as turret
            bulletObject.SetRotate(rad);
            //set the location of the bullet spawn
            bulletObject.SetPosition(turretObject.GlobalTransform.m20, turretObject.GlobalTransform.m21);
            
            //load sounds
            Sound shootFX = LoadSound(@"data\mixkit-empty-tube-hit-3197.wav");
            //play sound
            PlaySound(shootFX);

        }
    }
}
