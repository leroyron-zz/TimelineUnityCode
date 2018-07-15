using UnityEngine;

namespace TLMath
{
    public partial class TMath
    {
        public static float negInfinity = Mathf.NegativeInfinity;
        public static float infinity = Mathf.Infinity;
        // Converts from degrees to radians.
        public static float Radians(float degrees) {
            return degrees * Mathf.Deg2Rad;
        }
        // Converts from radians to degrees.
        public static float Degrees(float radians) {
            return radians * Mathf.Rad2Deg;
        }
        // Returns a fraction of value.
        public static float Lerp(float val, float frac) {
            return val * frac;
        }
        // Returns a fraction of two comparable value.
        public static float LerpSubject(float val1, float val2, float frac) {
            return Lerp((val1 - val2), frac);
            // Useage - obj.x += TMath.LerpSubject(move, obj.x, obj.speed);
        }
        // Assigns value directly to object property
        public static void LerpProp(object obj, string prop, float val, float frac) {
            float objVal = (float)obj.GetType().GetField(prop).GetValue(obj);
            objVal += LerpSubject(val, objVal, frac);
            obj.GetType().GetField(prop).SetValue(obj, objVal);
            // Useage - TMath.LerpProp(obj, 'x', move, obj.speed);
        }
        // Assigns value directly to multiple object property
        public static void LerpProps(object[] objs, string prop, float val, float frac) {
            for (int oi = 0; oi < objs.Length; oi++) {
                object obj = (object)objs[oi];
                float objVal = (float)obj.GetType().GetField(prop).GetValue(obj);
                objVal += LerpSubject(val, objVal, frac);
                obj.GetType().GetField(prop).SetValue(obj, objVal);
            }
            // Useage - TMath.LerpProp(obj, 'x', move, obj.speed);
        }
        // Get distance
        public static float Distance2(Vector2 p1, Vector2 p2) {
            float a = p1.x - p2.x;
            float b = p1.y - p2.y;
            return Mathf.Sqrt(a * a + b * b);
        }
        // Random number from to
        public static float RandomFromTo(float from, float to) {
            return Random.Range(from, to); //Mathf.Floor((Mathf.Random() * (to - from)) + from);
        }
        // Aspectratio
        public static float AspectRatio(float originalWidth, float originalHeight) {
            return originalWidth / originalHeight;
        }
        public static object RotateFromPoint(float pointX, float pointY, float originX, float originY, float angle, bool clockwise) {
            if (clockwise) angle -= 180f;
            angle = angle * Mathf.Deg2Rad;
            return new {
                x = Mathf.Cos(angle) * (pointX - originX) - Mathf.Sin(angle) * (pointY - originY) + originX,
                y = Mathf.Sin(angle) * (pointX - originX) + Mathf.Cos(angle) * (pointY - originY) + originY
            };
        }

    }
}