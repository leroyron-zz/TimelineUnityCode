using UnityEngine;

namespace TLMath
{
    public partial class TMath
    {
        // Converts from degrees to radians.
        public static float radians (float degrees) {
            return degrees * Mathf.Deg2Rad;
        }
        // Converts from radians to degrees.
        public static float degrees (float radians) {
            return radians * Mathf.Rad2Deg;
        }
        // Returns a fraction of value.
        public static float lerp (float val, float frac) {
            return val * frac;
        }
        // Returns a fraction of two comparable value.
        public static float lerpSubject (float val1, float val2, float frac) {
            return lerp((val1 - val2), frac);
            // Useage - obj.x += TMath.lerpSubject(move, obj.x, obj.speed);
        }
        // Assigns value directly to object property
        public static void lerpProp (object obj, string prop, float val, float frac) {
            float objVal = (float)obj.GetType().GetField(prop).GetValue(obj);
            objVal += lerpSubject(val, objVal, frac);
            obj.GetType().GetField(prop).SetValue(obj, objVal);
            // Useage - TMath.lerpProp(obj, 'x', move, obj.speed);
        }
        // Assigns value directly to multiple object property
        public static void lerpProps (object[] objs, string prop, float val, float frac) {
            for (int oi = 0; oi < objs.Length; oi++) {
                object obj = (object)objs[oi];
                float objVal = (float)obj.GetType().GetField(prop).GetValue(obj);
                objVal += lerpSubject(val, objVal, frac);
                obj.GetType().GetField(prop).SetValue(obj, objVal);
            }
            // Useage - TMath.lerpProp(obj, 'x', move, obj.speed);
        }
        // Get distance
        public static float distance2 (Vector2 p1, Vector2 p2) {
            float a = p1.x - p2.x;
            float b = p1.y - p2.y;
            return Mathf.Sqrt(a * a + b * b);
        }
        // Random number from to
        public static float randomFromTo (float from, float to) {
            return Random.Range(from, to); //Mathf.Floor((Mathf.Random() * (to - from)) + from);
        }
        // Aspectratio
        public static float aspectRatio (float originalWidth, float originalHeight) {
            return originalWidth / originalHeight;
        }
        public static object rotateFromPoint (float pointX, float pointY, float originX, float originY, float angle, bool clockwise) {
            if (clockwise) angle -= 180f;
            angle = angle * Mathf.Deg2Rad;
            return new {
                x = Mathf.Cos(angle) * (pointX - originX) - Mathf.Sin(angle) * (pointY - originY) + originX,
                y = Mathf.Sin(angle) * (pointX - originX) + Mathf.Cos(angle) * (pointY - originY) + originY
            };
        }

    }
}