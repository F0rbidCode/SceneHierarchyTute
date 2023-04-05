using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;


namespace SceneHierarchyTute
{
    internal class AABB
    {
        Vector3 min = new Vector3(float.NegativeInfinity,
                                  float.NegativeInfinity,
                                  float.NegativeInfinity);


        Vector3 max = new Vector3(float.PositiveInfinity,
                                  float.PositiveInfinity,
                                  float.PositiveInfinity);

        public AABB()
        {

        }

        public AABB(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        //fined the centre point of the box
        public Vector3 Center()
        {
            return (min + max) * 0.5f;
        }

        //calculate the Extents of the AABB
        public Vector3 Extents()
        {
            return new Vector3(Math.Abs(max.x - min.x) * 0.5f,
                               Math.Abs(max.y - min.y) * 0.5f,
                               Math.Abs(max.z - min.z) * 0.5f);
        }

        //calculate the corners of the AABB 2D
        public Vector3[] Corners()
        {
            Vector3[] corners = new Vector3[4];
            corners[0] = min;
            corners[1] = new Vector3(min.x, max.y, min.z);
            corners[2] = max;
            corners[3] = new Vector3(max.x, min.y, min.z);
            return corners;
        }

        public void Fit(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }


        //fit the OABB around given points
        public void Fit(Vector3[] points)
        {
            min = new Vector3(float.PositiveInfinity,
                              float.PositiveInfinity,
                              float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity,
                              float.NegativeInfinity,
                              float.NegativeInfinity);

            // find min and max of the points
            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }


        public bool Overlaps(Vector3 p)
        {
            //test for not overlapped as it is faster
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }


       //create a function to draw collision box on screen for debugging
       public void debugBox(Color c)
        {
            DrawRectangle((int)min.x, (int)min.y, (int)max.x - (int)min.x, (int)max.y - (int)min.y, c);
        }
    }
}
