using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;
using System.Numerics;
using System.Transactions;

namespace SceneHierarchyTute
{
    internal class SpriteObject : SceneObject
    {
        Texture2D texture = new Texture2D();
        Image image = new Image();

        public float Width
        {
            get {  return texture.width; }
        }

        public float Height
        {
            get { return texture.height;  }
        }

        public SpriteObject()
        {

        }

        public void Load(string filename)
        {
            image = LoadImage(filename);
            texture = LoadTextureFromImage(image);
        }

        //create an overriden OnDraw function
        public override void OnDraw()
        {
            //pass the local caxis x and y positions into Atan2
            float rotation = (float)Math.Atan2(globalTransform.m01, globalTransform.m00);

            DrawTextureEx(texture,
                new Vector2(globalTransform.m20, globalTransform.m21), //translation x and y
                rotation * (float)(180.0f / Math.PI),
                1,
                Color.WHITE);        
           
        }
    }
}
