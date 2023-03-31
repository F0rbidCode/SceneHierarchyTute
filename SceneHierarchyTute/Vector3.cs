using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SceneHierarchyTute
{
    //declair Vector3 as a struct
    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        //create a constructor that takes x y and z positions and sets them to the corrasponding positions in the struct
        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        //functions for creating min and max vectors for the AABB
        public static Vector3 Min(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        }
        public static Vector3 Max(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
        }
        //function to clamp value t byt the min and max values
        public static Vector3 Clamp(Vector3 t, Vector3 a, Vector3 b)
        {
            return Max(a, Min(b, t));
        }

        //create a function to add 2 vector3s together
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        //create a function to subtract a vector 3 from annother
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        //create a fucntion to multiply a vector3 by a scaler
        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }

        //create a fucntion to multiply a scaler by a vector3
        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }

        //create a function to multiply a Vector3 by a Matrix3
        public static Vector3 operator *(Matrix3 M, Vector3 V)
        {
            return new Vector3(V.x * M.m00 + V.y * M.m01 + V.z * M.m02,
                               V.x * M.m10 + V.y * M.m11 + V.z * M.m12,
                               V.x * M.m20 + V.y * M.m21 + V.z * M.m22);
        }

        //create a function to divide a vector3 by a scaler
        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        //create a function to calculate magnitude of a Vector3
        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        //create a function to calculate the squared magnitude of a Vector3
        public float MagnitudeSqr()
        {
            return (x * x + y * y + z * z);
        }

        //create a function to calculate distance between points
        public float Distance(Vector3 other)
        {
            Vector3 diff = this - other;
            return (float)Math.Sqrt(diff.x * diff.x + diff.y + diff.y + diff.z * diff.z);
        }

        //create a function to normalise the Vector3
        public void Normalize()
        {
            float m = Magnitude();
            this.x /= m;
            this.y /= m;
            this.z /= m;
        }

        //create a function that returns a normalised Vector3
        public Vector3 GetNormalised()
        {
            return (this / Magnitude());
        }

        //create a function to return the dot ptoduct of the Vector3
        public float Dot(Vector3 rhs)
        {
            return x * rhs.x + y * rhs.y + z * rhs.z;
        }

        //create a function to return a perpendiculat funtion (RH method)
        public Vector3 GetPerpendicularRH()
        {
            return new Vector3(-y, x, 0);
        }
        //create a function to return a perpendiculat funtion (LH method)
        public Vector3 GetPerpendicularLH()
        {
            return new Vector3(y, -x, 0);
        }

        //create a function to return a purpendicular vector using cross product
        public Vector3 Cross(Vector3 rhs)
        {
            return new Vector3(
                y * rhs.z - z * rhs.y,
                z * rhs.x - x * rhs.z,
                x * rhs.y - y * rhs.x);
        }

        //creare a function to return the angle between two vectors
        public float AngleBetween(Vector3 other)
        {
            //first normalise the vectors
            Vector3 a = GetNormalised();
            Vector3 b = other.GetNormalised();

            //calculate the dot product
            float d = a.x * b.x + a.y * b.y + a.z * b.z;

            //return the agle between them
            return (float)Math.Acos(d);
        }
    }

}
