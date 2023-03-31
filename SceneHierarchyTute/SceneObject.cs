using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SceneHierarchyTute
{
    internal class SceneObject
    {
        //create variables to set the SceneObjects perant and children
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

        //add transforms to the SceneObject
        protected Matrix3 localTransform = new Matrix3(1);
        protected Matrix3 globalTransform = new Matrix3(1);
       
        //create a constructor for the SceneObject
        public SceneObject()
        {

        }


        //create a destructor for the SceneObject
        ~SceneObject()
        {
            if(parent != null)
            {
                parent.RemoveChild(this);
            }

            foreach(SceneObject so in children)
            {
                so.parent = null;
            }
        }

        //create a function to return the local transfrom of the game object
        public Matrix3 LocalTransform
        {
            get { return localTransform; }
        }

        //create a function to return the global transfrom of the game object
        public Matrix3 GlobalTransform
        {
            get { return globalTransform; }
        }

        //create a function to return the perant of an object
        public SceneObject Parent
        {
            get { return parent; }
        }

        //create funtion to get number of children
        public int GetChildCount()
        {
            return children.Count;
        }

        //create a function to get a child in the list by an index
        public SceneObject GetChild(int index)
        {
            return children[index];
        }

        //create a function to add new child objects
        public void AddChild(SceneObject child)
        {
            //check to make sure the object doesnt already have a perant
            Debug.Assert(child.parent == null);

            //assign the child's perant to this object
            child.parent = this;

            //add new child to the list of children
            children.Add(child);
        }

        //create a function to remove a child from this object
        public void RemoveChild(SceneObject child)
        {
            if(children.Remove(child) == true)
            {
                child.parent = null;
            }
        }

        //create a virtual function to be overriden by children for when update has been called
        public virtual void OnUpdate(float deltaTime)
        {

        }

        //create a virtual function to be overridden by children for when draw has been called
        public virtual void OnDraw()
        {

        }

        //create a function to Update SceneObject
        public void Update (float deltaTime)
        {
            //call on update first
            OnUpdate(deltaTime);

            //call update on chidren
            foreach (SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }

        //create a function to draw SceneObject
        public void Draw()
        {
            //call OnDraw
            OnDraw();

            //call draw for all children
            foreach (SceneObject child in children)
            {
                child.Draw();
            }
        }

        //create a function to update transforms of the scene object
        void UpdateTransform()
        {
            //check if object has a parent
            if (parent != null) //if it does
                globalTransform = parent.globalTransform * localTransform; //global transfrom = its parents global transform * objects local transform
            else //if it doesnt
                globalTransform = localTransform; // global transform = objects own local transform

            //loop through children of the game object updating their transforms
            foreach (SceneObject child in children)
                child.UpdateTransform();
        }

        //create a function to set the position of a game object
        public void SetPosition (float x, float y)
        {
            localTransform.SetTranslation(x, y);//set the x and y positions of the local stransform with the passed in positions
            UpdateTransform();
        }

        //create a function to set rotation of the object on the z axis
        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }

        //create a function to scale game objects transform
        public void SetScale (float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }

        //create a function to translate objects transform
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }

        //create function to rotate the object arroudn the z axis
        public void Rotate (float radians)
        {
            localTransform.RotateZ(ref localTransform, radians);
            UpdateTransform();
        }

        //create a function to scale the objects transform
        public void Scale(float width, float height)
        {
            localTransform.Scale(ref localTransform, width, height, 1);
            UpdateTransform();
        }

        public float GetAngleTowards (Matrix3 pTransform)
        {
            Vector3 e = new Vector3(globalTransform.m20, globalTransform.m21, globalTransform.m22);
            Vector3 p = new Vector3(pTransform.m20, pTransform.m21, pTransform.m22);
            Vector3 toPlayer = p - e;
            
            float angle = (float)-(Math.Atan2(p.x, p.y) - Math.Atan2(e.x, e.y));
            //float angle = e.AngleBetween(p);
            
            float facing = (float)(Math.Atan2(GlobalTransform.m01, GlobalTransform.m11));
            // angle = angle + (float)(Math.Atan2(pTransform.m01, pTransform.m11) * (180 / Math.PI));



            angle = angle * (float)(180 / Math.PI);
            if (globalTransform.m21 < pTransform.m21 || globalTransform.m21 == pTransform.m21)
                {
                    angle = angle + 55;
                }
            
            angle = angle * (float)(Math.PI / 180);
            angle = angle - facing; 
            



            
            return angle;
        }

        //public void RotateTowards (Matrix3 pTransform, float deltaTime)
        //{
        //    Vector3 e = new Vector3(globalTransform.m20, globalTransform.m21, globalTransform.m22);
        //    Vector3 p = new Vector3(pTransform.m20, pTransform.m21, pTransform.m22);
        //    e.Normalize();
        //    p.Normalize();            
        //    Vector3 a = p - e;

        //    float angle = (float)-(Math.Atan2(a.z, a.x) + Math.PI / 2);
        //    Vector3 Rotate = new Vector3(0, angle, 0);

        //    if (angle > 5)
        //        this.Rotate(deltaTime);

        //}
    }
}
