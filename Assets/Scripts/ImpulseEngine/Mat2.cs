/*
    Copyright (c) 2013 Randy Gaul http://RandyGaul.net

    This software is provided 'as-is', without any express or implied
    warranty. In no event will the authors be held liable for any damages
    arising from the use of this software.

    Permission is granted to anyone to use this software for any purpose,
    including commercial applications, and to alter it and redistribute it
    freely, subject to the following restrictions:
      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.
      
    Port to Java by Philip Diffenderfer http://magnos.org - Port to C# Unity by Leroy Thompson http://leroy.ron@gmail.com
*/
using UnityEngine;

public partial class ImpulseEngine
{
    public class Mat2
    {

        public float m00, m01;
        public float m10, m11;

        public Mat2()
        {
        }

        public Mat2(float radians)
        {
            Set(radians);
        }

        public Mat2(float a, float b, float c, float d)
        {
            Set(a, b, c, d);
        }

        /**
         * Sets this matrix to a rotation matrix with the given radians.
         */
        public void Set(float radians)
        {
            float c = (float)Mathf.Cos(radians);
            float s = (float)Mathf.Sin(radians);

            m00 = c;
            m01 = -s;
            m10 = s;
            m11 = c;
        }

        /**
         * Sets the values of this matrix.
         */
        public void Set(float a, float b, float c, float d)
        {
            m00 = a;
            m01 = b;
            m10 = c;
            m11 = d;
        }

        /**
         * Sets this matrix to have the same values as the given matrix.
         */
        public void Set(Mat2 m)
        {
            m00 = m.m00;
            m01 = m.m01;
            m10 = m.m10;
            m11 = m.m11;
        }

        /**
         * Sets the values of this matrix to their absolute value.
         */
        public void Absi()
        {
            Abs(this);
        }

        /**
         * Returns a new matrix that is the absolute value of this matrix.
         */
        public Mat2 Abs()
        {
            return Abs(new Mat2());
        }

        /**
         * Sets Out to the absolute value of this matrix.
         */
        public Mat2 Abs(Mat2 Out)
        {
            Out.m00 = Mathf.Abs(m00);
            Out.m01 = Mathf.Abs(m01);
            Out.m10 = Mathf.Abs(m10);
            Out.m11 = Mathf.Abs(m11);
            return Out;
        }

        /**
         * Sets Out to the x-axis (1st column) of this matrix.
         */
        public Vec2 GetAxisX(Vec2 Out)
        {
            Out.x = m00;
            Out.y = m10;
            return Out;
        }

        /**
         * Returns a new vector that is the x-axis (1st column) of this matrix.
         */
        public Vec2 GetAxisX()
        {
            return GetAxisX(new Vec2());
        }

        /**
         * Sets Out to the y-axis (2nd column) of this matrix.
         */
        public Vec2 GetAxisY(Vec2 Out)
        {
            Out.x = m01;
            Out.y = m11;
            return Out;
        }

        /**
         * Returns a new vector that is the y-axis (2nd column) of this matrix.
         */
        public Vec2 GetAxisY()
        {
            return GetAxisY(new Vec2());
        }

        /**
         * Sets the matrix to it's transpose.
         */
        public void Transposei()
        {
            float t = m01;
            m01 = m10;
            m10 = t;
        }

        /**
         * Sets Out to the transpose of this matrix.
         */
        public Mat2 Transpose(Mat2 Out)
        {
            Out.m00 = m00;
            Out.m01 = m10;
            Out.m10 = m01;
            Out.m11 = m11;
            return Out;
        }

        /**
         * Returns a new matrix that is the transpose of this matrix.
         */
        public Mat2 Transpose()
        {
            return Transpose(new Mat2());
        }

        /**
         * Transforms v by this matrix.
         */
        public Vec2 Muli(Vec2 v)
        {
            return Mul(v.x, v.y, v);
        }

        /**
         * Sets Out to the transformation of v by this matrix.
         */
        public Vec2 Mul(Vec2 v, Vec2 Out)
        {
            return Mul(v.x, v.y, Out);
        }

        /**
         * Returns a new vector that is the transformation of v by this matrix.
         */
        public Vec2 Mul(Vec2 v)
        {
            return Mul(v.x, v.y, new Vec2());
        }

        /**
         * Sets Out the to transformation of {x,y} by this matrix.
         */
        public Vec2 Mul(float x, float y, Vec2 Out)
        {
            Out.x = m00 * x + m01 * y;
            Out.y = m10 * x + m11 * y;
            return Out;
        }

        /**
         * Multiplies this matrix by x.
         */
        public void Muli(Mat2 x)
        {
            Set(
                m00 * x.m00 + m01 * x.m10,
                m00 * x.m01 + m01 * x.m11,
                m10 * x.m00 + m11 * x.m10,
                m10 * x.m01 + m11 * x.m11);
        }

        /**
         * Sets Out to the multiplication of this matrix and x.
         */
        public Mat2 Mul(Mat2 x, Mat2 Out)
        {
            Out.m00 = m00 * x.m00 + m01 * x.m10;
            Out.m01 = m00 * x.m01 + m01 * x.m11;
            Out.m10 = m10 * x.m00 + m11 * x.m10;
            Out.m11 = m10 * x.m01 + m11 * x.m11;
            return Out;
        }

        /**
         * Returns a new matrix that is the multiplication of this and x.
         */
        public Mat2 Mul(Mat2 x)
        {
            return Mul(x, new Mat2());
        }

    }
}