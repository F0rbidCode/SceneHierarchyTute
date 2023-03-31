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
        //Stopwatch stopwatch = new Stopwatch();        

        //private long currentTime = 0;
        //private long lastTime = 0;
        //private float timer = 0;
        private int fps = 1;
        //private int frames;

        //private float deltaTime = 0.005f;


        Timer timer = new Timer();

        //create a variable used to adjust bullet speed
        private int bulletSpeed = 200;

        //create a variable to know if shot has occurd
        bool shot = false;
        private int smokeTimer = 0;
        bool isDead = false;

        private int moveDelay = 0;

        //create a variable to set at the end of the level
        private bool end = false;

        //create a variable to know if shot has occurd
        bool eShot = false;
        private int eSmokeTimer = 0;
        bool eIsDead = false;


        //Create a list to store the treeObjects that are used to make the walls
        List<SceneObject> wallList = new List<SceneObject>();
        //create a vatiable to store the last created x and y positions
        float lastX = 0;
        float lastY = 0;


        //create new scene and sprite objects for tank body and tank turret
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        OABB tankOABB = new OABB();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();

        //create new Scene and Sprite objects for bullet
        SceneObject bulletObject = new SceneObject();
        SpriteObject bulletSprite = new SpriteObject();

        //create new Scene and sprite objects for the trees
        SceneObject treeObject = new SceneObject();
        SpriteObject treeSprite = new SpriteObject();

        //create scene and sprite objects for smoke
        SceneObject smokeObject = new SceneObject();
        SpriteObject smokeSprite = new SpriteObject();

        //create the scene and sprite objects for the end point
        SceneObject endObject = new SceneObject();
        SpriteObject endSprite = new SpriteObject();

        //create a list to set out the FIN screen
        List<SceneObject> finList = new List<SceneObject>();

        ////////////////////////////////////////////////
        /// Enemy tank objects
        ////////////////////////////////////////////////    
        //create new scene and sprite objects for tank body and tank turret
        SceneObject eTankObject = new SceneObject();
        SceneObject eTurretObject = new SceneObject();

        SpriteObject eTankSprite = new SpriteObject();
        SpriteObject eTurretSprite = new SpriteObject();

        //create new Scene and Sprite objects for bullet
        SceneObject eBulletObject = new SceneObject();
        SpriteObject eBulletSprite = new SpriteObject();

        //create scene and sprite objects for smoke
        SceneObject eSmokeObject = new SceneObject();
        SpriteObject eSmokeSprite = new SpriteObject();



        public void Init()
        {
            //stopwatch.Start();
            //lastTime = stopwatch.ElapsedMilliseconds;

            

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

            //load the image for the end
            endSprite.Load(@"data\explosion1.png");
            //set the offset to be centred
            endSprite.SetPosition(-endSprite.Width / 2.0f, -endSprite.Height / 2.0f);

            ///////////////////////////////////////////////////////
            ///eney tank
            /////////////////////////////////////////////////////
            {
                //load the tank imiges into the sprites
                eTankSprite.Load(@"data\tankGreen_outline.png");
                //rotate the sprite so it faces the right way
                eTankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
                //set an offset for the base of the tank so that it rotates around the centre
                eTankSprite.SetPosition(-eTankSprite.Width / 2.0f, eTankSprite.Height / 2.0f);


                //load the image for the tank turret
                eTurretSprite.Load(@"data\barrelGreen_outline.png");
                //rotate the sprite so it faces the right way
                eTurretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
                // set the turret offset from the tank base
                eTurretSprite.SetPosition(0, eTurretSprite.Width / 2.0f);
            }

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

                ///////////////////////////////////////////////////
                ///Fil in the maze
                ///////////////////////////////////////////////////

                /// Rows starting from bottom of screen                
                lastY = 0;
                lastX = 0;
                for (int j = 0; lastX  < GetScreenWidth() - (tankSprite.Height * 8); j++)
                {
                    if (j != 0)
                    {
                        lastX = lastX + (tankSprite.Width * 4) + treeSprite.Width;
                    }

                    
                    for (int i = 0; lastY < GetScreenHeight() - (tankSprite.Height * 2); i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        //on the first loop
                        if (i == 0)
                        {
                            treeObject.SetPosition(lastX+ (tankSprite.Width + (treeSprite.Width * 2.0f)), 0 + (treeSprite.Height * 3.5f));
                        }
                        else
                        {
                            treeObject.SetPosition(lastX + (tankSprite.Width + (treeSprite.Width * 2.0f)), lastY + treeSprite.Height);
                        }


                        lastY = treeObject.GlobalTransform.m21;

                        wallList.Add(treeObject);
                    }

                    //reset lastY to 0
                    lastY = 0;                 
                    
                }

                /// Rows starting from top of screen     
                lastY = 0;
                lastX = 0;
                for (int j = 0; lastX < GetScreenWidth() - (tankSprite.Height * 4); j++)
                {
                    if (j != 0)
                    {
                        lastX = lastX + (tankSprite.Width * 4) + treeSprite.Width;
                    }


                    for (int i = 0; lastY < GetScreenHeight() - (tankSprite.Height * 4); i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        //on the first loop
                        if (i == 0)
                        {
                            treeObject.SetPosition(lastX + ((tankSprite.Width * 3) + (treeSprite.Width * 2.0f)), 0 + (treeSprite.Height * 1.5f));
                        }
                        else
                        {
                            treeObject.SetPosition(lastX + ((tankSprite.Width * 3)+ (treeSprite.Width * 2.0f)), lastY + treeSprite.Height);
                        }


                        lastY = treeObject.GlobalTransform.m21;

                        wallList.Add(treeObject);
                    }

                    //reset lastY to 0
                    lastY = 0;

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

            
            //set the scene object hierarchy
            turretObject.AddChild(turretSprite); //set the turret sprite to be a child of the turret object
            tankObject.AddChild(tankSprite); //set the tank sprite to be a child of the tank object
            tankObject.AddChild(turretObject); //set the turret object to be a child of the tank object
            tankObject.AddChild(tankOABB); // set the oabb to a child of the tank

            bulletObject.AddChild(bulletSprite); //set bullet sprite as a child of bullet object
            
            treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

            smokeObject.AddChild(smokeSprite);//set the smoke sprite as a child of smoke object

            endObject.AddChild(endSprite); //add endSprite as a chiled of the end Object

            //set the position of the tank to the centre of the sceen
            tankObject.SetPosition((treeSprite.Width + (tankSprite.Width / 1.5f)), GetScreenHeight() - (treeSprite.Width + tankSprite.Width));
            tankObject.Rotate(-90 * (float)(Math.PI / 180.0f));

            //set the position of the end Object
            endObject.SetPosition(GetScreenWidth() - (treeSprite.Width /2.0f + (endSprite.Width / 2.0f) ), 0 + (treeSprite.Height + (endSprite.Height / 2.0f)));

            //set the scene object hierarchy for enemy tank
            eTurretObject.AddChild(eTurretSprite); //set the turret sprite to be a child of the turret object
            eTankObject.AddChild(eTankSprite); //set the tank sprite to be a child of the tank object
            eTankObject.AddChild(eTurretObject); //set the turret object to be a child of the tank object

            eBulletObject.AddChild(eBulletSprite); //set bullet sprite as a child of bullet object

            eSmokeObject.AddChild(eSmokeSprite);//set the smoke sprite as a child of smoke object

            //set the position of the tank to the centre of the sceen
            eTankObject.SetPosition(eTankSprite.Width + treeSprite.Width, eTankSprite.Height + treeSprite.Height);
            eTankObject.Rotate(-90 * (float)(Math.PI / 180.0f));

            ///////////////////////////////////////////////////////////////
            ///FIN SCREEN
            ///////////////////////////////////////////////////////////////
            {
                if (!end)
                {
                    //set up the first line in the I
                    lastX = 0;
                    lastY = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        //on the first loop
                        if (i == 0)
                        {
                            treeObject.SetPosition(GetScreenWidth() / 2.0f, GetRenderHeight() / 2.0f);
                        }
                        else
                        {
                            treeObject.SetPosition(GetScreenWidth() / 2.0f, lastY - treeSprite.Height);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }
                    //set the top of the I
                    float mX = lastX; //used to come back to the top centre of the I
                    for (int i = 0; i < 3; i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        treeObject.SetPosition(lastX - treeSprite.Width, lastY);

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//Left
                    float UIX = lastX;
                    float UIY = lastY; // used to get back the the upper left of the I
                    for (int i = 0; i < 3; i++)
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
                            treeObject.SetPosition(mX + treeSprite.Width, lastY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX + treeSprite.Width, lastY);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//Right
                    for (int i = 0; i < 5; i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        //on the first loop
                        if (i == 0)
                        {
                            treeObject.SetPosition(GetScreenWidth() / 2.0f, (GetRenderHeight() / 2.0f) + treeSprite.Height);
                        }
                        else
                        {
                            treeObject.SetPosition(GetScreenWidth() / 2.0f, lastY + treeSprite.Height);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//start the lower half of the I
                    for (int i = 0; i < 3; i++)
                    {
                        //create new Scene and sprite objects for the trees
                        SceneObject treeObject = new SceneObject();
                        SpriteObject treeSprite = new SpriteObject();

                        //load the image for the tree
                        treeSprite.Load(@"data\treeGreen_small.png");

                        //set the sprite offset to be in the centre of the tree object                
                        treeSprite.SetPosition(-treeSprite.Width / 2.0f, -treeSprite.Height / 2.0f);

                        treeObject.AddChild(treeSprite);//set the tree sprite as a child of tree object

                        treeObject.SetPosition(lastX - treeSprite.Width, lastY);

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//Left
                    for (int i = 0; i < 3; i++)
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
                            treeObject.SetPosition(mX + treeSprite.Width, lastY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX + treeSprite.Width, lastY);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//Right                    
                    float LIX = lastX;
                    float LIY = lastY; //used to get back to the lower right spot on the I

                    for (int i = 0; i < 5; i++)
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
                            treeObject.SetPosition(UIX - (treeSprite.Width * 2), UIY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX - treeSprite.Width, lastY);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//top line on F
                    UIY = lastY; //used to get back to the upper left of the F
                    for (int i = 0; i < 5; i++)
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
                            treeObject.SetPosition(UIX - (treeSprite.Width * 2), UIY + (treeSprite.Height * 3));
                        }
                        else
                        {
                            treeObject.SetPosition(lastX - treeSprite.Width, lastY);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//lower line on F
                    UIX = lastX; //used to get back to the upper left of the F
                    for (int i = 0; i < 10; i++)
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
                            treeObject.SetPosition(UIX - (treeSprite.Width), UIY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX, lastY + treeSprite.Width);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//virtical line of the F


                    //Start the N
                    for (int i = 0; i < 10; i++)
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
                            treeObject.SetPosition(LIX + (treeSprite.Width * 2), LIY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX, lastY - treeSprite.Width);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//first line of N

                    for (int i = 0; i < 9; i++)
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
                            treeObject.SetPosition(lastX + (treeSprite.Width), lastY + (treeSprite.Width /2.0f));
                        }
                        else
                        {
                            treeObject.SetPosition(lastX + (treeSprite.Width / 2.0f), lastY + treeSprite.Width);
                        }

                        
                        

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//second line of N

                    for (int i = 0; i < 10; i++)
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
                            treeObject.SetPosition(lastX + (treeSprite.Width), LIY);
                        }
                        else
                        {
                            treeObject.SetPosition(lastX, lastY - treeSprite.Width);
                        }

                        lastX = treeObject.GlobalTransform.m20;
                        lastY = treeObject.GlobalTransform.m21;

                        finList.Add(treeObject);
                    }//last line of N


                    lastX = 0;
                    lastY = 0;
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

                        finList.Add(treeObject);
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

                        finList.Add(treeObject);
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

                        finList.Add(treeObject);
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

                        finList.Add(treeObject);
                    }
                }
            }


        }

        public void Shutdown()
        {

        }

        public void Update()
        {
            //currentTime = stopwatch.ElapsedMilliseconds;
            //deltaTime = (currentTime - lastTime) / 1000.0f;

            //timer += deltaTime;
            //if(timer >= 1)
            //{
            //    fps = frames;
            //    frames = 0;
            //    timer -= 1;
            //}
            //frames++;

            float deltaTime = timer.GetDeltaTime();
            fps = timer.UpdateFPS(deltaTime);

            


            //increment move delay by 1 every updte if it does not = 0
            if (moveDelay != 0)
            {
                moveDelay++;
                if (moveDelay > 60)
                {
                    moveDelay = 0;
                }
            }

            if (smokeTimer != 0)
            {
                smokeTimer++;
            }
            ////////////////////////////////////////////////////////////////////////////////////
            //////MOVENET
            ///////////////////////////////////////////////////////////////////////////////////
            {
                if (!isDead)
                {
                    //get user input to move the tank
                    if (IsKeyDown(KeyboardKey.KEY_A))
                    {
                        tankObject.Rotate(-deltaTime);
                    }
                    if (IsKeyDown(KeyboardKey.KEY_D))
                    {
                        tankObject.Rotate(deltaTime);
                    }
                    if (IsKeyDown(KeyboardKey.KEY_W))
                    {
                        //stop the tank from moving if recently ran into wall
                        if (moveDelay == 0)
                        {
                            Vector3 facing = new Vector3(
                             tankObject.LocalTransform.m00,
                             tankObject.LocalTransform.m01, 1) * deltaTime * 100;
                            tankObject.Translate(facing.x, facing.y);

                            if (!end)
                            {
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

                            if (end)
                            {
                                
                                int i3 = 0;                                
                                foreach (SceneObject treeObject in finList)
                                {
                                    if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 4.0f) >= finList[i3].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 4.0f) <= finList[i3].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                                    {
                                        if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 4.0f) >= finList[i3].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 4.0f) <= finList[i3].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                                        {
                                                facing = new Vector3(
                                             tankObject.LocalTransform.m00,
                                             tankObject.LocalTransform.m01, 1) * deltaTime * -2000;
                                                tankObject.Translate(facing.x, facing.y);

                                                moveDelay = 1;//start move delay counter


                                         }
                                    }
                                    i3++;
                                }

                                //stop the player running into the other tank
                                if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 2.0f) >= eTankSprite.GlobalTransform.m20 - (eTankSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 2.0f) <= eTankSprite.GlobalTransform.m20 + (eTankSprite.Width / 2.0f))
                                {
                                    if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 2.0f) >= eTankSprite.GlobalTransform.m21 - (eTankSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 2.0f) <= eTankObject.GlobalTransform.m21 + (eTankSprite.Height / 2.0f))
                                    {
                                        facing = new Vector3(
                                     tankObject.LocalTransform.m00,
                                     tankObject.LocalTransform.m01, 1) * deltaTime * -2000;
                                        tankObject.Translate(facing.x, facing.y);

                                        moveDelay = 1;//start move delay counter


                                    }
                                }

                            }

                            //stop the plaer leaving the sceen
                            if (tankSprite.GlobalTransform.m20 < 0 + (tankSprite.Width / 2.0f) || tankSprite.GlobalTransform.m20 > GetScreenWidth() - (tankSprite.Width / 2.0f))
                            {
                                facing = new Vector3(
                                tankObject.LocalTransform.m00,
                                tankObject.LocalTransform.m01, 1) * deltaTime * -6000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter
                            }

                            if (tankSprite.GlobalTransform.m21 < 0 + (tankSprite.Height / 4.0f) || tankSprite.GlobalTransform.m21 > GetScreenHeight() - (tankSprite.Height / 2.0f))
                            {
                                facing = new Vector3(
                                tankObject.LocalTransform.m00,
                                tankObject.LocalTransform.m01, 1) * deltaTime * -6000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter
                            }

                            
                        }
                    }

                    if (IsKeyDown(KeyboardKey.KEY_S))
                    {
                        //stop the tank from moving if recently ran into wall
                        if (moveDelay == 0)
                        {
                            Vector3 facing = new Vector3(
                            tankObject.LocalTransform.m00,
                            tankObject.LocalTransform.m01, 1) * deltaTime * -100;
                            tankObject.Translate(facing.x, facing.y);


                            if (!end)
                            {
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
                                         tankObject.LocalTransform.m01, 1) * deltaTime * 2000;
                                            tankObject.Translate(facing.x, facing.y);

                                            moveDelay = 1;//start move delay counter


                                        }
                                    }
                                    i3++;
                                }

                            }

                            if (end)
                            {
                                int i3 = 0;
                                foreach (SceneObject treeObject in finList)
                                {
                                    if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 4.0f) >= finList[i3].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 4.0f) <= finList[i3].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                                    {
                                        if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 4.0f) >= finList[i3].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 4.0f) <= finList[i3].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
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

                                //stop the player running into the other tank
                                if (tankObject.GlobalTransform.m20 + (tankSprite.Width / 2.0f) >= eTankSprite.GlobalTransform.m20 - (eTankSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 - (tankSprite.Width / 2.0f) <= eTankSprite.GlobalTransform.m20 + (eTankSprite.Width / 2.0f))
                                {
                                    if (tankObject.GlobalTransform.m21 + (tankSprite.Width / 2.0f) >= eTankSprite.GlobalTransform.m21 - (eTankSprite.Height / 2.0f) && tankObject.GlobalTransform.m21 - (tankSprite.Width / 2.0f) <= eTankObject.GlobalTransform.m21 + (eTankSprite.Height / 2.0f))
                                    {
                                        facing = new Vector3(
                                     tankObject.LocalTransform.m00,
                                     tankObject.LocalTransform.m01, 1) * deltaTime * 2000;
                                        tankObject.Translate(facing.x, facing.y);

                                        moveDelay = 1;//start move delay counter


                                    }
                                }
                            }

                            //stop player from leaving screen
                            if (tankSprite.GlobalTransform.m20 < 0 - (tankSprite.Width / 2.0f) || tankSprite.GlobalTransform.m20 > GetScreenWidth() + (tankSprite.Width / 2.0f))
                            {
                                facing = new Vector3(
                                tankObject.LocalTransform.m00,
                                tankObject.LocalTransform.m01, 1) * deltaTime * 6000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter
                            }

                            if (tankSprite.GlobalTransform.m21 < 0 - (tankSprite.Height / 2.0f) || tankSprite.GlobalTransform.m21 > GetScreenHeight() + (tankSprite.Height / 2.0f))
                            {
                                facing = new Vector3(
                                tankObject.LocalTransform.m00,
                                tankObject.LocalTransform.m01, 1) * deltaTime * 6000;
                                tankObject.Translate(facing.x, facing.y);

                                moveDelay = 1;//start move delay counter
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

                        Console.WriteLine("m00" + tankObject.GlobalTransform.m00 + " m01" + tankObject.GlobalTransform.m01 + " m02" + tankObject.GlobalTransform.m02);
                        Console.WriteLine("m10" + tankObject.GlobalTransform.m10 + " m11" + tankObject.GlobalTransform.m11 + " m12" + tankObject.GlobalTransform.m12);
                        Console.WriteLine("m20" + tankObject.GlobalTransform.m20 + " m21" + tankObject.GlobalTransform.m21 + " m22" + tankObject.GlobalTransform.m22);

                    }

                    //get user inpuit to fire bullet
                    if (IsKeyDown(KeyboardKey.KEY_SPACE))
                    {
                        if (!shot)
                        {
                            Shoot();
                            shot = true;
                        }
                    }
                }
            }

            //call update on tank object
            tankObject.Update(deltaTime);
            eTankObject.Update(deltaTime);
            treeObject.Update(deltaTime);

            /////////////////////////////////////////////////////////////////
            //////FIT OABB
            /////////////////////////////////////////////////////////////////
            {
                Vector3[] point = new Vector3[4];
                Vector3 p0 = new Vector3(tankObject.GlobalTransform.m20 - (tankSprite.Height / 2), tankSprite.GlobalTransform.m21, 1);
                point[0] = p0;

                Vector3 p1 = new Vector3(tankSprite.GlobalTransform.m20 + (tankSprite.Height / 2), tankSprite.GlobalTransform.m21, 1);
                point[1] = p1;

                Vector3 p2 = new Vector3 (tankSprite.GlobalTransform.m20, tankSprite.GlobalTransform.m21 - (tankSprite.Width / 2), 1);
                point[2] = p2;

                Vector3 p3 = new Vector3(tankSprite.GlobalTransform.m20, tankSprite.GlobalTransform.m21 + (tankSprite.Width / 2), 1);
                point[3]= p3;

                tankOABB.Fit(point);
            }

            ////call update on tree walls
            //foreach (SceneObject treeObject in wallList)
            //{
            //    //draw tree
            //    treeObject.Update(deltaTime);
            //}

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

                    smokeSprite.SetPosition(-smokeSprite.Width / 2.0f, -smokeSprite.Height / 2.0f);
                    smokeObject.SetPosition(bulletSprite.GlobalTransform.m20, bulletSprite.GlobalTransform.m21);
                    smokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                             //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    smokeTimer = 1;
                    shot = false;//set shot to false so gun can be fired again                  
                }

                if (bulletSprite.GlobalTransform.m21 < 0 + (bulletSprite.Width / 2.0f) || bulletSprite.GlobalTransform.m21 > GetScreenHeight() - (bulletSprite.Height / 2.0f))
                {
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    smokeSprite.SetPosition(-smokeSprite.Width / 2.0f, -smokeSprite.Height / 2.0f);
                    smokeObject.SetPosition(bulletSprite.GlobalTransform.m20, bulletSprite.GlobalTransform.m21);
                    smokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                             //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    smokeTimer = 1;
                    shot = false;//set shot to false so gun can be fired again
                }

                if (!end)
                {
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

                                smokeSprite.SetPosition(-smokeSprite.Width / 2.0f, -smokeSprite.Height / 2.0f);
                                smokeObject.SetPosition(bulletSprite.GlobalTransform.m20, bulletSprite.GlobalTransform.m21);
                                smokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke

                                smokeTimer = 1;
                                shot = false;//set shot to false so gun can be fired again

                                wallList[i].SetPosition(-1000, -1000);
                            }

                        }
                        i++;
                    }
                }

                if (end)
                {
                    int i = 0;
                    foreach (SceneObject treeObject in finList)
                    {
                        if (bulletSprite.GlobalTransform.m20 >= finList[i].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && bulletSprite.GlobalTransform.m20 <= finList[i].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                        {
                            if (bulletSprite.GlobalTransform.m21 >= finList[i].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && bulletSprite.GlobalTransform.m21 <= finList[i].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                            {
                                //load sounds
                                Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                                //play sound
                                PlaySound(ExplodeFX);

                                smokeSprite.SetPosition(-smokeSprite.Width / 2.0f, -smokeSprite.Height / 2.0f);
                                smokeObject.SetPosition(bulletSprite.GlobalTransform.m20, bulletSprite.GlobalTransform.m21);
                                smokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke

                                smokeTimer = 1;
                                shot = false;//set shot to false so gun can be fired again

                                finList[i].SetPosition(-1000, -1000);
                            }

                        }
                        i++;
                    }
                }

                if (end)
                {
                    if (bulletSprite.GlobalTransform.m20 >= eTankSprite.GlobalTransform.m20 - (eTankSprite.Width / 2.0f) && bulletSprite.GlobalTransform.m20 <= eTankSprite.GlobalTransform.m20 + (eTankSprite.Width / 2.0f))
                    {
                        if (bulletSprite.GlobalTransform.m21 >= eTankSprite.GlobalTransform.m21 - (eTankSprite.Height / 2.0f) && bulletSprite.GlobalTransform.m21 <= eTankSprite.GlobalTransform.m21 + (eTankSprite.Height / 2.0f))
                        {
                            //load sounds
                            Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                            //play sound
                            PlaySound(ExplodeFX);

                            smokeSprite.SetPosition(-smokeSprite.Width / 2.0f, -smokeSprite.Height / 2.0f);
                            smokeObject.SetPosition(bulletSprite.GlobalTransform.m20, bulletSprite.GlobalTransform.m21);
                            smokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                                     //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                            smokeTimer = 1;

                            eTankObject.SetPosition(-1000, -1000);

                            eIsDead = true;
                            shot = false;
                        }

                    }
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

            //check if eney has shot
            if (eShot)
            {
                //update bullets position
                Vector3 facing = new Vector3(
                    eBulletObject.LocalTransform.m00,
                    eBulletObject.LocalTransform.m01, 1) * deltaTime * bulletSpeed;
                eBulletObject.Translate(facing.x, facing.y);
                eBulletObject.Update(deltaTime);

                //check if bullet has hit a wall
                if (eBulletSprite.GlobalTransform.m20 < 0 + (eBulletSprite.Height / 2.0f) || eBulletSprite.GlobalTransform.m20 > GetScreenWidth() - (eBulletSprite.Height / 2.0f))
                {
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    eSmokeSprite.SetPosition(-eSmokeSprite.Width / 2.0f, -eSmokeSprite.Height / 2.0f);
                    eSmokeObject.SetPosition(eBulletSprite.GlobalTransform.m20, eBulletSprite.GlobalTransform.m21);
                    eSmokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                              //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    eSmokeTimer = 1;
                    eShot = false;//set shot to false so gun can be fired again                  
                }

                if (eBulletSprite.GlobalTransform.m21 < 0 + (eBulletSprite.Width / 2.0f) || eBulletSprite.GlobalTransform.m21 > GetScreenHeight() - (eBulletSprite.Height / 2.0f))
                {
                    //load sounds
                    Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                    //play sound
                    PlaySound(ExplodeFX);

                    eSmokeSprite.SetPosition(-eSmokeSprite.Width / 2.0f, -eSmokeSprite.Height / 2.0f);
                    eSmokeObject.SetPosition(eBulletSprite.GlobalTransform.m20, eBulletSprite.GlobalTransform.m21);
                    eSmokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                              //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                    eSmokeTimer = 1;
                    eShot = false;//set shot to false so gun can be fired again
                }

                int i = 0;
                foreach (SceneObject treeObject in finList)
                {
                    if (eBulletSprite.GlobalTransform.m20 >= finList[i].GlobalTransform.m20 - (treeSprite.Width / 2.0f) && eBulletSprite.GlobalTransform.m20 <= finList[i].GlobalTransform.m20 + (treeSprite.Width / 2.0f))
                    {
                        if (eBulletSprite.GlobalTransform.m21 >= finList[i].GlobalTransform.m21 - (treeSprite.Height / 2.0f) && eBulletSprite.GlobalTransform.m21 <= finList[i].GlobalTransform.m21 + (treeSprite.Height / 2.0f))
                        {
                            //load sounds
                            Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                            //play sound
                            PlaySound(ExplodeFX);

                            eSmokeSprite.SetPosition(-eSmokeSprite.Width / 2.0f, -eSmokeSprite.Height / 2.0f);
                            eSmokeObject.SetPosition(eBulletSprite.GlobalTransform.m20, eBulletSprite.GlobalTransform.m21);
                            eSmokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                                      //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                            eSmokeTimer = 1;
                            eShot = false;//set shot to false so gun can be fired again

                            finList[i].SetPosition(-1000, -1000);
                            //treeObject.SetPosition(-1000, -1000);
                        }

                    }
                    i++;
                }

                //if (eBulletSprite.GlobalTransform.m20 > tankSprite.GlobalTransform.m20 - (tankSprite.Width) && eBulletSprite.GlobalTransform.m20 < tankSprite.GlobalTransform.m20 + (tankSprite.Width / 6.0f))
                {
                    // if (eBulletSprite.GlobalTransform.m21 > tankSprite.GlobalTransform.m21 - (tankSprite.Height ) && eBulletSprite.GlobalTransform.m21 < tankSprite.GlobalTransform.m21 + (tankSprite.Height / 6.0f))
                    Vector3 b = new Vector3(eBulletSprite.GlobalTransform.m20, eBulletSprite.GlobalTransform.m21, eBulletSprite.GlobalTransform.m22);
                    if  (tankOABB.Overlaps(b))
                    {
                        //load sounds
                        Sound ExplodeFX = LoadSound(@"data\mixkit-arcade-game-explosion-2759.wav");
                        //play sound
                        PlaySound(ExplodeFX);

                        eSmokeSprite.SetPosition(-eSmokeSprite.Width / 2.0f, -eSmokeSprite.Height / 2.0f);
                        eSmokeObject.SetPosition(eBulletSprite.GlobalTransform.m20, eBulletSprite.GlobalTransform.m21);
                        eSmokeSprite.Load(@"data\smokeGrey4.png");//change bullet to smoke
                                                                  //bulletSprite.Load(@"data\smokeGrey4.png");//change bullet to smokea
                        eSmokeTimer = 1;
                        eShot = false;//set shot to false so gun can be fired again

                        tankObject.SetPosition(-1000, -1000);

                        isDead = true;

                    }

                }

            }

            //lastTime = currentTime;

            //check if tank has made it to the end
            if (tankObject.GlobalTransform.m20 > endObject.GlobalTransform.m20 - (endSprite.Width / 2.0f) && tankObject.GlobalTransform.m20 < endObject.GlobalTransform.m20 + (endSprite.Width / 2.0f))
            {
                if (tankObject.GlobalTransform.m21 > endObject.GlobalTransform.m21 - (endSprite.Width / 2.0f) && tankObject.GlobalTransform.m21 < endObject.GlobalTransform.m21 + (endSprite.Width / 2.0f))
                {
                    end = true;
                }
            }


            if (end)
            {
               
                

                /////////////////////////////////////////////////////////////////////////
                ////////Rotate enemy tank to face player
                //////////////////////////////////////////////////////////////////////////
                {
                    ///rotate the enemy tank to face the player
                    float angleTo = eTankObject.GetAngleTowards(tankObject.GlobalTransform);
                    //eTankObject.Rotate(angleTo);
                    if (angleTo > 0)
                    {
                        eTankObject.Rotate(deltaTime);
                    }
                    if (angleTo < 0)
                    {
                        eTankObject.Rotate(-deltaTime);
                    }
                    if (angleTo > -0.5 && angleTo < 0.5 && eShot == false && !isDead && !eIsDead)
                    {
                        EnemyShoot();
                        eShot = true;
                    }

                    //eTankObject.RotateTowards(tankObject.GlobalTransform, deltaTime);
                }
            }
           
        }


        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.BLACK);


            if (!end)
            {

                //call to draw the tank object
                tankObject.Draw();

                //draw the endObject
                endObject.Draw();                

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
            }

            //check if shot has been fired
            if (shot)
            {
                //draw bullet
                bulletObject.Draw();
            }

            if (smokeTimer > 0)
            {
                smokeTimer++;
                smokeObject.Draw();

                if (smokeTimer > 60)
                {
                    smokeTimer = 0;
                }
            }

            if (eShot)
            {
                eBulletObject.Draw();
            }

            if (eSmokeTimer > 0)
            {
                eSmokeTimer++;
                eSmokeObject.Draw();

                if (eSmokeTimer > 60)
                {
                    eSmokeTimer = 0;
                }
            }        

            if (end)
            {
                int i = 0;
                foreach (SceneObject treeObject in finList)
                {
                    finList[i].Draw();
                    i++;
                }

                tankObject.Draw();
                eTankObject.Draw();                
            }

            DrawText(fps.ToString(), 10, 10, 20, Color.RED);
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

        public void EnemyShoot()
        {
            //load the image for the bullet sprite
            eBulletSprite.Load(@"data\bulletBlueSilver_outline.png");
            //set positions and rotation for bullet sprite
            eBulletSprite.SetRotate(90 * (float)(Math.PI / 180.0f));
            eBulletSprite.SetPosition(eBulletSprite.Height + eTurretSprite.Height, eBulletSprite.Width / 2 - eTurretSprite.Width);

            //create a variable to store rotation of turret
            float rad = (float)Math.Atan2(eTurretObject.GlobalTransform.m01, eTurretObject.GlobalTransform.m11);
            //set rotation of bullet object to be the same as turret
            eBulletObject.SetRotate(rad);
            //set the location of the bullet spawn
            eBulletObject.SetPosition(eTurretObject.GlobalTransform.m20, eTurretObject.GlobalTransform.m21);

            //load sounds
            Sound shootFX = LoadSound(@"data\mixkit-empty-tube-hit-3197.wav");
            //play sound
            PlaySound(shootFX);
        }
    }
}
