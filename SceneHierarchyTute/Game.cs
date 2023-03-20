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

        //create a variable used to adjust bullet speed
        private int bulletSpeed = 200;

        //create a variable to know if shot has occurd
        bool shot = false;

        private int moveDelay = 0;       


        //Create a list to store the treeObjects that are used to make the walls
        List<SceneObject> wallList = new List<SceneObject>();
        //create a vatiable to store the last created x and y positions
        float lastX = 0;
        float lastY = 0;
        

        //create new scene and sprite objects for tank body and tank turret
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        //create new Scene and Sprite objects for bullet
        SceneObject bulletObject = new SceneObject();
        SpriteObject bulletSprite = new SpriteObject();

        //create new Scene and sprite objects for the trees
        SceneObject treeObject = new SceneObject();
        SpriteObject treeSprite = new SpriteObject();
       

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

            //////////////////////////////////////////////////////////////////
            //////TREE WALLS!!!!!!!!!!!
            //////////////////////////////////////////////////////////////////
            {

                //loop through to fill out the top row of wall
                for (int i = 0; lastX < GetScreenWidth(); i++)
                {
                    //create new Scene and sprite objects for the trees
                    SceneObject treeObject = new SceneObject();
                    SpriteObject treeSprite = new SpriteObject();

                    //load the image for the tree
                    treeSprite.Load(@"data\treeGreen_small.png");

                    //set the sprite offset to be in the centre of the tree object                
                    treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                    treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object
                    if (i == 0)
                    {
                        treeObject.SetPosition(0 + (treeSprite.Width / 2.0f), 0 + (treeSprite.Height / 2.0f));
                    }
                    else
                    {
                        treeObject.SetPosition(lastX + (treeSprite.Width), lastY + (treeSprite.Height / 2.0f));
                    }


                    lastX = treeObject.GlobalTransform.m20;

                    wallList.Add(treeObject);
                }

                //loop through to fill out the Right row of wall
                for (int i = 0; lastY < GetScreenHeight(); i++)
                {
                    //create new Scene and sprite objects for the trees
                    SceneObject treeObject = new SceneObject();
                    SpriteObject treeSprite = new SpriteObject();

                    //load the image for the tree
                    treeSprite.Load(@"data\treeGreen_small.png");

                    //set the sprite offset to be in the centre of the tree object                
                    treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                    treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                    if (i == 0)
                    {
                        treeObject.SetPosition(lastX - treeSprite.Width, lastY + (treeSprite.Height * 1.5f));
                    }
                    else
                    {
                        treeObject.SetPosition(lastX - treeSprite.Width, lastY + treeSprite.Height);
                    }
                    lastY = treeObject.GlobalTransform.m21;

                    wallList.Add(treeObject);
                }

                //loop through to fill out the bottom row of wall
                for (int i = 0; lastX > 0; i++)
                {
                    //create new Scene and sprite objects for the trees
                    SceneObject treeObject = new SceneObject();
                    SpriteObject treeSprite = new SpriteObject();

                    //load the image for the tree
                    treeSprite.Load(@"data\treeGreen_small.png");

                    //set the sprite offset to be in the centre of the tree object                
                    treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                    treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object


                    treeObject.SetPosition(lastX - (treeSprite.Width), lastY - treeSprite.Height);



                    lastX = treeObject.GlobalTransform.m20;

                    wallList.Add(treeObject);
                }

                //loop through to fill out the Left row of wall
                for (int i = 0; lastY > 0 + treeSprite.Height; i++)
                {
                    //create new Scene and sprite objects for the trees
                    SceneObject treeObject = new SceneObject();
                    SpriteObject treeSprite = new SpriteObject();

                    //load the image for the tree
                    treeSprite.Load(@"data\treeGreen_small.png");

                    //set the sprite offset to be in the centre of the tree object                
                    treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                    treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object



                    treeObject.SetPosition(lastX + treeSprite.Width, lastY - treeSprite.Height);

                    lastY = treeObject.GlobalTransform.m21;

                    wallList.Add(treeObject);
                }

                

                /////////////////////////////////////////////////////
                ////CURENTLY NEEDED FOR TREE COLLISIONS TO WORK!!!!!!
                /////////////////////////////////////////////////////
                ///// test tree
                /////////////////////////////////////////////////////
                /////
                ////load the image for the tree
                treeSprite.Load(@"data\treeGreen_small.png");
                ////set the sprite offset to be in the centre of the tree object
                treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);
                ////set the position of the tree
                treeObject.SetPosition(0 + (treeSprite.Width / 2.0f), 0 + (treeSprite.Height / 2.0f));
                treeObject.SetPosition(1000 + (treeSprite.Width / 2.0f), 500 + (treeSprite.Height / 2.0f));

            }

            //////////////////////////////////////////////////////////////////
            ///////END WALLS
            //////////////////////////////////////////////////////////////////

            //set the scene object hierarchy
            turretObject.AddChild(turretSprite); //set the turret sprite to be a child of the turret object
            tankObject.AddChild(tankSprite); //set the tank sprite to be a child of the tank object
            tankObject.AddChild(turretObject); //set the turret object to be a child of the tank object

            bulletObject.AddChild(bulletSprite); //set bullet sprite as a child of bullet object
            
            treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object


            //set the position of the tank to the centre of the sceen
            tankObject.SetPosition((treeSprite.Width + tankSprite.Width), GetScreenHeight() - (treeSprite.Width + tankSprite.Width));
            tankObject.Rotate(-90 * (float)(Math.PI / 180.0f));


        }

        public void Shutdown()
        {

        }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;

            timer += deltaTime;

            //increment move delay by 1 every updte if it does not = 0
            if (moveDelay != 0)
            {
                moveDelay++;
                if (moveDelay > 60)
                {
                    moveDelay = 0;
                }
            }

            

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
                //stop the tank from moving if recently ran into wall
                if (moveDelay == 0)
                {
                    Vector3 facing = new Vector3(
                     tankObject.LocalTransform.m00,
                     tankObject.LocalTransform.m01, 1) * deltaTime * 100;
                    tankObject.Translate(facing.x, facing.y);

                    //cycle through the wall list to check if tank colides with wall
                    int i3 = 0;
                    foreach (SceneObject treeObject in wallList)
                    {
                        if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 4.0f) >= wallList[i3].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 4.0f) <= wallList[i3].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                        {
                            if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 4.0f) >= wallList[i3].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 4.0f) <= wallList[i3].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                            {
                                facing = new Vector3(
                             tankObject.LocalTransform.m00,
                             tankObject.LocalTransform.m01, 1) * deltaTime * -2000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter


                            }
                        }
                        //    else
                        //    {
                        //        Vector3 facing = new Vector3(
                        // tankObject.LocalTransform.m00,
                        // tankObject.LocalTransform.m01, 1) * deltaTime * 100;
                        //        tankObject.Translate(facing.x, facing.y);
                        //    }
                        //}
                        //else
                        //{
                        //    Vector3 facing = new Vector3(
                        //  tankObject.LocalTransform.m00,
                        //  tankObject.LocalTransform.m01, 1) * deltaTime * 100;
                        //    tankObject.Translate(facing.x, facing.y);
                        //}
                        i3++;
                    }
                }
                      
            }
            if(IsKeyDown(KeyboardKey.KEY_S))
            {
                //stop the tank from moving if recently ran into wall
                if (moveDelay == 0)
                {
                    Vector3 facing = new Vector3(
                    tankObject.LocalTransform.m00,
                    tankObject.LocalTransform.m01, 1) * deltaTime * -100;
                    tankObject.Translate(facing.x, facing.y);

                    //cycle through the wall list to check if tank colides with wall
                    int i3 = 0;
                    foreach (SceneObject treeObject in wallList)
                    {
                        if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 4.0f) >= wallList[i3].GlobalTransform.m20 - (treeSprite.Width /2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 4.0f) <= wallList[i3].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                        {
                            if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 4.0f) >= wallList[i3].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 4.0f) <= wallList[i3].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                            {
                                facing = new Vector3(
                             tankObject.LocalTransform.m00,
                             tankObject.LocalTransform.m01, 1) * deltaTime * 2000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter


                            }
                        }
                        i3++;
                    }
                }
                
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
            treeObject.Update(deltaTime);

            //call update on tree walls
            foreach (SceneObject treeObject in wallList)
            {
                //draw tree
                treeObject.Update(deltaTime);
            }

            //check if bullet has been shot
            if (shot)
            {
                //update bullets position
                Vector3 facing = new Vector3(
                    bulletObject.LocalTransform.m00,
                    bulletObject.LocalTransform.m01, 1) * deltaTime * bulletSpeed;
                bulletObject.Translate(facing.x, facing.y);
                bulletObject.Update(deltaTime);

                //check if bullet has hit a wall
                if (bulletSprite.GlobalTransform.m20 < 0 + (bulletSprite.Height / 2.0f) || bulletSprite.GlobalTransform.m20 > GetScreenWidth() - (bulletSprite.Height / 2.0f))
                {    
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                    shot = false;//set shot to false so gun can be fired again                  
                }

                if (bulletSprite.GlobalTransform.m21 < 0 + (bulletSprite.Width /2.0f) || bulletSprite.GlobalTransform.m21 > GetScreenHeight() - (bulletSprite.Height / 2.0f))
                {
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    shot = false;//set shot to false so gun can be fired again
                }

                int i = 0;
                foreach (SceneObject treeObject in wallList)
                {                    
                    if (bulletSprite.GlobalTransform.m20 >= wallList[i].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && bulletSprite.GlobalTransform.m20 <= wallList[i].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                    {
                        if (bulletSprite.GlobalTransform.m21 >= wallList[i].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && bulletSprite.GlobalTransform.m21 <= wallList[i].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                        {
                            //load sounds
                            Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                            //play sound
                            PlaySound(ExplodeFX);

                            bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                            shot = false;//set shot to false so gun can be fired again

                            wallList[i].SetPosition(-1000, -1000);
                            //treeObject.SetPosition(-1000, -1000);
                        }

                    }
                    i++;
                }

                //    if (bulletSprite.GlobalTransform.m20 >= treeSprite.GlobalTransform.m20  && bulletSprite.GlobalTransform.m20 <= treeSprite.GlobalTransform.m20 + (treeSprite.Width))
                //{
                //    if (bulletSprite.GlobalTransform.m21 >= treeSprite.GlobalTransform.m21 && bulletSprite.GlobalTransform.m21 <= treeSprite.GlobalTransform.m21 + (treeSprite.Height))
                //    {
                //        //load sounds
                //        Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                //        //play sound
                //        PlaySound(ExplodeFX);

                //        bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                //        shot = false;//set shot to false so gun can be fired again

                //        treeObject.SetPosition(-1000, -1000);
                //    }

                //}
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

            int i2 = 0;
            //draw tree walls
            foreach (SceneObject treeObject in wallList)
            {
                //dont draw tree if object is of screen
                if (wallList[i2].GlobalTransform.m20 > 0)
                {
                    //draw tree
                    wallList[i2].Draw();
                }
                i2++;
            }
            
            
            
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
