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
    public class Vec2
    {
        public float x, y;

        public Vec2()
        {
        }

        public Vec2(float x, float y)
        {
            Set(x, y);
        }

        public Vec2(Vec2 v)
        {
            Set(v);
        }

        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 Set(Vec2 v)
        {
            x = v.x;
            y = v.y;
            return this;
        }

        /**
         * Negates this vector and returns this.
         */
        public Vec2 Negi()
        {
            return Neg(this);
        }

        /**
         * Sets Out to the negation of this vector and returns Out.
         */
        public Vec2 Neg(Vec2 Out)
        {
            Out.x = -x;
            Out.y = -y;
            return Out;
        }

        /**
         * Returns a new vector that is the negation to this vector.
         */
        public Vec2 Neg()
        {
            return Neg(new Vec2());
        }

        /**
         * Multiplies this vector by s and returns this.
         */
        public Vec2 Muli(float s)
        {
            return Mul(s, this);
        }

        /**
         * Sets Out to this vector multiplied by s and returns Out.
         */
        public Vec2 Mul(float s, Vec2 Out)
        {
            Out.x = s * x;
            Out.y = s * y;
            return Out;
        }

        /**
         * Returns a new vector that is a multiplication of this vector and s.
         */
        public Vec2 Mul(float s)
        {
            return Mul(s, new Vec2());
        }

        /**
         * Divides this vector by s and returns this.
         */
        public Vec2 Divi(float s)
        {
            return Div(s, this);
        }

        /**
         * Sets Out to the division of this vector and s and returns Out.
         */
        public Vec2 Div(float s, Vec2 Out)
        {
            Out.x = x / s;
            Out.y = y / s;
            return Out;
        }

        /**
         * Returns a new vector that is a division between this vector and s.
         */
        public Vec2 Div(float s)
        {
            return Div(s, new Vec2());
        }

        /**
         * Adds s to this vector and returns this. 
         */
        public Vec2 Addi(float s)
        {
            return Add(s, this);
        }

        /**
         * Sets Out to the sum of this vector and s and returns Out.
         */
        public Vec2 Add(float s, Vec2 Out)
        {
            Out.x = x + s;
            Out.y = y + s;
            return Out;
        }

        /**
         * Returns a new vector that is the sum between this vector and s.
         */
        public Vec2 Add(float s)
        {
            return Add(s, new Vec2());
        }

        /**
         * Multiplies this vector by v and returns this.
         */
        public Vec2 Muli(Vec2 v)
        {
            return Mul(v, this);
        }

        /**
         * Sets Out to the product of this vector and v and returns Out.
         */
        public Vec2 Mul(Vec2 v, Vec2 Out)
        {
            Out.x = x * v.x;
            Out.y = y * v.y;
            return Out;
        }

        /**
         * Returns a new vector that is the product of this vector and v.
         */
        public Vec2 Mul(Vec2 v)
        {
            return Mul(v, new Vec2());
        }

        /**
         * Divides this vector by v and returns this.
         */
        public Vec2 Divi(Vec2 v)
        {
            return Div(v, this);
        }

        /**
         * Sets Out to the division of this vector and v and returns Out.
         */
        public Vec2 Div(Vec2 v, Vec2 Out)
        {
            Out.x = x / v.x;
            Out.y = y / v.y;
            return Out;
        }

        /**
         * Returns a new vector that is the division of this vector by v.
         */
        public Vec2 Div(Vec2 v)
        {
            return Div(v, new Vec2());
        }

        /**
         * Adds v to this vector and returns this.
         */
        public Vec2 Addi(Vec2 v)
        {
            return Add(v, this);
        }

        /**
         * Sets Out to the addition of this vector and v and returns Out.
         */
        public Vec2 Add(Vec2 v, Vec2 Out)
        {
            Out.x = x + v.x;
            Out.y = y + v.y;
            return Out;
        }

        /**
         * Returns a new vector that is the addition of this vector and v.
         */
        public Vec2 Add(Vec2 v)
        {
            return Add(v, new Vec2());
        }

        /**
         * Adds v * s to this vector and returns this.
         */
        public Vec2 Addsi(Vec2 v, float s)
        {
            return Adds(v, s, this);
        }

        /**
         * Sets Out to the addition of this vector and v * s and returns Out.
         */
        public Vec2 Adds(Vec2 v, float s, Vec2 Out)
        {
            Out.x = x + v.x * s;
            Out.y = y + v.y * s;
            return Out;
        }

        /**
         * Returns a new vector that is the addition of this vector and v * s.
         */
        public Vec2 Adds(Vec2 v, float s)
        {
            return Adds(v, s, new Vec2());
        }

        /**
         * Subtracts v from this vector and returns this.
         */
        public Vec2 Subi(Vec2 v)
        {
            return Sub(v, this);
        }

        /**
         * Sets Out to the subtraction of v from this vector and returns Out.
         */
        public Vec2 Sub(Vec2 v, Vec2 Out)
        {
            Out.x = x - v.x;
            Out.y = y - v.y;
            return Out;
        }

        /**
         * Returns a new vector that is the subtraction of v from this vector.
         */
        public Vec2 Sub(Vec2 v)
        {
            return Sub(v, new Vec2());
        }

        /**
         * Returns the squared length of this vector.
         */
        public float LengthSq()
        {
            return x * x + y * y;
        }

        /**
         * Returns the length of this vector.
         */
        public float Length()
        {
            return (float)Mathf.Sqrt(x * x + y * y);
        }

        /**
         * Rotates this vector by the given radians.
         */
        public void Rotate(float radians)
        {
            float c = (float)Mathf.Cos(radians);
            float s = (float)Mathf.Sin(radians);

            float xp = x * c - y * s;
            float yp = x * s + y * c;

            x = xp;
            y = yp;
        }

        /**
         * Normalizes this vector, making it a unit vector. A unit vector has a length of 1.0.
         */
        public void Normalize()
        {
            float lenSq = LengthSq();

            if (lenSq > ImpulseMath.EPSILON_SQ)
            {
                float invLen = 1.0f / (float)Mathf.Sqrt(lenSq);
                x *= invLen;
                y *= invLen;
            }
        }

        /**
         * Sets this vector to the minimum between a and b.
         */
        public Vec2 mini(Vec2 a, Vec2 b)
        {
            return Min(a, b, this);
        }

        /**
         * Sets this vector to the maximum between a and b.
         */
        public Vec2 maxi(Vec2 a, Vec2 b)
        {
            return Max(a, b, this);
        }

        /**
         * Returns the dot product between this vector and v.
         */
        public float Dot(Vec2 v)
        {
            return Dot(this, v);
        }

        /**
         * Returns the squared distance between this vector and v.
         */
        public float DistanceSq(Vec2 v)
        {
            return DistanceSq(this, v);
        }

        /**
         * Returns the distance between this vector and v.
         */
        public float Distance(Vec2 v)
        {
            return Distance(this, v);
        }

        /**
         * Sets this vector to the cross between v and a and returns this.
         */
        public Vec2 Cross(Vec2 v, float a)
        {
            return Cross(v, a, this);
        }

        /**
         * Sets this vector to the cross between a and v and returns this.
         */
        public Vec2 Cross(float a, Vec2 v)
        {
            return Cross(a, v, this);
        }

        /**
         * Returns the scalar cross between this vector and v. This is essentially
         * the length of the cross product if this vector were 3d. This can also
         * indicate which way v is facing relative to this vector.
         */
        public float Cross(Vec2 v)
        {
            return Cross(this, v);
        }

        public static Vec2 Min(Vec2 a, Vec2 b, Vec2 Out)
        {
            Out.x = (float)Mathf.Min(a.x, b.x);
            Out.y = (float)Mathf.Min(a.y, b.y);
            return Out;
        }

        public static Vec2 Max(Vec2 a, Vec2 b, Vec2 Out)
        {
            Out.x = (float)Mathf.Max(a.x, b.x);
            Out.y = (float)Mathf.Max(a.y, b.y);
            return Out;
        }

        public static float Dot(Vec2 a, Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static float DistanceSq(Vec2 a, Vec2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;

            return dx * dx + dy * dy;
        }

        public static float Distance(Vec2 a, Vec2 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;

            return (float)Mathf.Sqrt(dx * dx + dy * dy);
        }

        public static Vec2 Cross(Vec2 v, float a, Vec2 Out)
        {
            Out.x = v.y * a;
            Out.y = v.x * -a;
            return Out;
        }

        public static Vec2 Cross(float a, Vec2 v, Vec2 Out)
        {
            Out.x = v.y * -a;
            Out.y = v.x * a;
            return Out;
        }

        public static float Cross(Vec2 a, Vec2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        /**
         * Returns an array of allocated Vec2 of the requested length.
         */
        public static Vec2[] ArrayOf(int length)
        {
            Vec2[] array = new Vec2[length];

            while (--length >= 0)
            {
                array[length] = new Vec2();
            }

            return array;
        }
    }
}