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
    public class ImpulseMath
    {
        public static readonly float PI = (float)Mathf.PI;
        public static readonly float EPSILON = 0.0001f;
        public static readonly float EPSILON_SQ = EPSILON * EPSILON;
        public static readonly float BIAS_RELATIVE = 0.95f;
        public static readonly float BIAS_ABSOLUTE = 0.01f;
        public static float DT = 1.0f / 60.0f;
        public static readonly Vec2 GRAVITY = new Vec2(0.0f, -50.0f);
        public static float RESTING = GRAVITY.Mul(DT).LengthSq() + EPSILON;
        public static readonly float PENETRATION_ALLOWANCE = 0.05f;
        public static readonly float PENETRATION_CORRETION = 0.4f;

        public static bool Equal(float a, float b)
        {
            return Mathf.Abs(a - b) <= EPSILON; //StrictMath.abs( a - b ) <= EPSILON;
        }

        public static float Clamp(float min, float max, float a)
        {
            return (a < min ? min : (a > max ? max : a));
        }

        public static int Round(float a)
        {
            return (int)(a + 0.5f);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, (max - min)); //(float)((max - min) * Math.random() + min);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, (max - min + 1)); //(int)((max - min + 1) * Math.random() + min);
        }

        public static bool Gt(float a, float b)
        {
            return a >= b * BIAS_RELATIVE + a * BIAS_ABSOLUTE;
        }
    }
}