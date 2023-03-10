using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SceneHierarchyTute
{   
        //declair Matrix3 as a struct
        public struct Matrix3
        {
            public float m00, m01, m02;
            public float m10, m11, m12;
            public float m20, m21, m22;

            //create a constructor taking individual elements and asigning them
            public Matrix3(float _m00, float _m01, float _m02, float _m10, float _m11, float _m12, float _m20, float _m21, float _m22)
            {
                m00 = _m00;
                m01 = _m01;
                m02 = _m02;
                m10 = _m10;
                m11 = _m11;
                m12 = _m12;
                m20 = _m20;
                m21 = _m21;
                m22 = _m22;
            }
           
          

            //create a copying constructor
            public Matrix3(Matrix3 m)
            {
                m00 = m.m00; m01 = m.m01; m02 = m.m02;
                m10 = m.m10; m11 = m.m11; m12 = m.m12;
                m20 = m.m20; m21 = m.m21; m22 = m.m22;
            }

            //create a constructor to create a uniform scaling matrix
            public Matrix3(float uniformScale)
            {
                m00 = m11 = m22 = uniformScale;
                m01 = m02 = m10 = m12 = m20 = m21 = 0;
            }

            //create a function to construct a Matrix3 Identity Matrix
            public Matrix3 IdentityMatrix3()
            {
                Matrix3 IdentityM3 = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
                return IdentityM3;
            }

            //create a function to multiply a Matrix3 by another Matrix3
            public static Matrix3 operator *(Matrix3 M2, Matrix3 M1)
            {
                return new Matrix3(
                    M1.m00 * M2.m00 + M1.m01 * M2.m10 + M1.m02 * M2.m20, M1.m00 * M2.m01 + M1.m01 * M2.m11 + M1.m02 * M2.m21, M1.m00 * M2.m02 + M1.m01 * M2.m12 + M1.m02 * M2.m22,
                    M1.m10 * M2.m00 + M1.m11 * M2.m10 + M1.m12 * M2.m20, M1.m10 * M2.m01 + M1.m11 * M2.m11 + M1.m12 * M2.m21, M1.m10 * M2.m02 + M1.m11 * M2.m12 + M1.m12 * M2.m22,
                    M1.m20 * M2.m00 + M1.m21 * M2.m10 + M1.m22 * M2.m20, M1.m20 * M2.m01 + M1.m21 * M2.m11 + M1.m22 * M2.m21, M1.m20 * M2.m02 + M1.m21 * M2.m12 + M1.m22 * M2.m22);
            }

            //create a function to transpose a Matrix3
            public Matrix3 Transpose(Matrix3 m)
            {
                //flip the Matrix on its diagonal
                Matrix3 newM = new Matrix3();
                newM.m00 = m.m00; newM.m01 = m.m10; newM.m02 = m.m20;
                newM.m10 = m.m01; newM.m11 = m.m11; newM.m12 = m.m21;
                newM.m20 = m.m02; newM.m21 = m.m12; newM.m22 = m.m22;

                return newM;
            }

            //create a function to create a scaled Matrix3 based on 3 passed in floats
            public void SetScaled(float x, float y, float z)
            {
                m00 = x; m01 = 0; m02 = 0;
                m10 = 0; m11 = y; m12 = 0;
                m20 = 0; m21 = 0; m22 = z;
            }

            //create a cuntion to scale a Matrix3 based on 3 passed in floats
            public void Scale(float x, float y, float z)
            {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);
            this = this * m;
            }

           
            //create a function to create a rotation matrix3 to rotate around the X axis
            public void SetRotateX(double radians)
            {
                m00 = 1; m01 = 0; m02 = 0;
                m10 = 0; m11 = (float)Math.Cos(radians); m12 = (float)Math.Sin(radians);
                m20 = 0; m21 = (float)-Math.Sin(radians); m22 = (float)Math.Cos(radians);
            }

            //create a function to create a rotation matrix3 to rotate around the Y axis
            public void SetRotateY(double radians)
            {
                m00 = (float)Math.Cos(radians); m01 = 0; m02 = (float)-Math.Sin(radians);
                m10 = 0; m11 = 1; m12 = 0;
                m20 = (float)Math.Sin(radians); m21 = 0; m22 = (float)Math.Cos(radians);
            }


            //create a function to create a rotation matrix3 to rotate around the Z axis
            public void SetRotateZ(double radians)
            {
                m00 = (float)Math.Cos(radians); m01 = (float)Math.Sin(radians); m02 = 0;
                m10 = (float)-Math.Sin(radians); m11 = (float)Math.Cos(radians); m12 = 0;
                m20 = 0; m21 = 0; m22 = 1;
            }

            //create a function to rotate aroudn the z axis
            public void RotateZ(double radians)
            {
                Matrix3 m = new Matrix3();
                m.SetRotateZ(radians);

                this = this * m;            
            }

            //create a funtion to set a Euler roation
            public Matrix3 SetEuler(float pitch, float yaw, float roll)
            {
                //create new Matrix3s to hold the x y and z rotations and the final Euler rotation 
                Matrix3 x = new Matrix3();
                Matrix3 y = new Matrix3();
                Matrix3 z = new Matrix3();
                Matrix3 Eu = new Matrix3();

                //set x y and z Matrix3 to appropriate rotation Matrix3
                x.SetRotateX(pitch);
                y.SetRotateY(yaw);
                z.SetRotateZ(roll);

                //multiply the 3 Matrix3s together starting with z then y then x to get our final Euler Matrix3
                Eu = (z * y * x);
                return Eu;
            }

            //create a function to set stranslation information in Matrix3
            public void SetTranslation(float x, float y)
            {
                m20 = x; m21 = y; m22 = 1;
            }

            //create a function to translate a Matrix3
            public void Translate(float x, float y)
            {
                m20 += x; m21 += y;
            }
        }
    }
