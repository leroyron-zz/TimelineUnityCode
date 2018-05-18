using System;
using System.Collections.Generic;
using UnityEngine;
using TLExtensions;

namespace TLExtensions
{
    // Extend Func
    public delegate TResult Func<T1, T2, T3, T4, T5, TResult>
    (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    public delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>
        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>
        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
    //Extension methods must be defined in a static class
    public static class FloatExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static float at(this int value)
        {
            return value;
        }
    }    
}

class Exec {
    private object parameter;
    public static string execBinding;
    public static int execByteOffset;
    public static int execData0PosI;
    public static int execLastOffset;
    public static float execLastVal;
    //public static Exec execNode;
    public static float value;
    public static float[] array;
    public static Func<float, string, int, object, bool, bool, float, bool, object, object> assign;
    public static Func<float, string, int, object, bool, bool, float, bool, object, object> then;
    public static Func<object, bool, bool, float, bool, object, object> keep;
    public static Func<int, object, bool, bool, float, bool, object, object> wait;
    public static Func<int, object, bool, bool, float, bool, object, object> hold;
    public static Func<int, object, bool, bool, float, bool, object, object> pair;
    public static Func<int, object, bool, bool, float, bool, object, object> shift;
    public static Func<int, object, bool, bool, float, bool, object, object> stop;
    public static Func<float[], object, bool, bool, float, bool, object, object> at;
    public static Func<float, int, int, object> flood;
    public Exec (TIMELINE.CODE.TLVector3 parameter) {
        this.parameter = parameter;
    }
    public Exec (TIMELINE.CODE.TLPoly parameter) {
        this.parameter = parameter;
    }
}

partial class TIMELINE
{
    public CODE code = new CODE();

    public partial class CODE
    {

        private TIMELINE timeline;
        public Func<int, int> reversion;
        public void init(TIMELINE timeline)
        {
            this.timeline = timeline;
            timeline.Log("Code Started");
        }

        public void Log(object msg)
        {
            timeline.Log(msg);

        }

        public class TLVector3
        {
            public string type { get; set; }
            public Vector3 Vector3;
            public object timeline;
            public float x { get { return Vector3.x; } set { Vector3.x = value; } }
            public float y { get { return Vector3.y; } set { Vector3.y = value; } }
            public float z { get { return Vector3.z; } set { Vector3.z = value; } }
            public TLVector3(TIMELINE timeline, float x, float y, float z, string type = "position") {
                this.type = type;
                this.Vector3 = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new {x = new Exec(this), y = new Exec(this), z = new Exec(this)};
            }
        }

        public class TLPoly
        {
            public string type { get; set; }
            public object timeline;
            public float[] poly;
            public TLPoly(TIMELINE timeline, float[] poly) {
                this.type = "poly";
                this.poly = poly;
                object[] polyExec = new object[poly.Length];
                for (int p = 0; p < poly.Length; p++) {
                    polyExec[p] = new Exec(this);
                }
                this.timeline = new {poly = polyExec};
            }
        }

        public object[] instructionSet(object[] options)
        {
            int optionsLen = options.Length;

            // prepare rows
            int[] rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (checkType(options, o, typeof(bool))) break;

                // o = 0 TIMELINE
                if (checkType(options, o, typeof(TIMELINE)))
                    rows[0]++;

                // o = 1 bind/element/tranform, o = 2 int
                if (checkListTypes(options, o, new System.Type[] { typeof(Transform), typeof(TLVector3), typeof(Vector3), typeof(Vector4) }) || checkFieldType(options, o, "poly", typeof(float[])))
                    rows[1]++;

                // o = 3 string, o = 4 int, o = 5 int
                if (checkTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" }))
                    rows[2]++;

                // o = 6 int x 4
                if (checkType(options, o, typeof(int)))
                    rows[3]++;
            }

            object[][] phrase = new object[rows.Length][];
            for (int r = 0; r < rows.Length; r++)
            {
                phrase[r] = new object[rows[r]];
            }

            // add data to rows
            rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (checkType(options, o, typeof(bool))) break;

                // o = 0 TIMELINE
                if (checkType(options, o, typeof(TIMELINE)))
                {
                    phrase[0][rows[0]] = options[o];
                    rows[0]++;
                }

                // o = 1 bind/element/tranform, o = 2 int
                if (checkListTypes(options, o, new System.Type[] { typeof(Transform), typeof(TLVector3), typeof(Vector3), typeof(Vector4) }) || 
                    checkFieldType(options, o, "poly", typeof(float[])))
                {
                    phrase[1][rows[1]] = new object[]
                    {
                            options[o],
                            checkType(options, o+1, typeof(int)) ? options[++o] : null,
                            null,
                            null
                    };
                    rows[1]++;
                }

                // o = 3 string, o = 4 int, o = 5 int
                if (checkTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" }))
                {
                    phrase[2][rows[2]] = new object[]
                    {
                            options[o],
                            checkType(options, o+1, typeof(float)) ? options[++o] : null,
                            checkType(options, o+1, typeof(float)) ? options[++o] : null,
                            null
                    };
                    rows[2]++;
                }

                // o = 6 int x 4
                if (checkType(options, o, typeof(int)))
                {
                    phrase[3] = new object[]
                    {
                            options[o],
                            checkType(options, o+1, typeof(int)) ? options[++o] : null,
                            checkType(options, o+1, typeof(int)) ? options[++o] : null,
                            checkType(options, o+1, typeof(int)) ? options[++o] : null
                    };
                }
            }

            return new object[]
            {
                    phrase[0],
                    phrase[1],
                    phrase[2],
                    phrase[3],
                    checkType(options, options.Length-2, typeof(bool)) ? options[options.Length-2] : false,
                    checkType(options, options.Length-1, typeof(float)) ? options[options.Length-1] : 1F
            };
        }

        public int count(object obj)
        {
            return ((object[])obj).Length;
        }

        public bool checkType(object[] options, int i, System.Type type)
        {
            if (i < options.Length && options[i].GetType() == type)
                return true;
            else
                return false;
        }

        public bool checkFieldType(object[] options, int i, string fieldType, System.Type type)
        {
            if (i < options.Length && options[i].GetType().GetProperty(fieldType) != null)
                return true;
            else
                return false;
        }


        public bool checkListTypes(object[] options, int i, System.Type[] list)
        {
            if (i < options.Length)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].GetType() == list[l]) return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool checkTypeList(object[] options, int i, System.Type type, object[] list)
        {
            if (i < options.Length && options[i].GetType() == type)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i] == list[l]) return true;
                }
                return false;
            }
            else
                return false;
        }

        public bool checkList(object option, object[] list)
        {
            for (int l = 0; l < list.Length; l++)
            {
                if (option == list[l]) return true;
            }
            return false;
        }

        public object checkListGet(string option, string[] list)
        {
            foreach (var pair in list)
            {
                string[] itemArr = pair.Split('=');
                if (option == itemArr[0])
                {
                    return itemArr[1];
                }
            }
            return null;
        }

    }
}