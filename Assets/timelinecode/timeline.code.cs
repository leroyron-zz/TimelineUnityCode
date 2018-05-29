using System;
using UnityEngine;
using TLMath;
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
        public static T Cast<T>(this object obj, T type)
        {
            return (T)obj;
        }
        public static T Member<T>(this object obj, T type)
        {
            return (T)obj;
        }
        public static int Count(this object param)
        {   
            int c = 0;
            object[] objs = (object[])param;
            for (int i = 0; i < objs.Length; i++) if (objs[i] != null) c++;
            return c;
        }
        public static int Count(this object[] param)
        {   
            int c = 0;
            for (int i = 0; i < param.Length; i++) if (param[i] != null) c++;
            return c;
        }
        public static object GetMember(this object param, string propName, object value = null)
            {
                if (param == null) throw new System.ArgumentException("Value cannot be null.", "param");
                if (propName == null) throw new System.ArgumentException("Value cannot be null.", "propName");
                
                if(propName.Contains("."))//complex type nested
                {
                    var temp = propName.Split(new char[] { '.' }, 2);
                    return GetMember(GetMember(param, temp[0], value), temp[1], value);
                }
                else
                {
                    var property = param.GetType().GetProperty(propName);
                    if (property != null) {
                        if (value != null && property.GetValue(param, null).GetType() == value.GetType()) property.SetValue(param, value, null);
                        return property.GetValue(param, null);
                    }

                    var field = param.GetType().GetField(propName);
                    if (field != null) {
                        if (value != null && field.IsStatic == false && field.GetValue(param).GetType() == value.GetType()) field.SetValue(param, value);
                        return field.IsStatic == false ? (object)field.GetValue(param) : null;
                    }
                    return null;
                }
            }

            public static bool MemberExists(this object param, string propName, object value = null)
            {
                if (param == null) throw new System.ArgumentException("Value cannot be null.", "param");
                if (propName == null) throw new System.ArgumentException("Value cannot be null.", "propName");
                
                if(propName.Contains("."))//complex type nested
                {
                    var temp = propName.Split(new char[] { '.' }, 2);
                    return MemberExists(MemberExists(param, temp[0], value), temp[1], value);
                }
                else
                {
                    var property = param.GetType().GetProperty(propName);
                    if (property != null) {
                        if (value != null && property.GetValue(param, null).GetType() == value.GetType()) property.SetValue(param, value, null);
                        return true;
                    }

                    var field = param.GetType().GetField(propName);
                    if (field != null) {
                        if (value != null && field.IsStatic == false && field.GetValue(param).GetType() == value.GetType()) field.SetValue(param, value);
                        return true;
                    }
                    return false;
                }
            }
        public static object CastToElement(this object param, object[] options)
        {
            object paramType = param.GetType().GetProperty("type").GetValue(param, null);

            if (paramType.GetType() != typeof(string))
                return null;

            if (paramType.GetType() == typeof(string) && (string)paramType == "position" || (string)paramType == "rotation")
            {
                getDimensions dimension = new getDimensions(options);
                if (dimension.count == 1)
                    return null;//new TIMELINE.CODE.TLVector1(dimension.x, (string)paramType);
                else if (dimension.count == 2)
                    return new TIMELINE.CODE.TLVector2(dimension.x, dimension.y, (string)paramType);
                else if (dimension.count == 3)
                    return new TIMELINE.CODE.TLVector3(dimension.x, dimension.y, dimension.z, (string)paramType);
                else if (dimension.count == 4)
                    return null;//new TIMELINE.CODE.TLVector4(dimension.x, dimension.y, dimension.z, dimension.w, (string)paramType);
            }

            checkGetFieldNamesValues type = new checkGetFieldNamesValues(param, new string[] { "type" });

            checkGetFieldNamesValues field = new checkGetFieldNamesValues(param, new string[] { "value", "radius", "rotation", "alpha", "scale" });

            return new TIMELINE.CODE.TLElement(type.names[0], field.names, field.floats);
        }
        private class checkGetFieldNamesValues
        {
            public string[] names;
            public float[] floats;
            public string[] strings;
            public checkGetFieldNamesValues(object options, string[] fieldTypes)
            {
                names = new string[fieldTypes.Length];
                floats = new float[fieldTypes.Length];
                strings = new string[fieldTypes.Length];
                for (int f = 0; f < fieldTypes.Length; f++)
                {
                    var property = options.GetType().GetProperty(fieldTypes[f]);
                    if (property != null)
                    {
                        names[f] = fieldTypes[f];
                        object val = property.GetValue(options, null);
                        if (val.GetType() == typeof(string))
                        {
                            strings[f] = (string)val;
                        }
                        else
                        {
                            floats[f] = Convert.ToSingle(val);
                        }
                        return;
                    }
                    var field = options.GetType().GetField(fieldTypes[f]);
                    if (field != null)
                    {
                        names[f] = fieldTypes[f];
                        object val = field.GetValue(options);
                        if (val.GetType() == typeof(string))
                        {
                            strings[f] = (string)val;
                        }
                        else
                        {
                            floats[f] = Convert.ToSingle(val);
                        }
                        return;
                    }
                }
            }
        }
        private class getDimensions
        {
            public float x, y, z, w, u, v;
            public int count = 0;
            public getDimensions(object[] options)
            {
                for (int o = 0; o < options.Length; o++)
                {
                    if (options[o].GetType() == typeof(string))
                    {
                        if ((string)options[o] == "x") { x = (float)options[o + 1]; count++; };
                        if ((string)options[o] == "y") { y = (float)options[o + 1]; count++; };
                        if ((string)options[o] == "z") { z = (float)options[o + 1]; count++; };
                        if ((string)options[o] == "w") { w = (float)options[o + 1]; count++; };
                        if ((string)options[o] == "u") { u = (float)options[o + 1]; count++; };
                        if ((string)options[o] == "v") { v = (float)options[o + 1]; count++; };

                    }
                }
            }
        }
    }
}

partial class TIMELINE
{
    public partial class CODE
    {
        public partial class Exec {
            public object parameter;

            public int binding;
            public int position;
            public bool relative;
            public string conversion = "uniform";
            public float precision;

            public static object execNode;
            public static string execBinding;
            public static int execByteOffset;
            public static int execData0PosI;
            public static int execLastOffset;
            public static float execLastVal;
            public static float value;
            public static float[] array;
            // controlled chaining methods
            private Func<float, string, int, object, bool, bool, float?, bool, Exec, Exec> _assign;
            private Func<float, string, int, object, bool, bool, float?, bool, Exec, Exec> _then;
            public Exec then (
                float value, 
                string ease, 
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._then(value, ease, duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<object, bool, bool, float?, bool, Exec, Exec> _keep;
            public Exec keep (
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._keep(leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, Exec, Exec> _wait;
            public Exec wait (
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._wait(duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, Exec, Exec> _hold;// data offset back and flood with value
            public Exec hold (
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._hold(duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<float[], object, bool, bool, float?, bool, Exec, Exec> _pair;// replace data with 
            public Exec pair (
                float[] array, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._pair(array, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, Exec, Exec> _shift;// data offset
            public Exec shift (
                int shift, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._shift(shift, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, int, object, bool, bool, float?, bool, Exec, Exec> _revert;// break// put back old data
            public Exec revert (
                int from,
                int to, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._revert(from, to, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, Exec, Exec> _at;// jump to/get from data
            public Exec at (
                int at, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                Exec This = null) {
                    This = This ?? this;
                    this._at(at, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<float, int, int, Exec, Exec> _flood;
            public Exec flood (
                float value,
                int from,
                int to,
                Exec This = null) {
                    This = This ?? this;
                    this._flood(value, from, to, This);
                return this;
            }
            public Exec (TLElement parameter) {
                this.parameter = parameter;
                init();
            }
            public Exec (TLVector2 parameter) {
                this.parameter = parameter;
                init();
            }
            public Exec (TLVector3 parameter) {
                this.parameter = parameter;
                init();
            }
            public Exec (TLPoly parameter) {
                this.parameter = parameter;
                init();
            }
            void init() {
                this._then = (float value, string ease, int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Then");
                    return This;
                };
                this._keep = (object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Keep");
                    return This;
                };
                this._wait = (int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Wait");
                    return This;
                };
                this._hold = (int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Hold");
                    return This;
                };
                this._pair = (float[] array, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Pair");
                    return This;
                };
                this._shift = (int shift, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Shift");
                    return This;
                };
                this._revert = (int from, int to, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec Revert");
                    return this;
                };
                this._at = (int at, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, Exec This) => {
                    TIMELINE.Log("Exec At");
                    return This;
                };
                this._flood = (float value, int from, int to, Exec This) => {
                    TIMELINE.Log("Exec Flood");
                    return This;
                };
            }
        }
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
            TIMELINE.Log("Code Started");
        }
        public void Log(object msg)
        {
            TIMELINE.Log(msg);
        }
        public class TLVector3 : TLType
        {
            public Vector3 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public float z { get { return transform.z; } set { transform.z = value; } }
            public struct exec
            {
                public Exec x, y, z;
                public exec (TLVector3 This)
                {
                    x = new Exec(This);
                    y = new Exec(This);
                    z = new Exec(This);
                }
            }
            public exec timeline;
            public TLVector3(string type = "position", float x = 0, float y = 0, float z = 0) {
                init(x, y, z, type);
            }
            public TLVector3(float x = 0, float y = 0, float z = 0, string type = "position") {
                init(x, y, z, type);
            }
            void init (float x, float y, float z, string type) {
                this.type = type;
                this.transform = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new exec(this);
            }
        }
        public class TLVector2 : TLType
        {
            public Vector2 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public struct exec
            {
                public Exec x, y;
                public exec (TLVector2 This)
                {
                    x = new Exec(This);
                    y = new Exec(This);
                }
            }
            public exec timeline;
            public TLVector2(string type = "position", float x = 0, float y = 0) {
                init(x, y, type);
            }
            public TLVector2(float x = 0, float y = 0, string type = "position") {
                init(x, y, type);
            }
            void init (float x, float y, string type) {
                this.type = type;
                this.transform = new Vector2(this.x = x, this.y = y);
                this.timeline = new exec(this);
            }
        }
        public class TLPoly : TLType
        {
            public Exec[] timeline;
            public TLPoly(float[] poly = null) {
                this.poly = init(poly);
            }
            private float[] init (float[] poly) {
                this.type = "poly";
                this.timeline = new Exec[poly != null ? poly.Length : 0];
                for (int p = 0; p < timeline.Length; p++) {
                    this.timeline[p] = new Exec(this);
                }
                return poly;
            }
        }
        const float _radianMax =  Mathf.PI * 2;
        public class TLVectors
        {
            
            public bool isRadian { get; set; }
            public float x, y, z, w, u, v;
            public TLVectors (object param) {
                this.x = Convert.ToSingle(param.GetMember("x"));
                this.y = Convert.ToSingle(param.GetMember("y"));
                this.z = Convert.ToSingle(param.GetMember("z"));
                this.w = Convert.ToSingle(param.GetMember("w"));
                this.u = Convert.ToSingle(param.GetMember("u"));
                this.v = Convert.ToSingle(param.GetMember("v"));
                this.isRadian = this.x < _radianMax
                &&
                this.y < _radianMax
                &&
                this.z < _radianMax
                &&
                this.w < _radianMax;
            }
        }
        public class TLType
        {
            public string type { get; set; }
            public float[] poly;
            public struct exec
            {
                public Exec x, y, z, w, u, v, value, radius, rotation, alpha, scale;
            }
            public exec timeline;
        }
        public class TLElement : TLType
        {
            public float? value, radius, rotation, alpha, scale;
            public struct exec
            {
                public Exec value, radius, rotation, alpha, scale;
            }
            public exec timeline;
            public TLElement(string type = null, string[] names = null, float[] values = null)
            {
                this.type = "uniform";
                string name;
                float value;
                for (int f = 0; f < names.Length; f++) {
                    name = (string)names[f];
                    value = (float)values[f];
                    switch (name)
                    {
                        case "value":
                            this.value = value;
                            this.timeline.value = new Exec(this);
                            break;
                        case "radius":
                            this.radius = value;
                            this.timeline.radius = new Exec(this);
                            break;
                        case "rotation":
                            this.rotation = value;
                            this.timeline.rotation = new Exec(this);
                            break;
                        case "alpha":
                            this.alpha = value;
                            this.timeline.alpha = new Exec(this);
                            break;
                        case "scale":
                            this.scale = value;
                            this.timeline.scale = new Exec(this);
                            break;
                        default :
                            break;
                    }
                }
            }
        }
        public bool isVector (object value) {
            return value.MemberExists("x") && value.MemberExists("y") || value.MemberExists("u") && value.MemberExists("v");
        }
        public bool smartCheckIndexType(object[] options, int i) {
            if (isVector(options[i]) && i < options.Length) {
                TLVectors tempVect = new TLVectors(options[i]);
                if (checkIndexFieldTypeByString(options, i, new string[]{"x", "y", "z"}, true)) 
                {
                    options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (checkIndexFieldTypeByString(options, i, new string[]{"x", "y"}, true)) 
                {
                    options[i] = new TLVector2(tempVect.x, tempVect.y, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (checkIndexFieldTypeByString(options, i, new string[]{"x", "y", "z", "w"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.w, tempVect.isRadian ? "translate" : "radian");
                    return true;
                }
                else if (checkIndexFieldTypeByString(options, i, new string[]{"u", "v"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.u, tempVect.v);
                    return true;
                };
            }
            return false;
        }
        public object[] instructionSet(object[] options)
        {
            int optionsLen = options.Length;

            // prepare rows
            int[] rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (checkIndexType(options, o, typeof(bool))) break;

                // o = 0 TIMELINE
                if (checkIndexType(options, o, typeof(TIMELINE)))
                    rows[0]++;

                // o = 1 bind/element/transform, o = 2 int
                if (checkIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || checkIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"})
                 || smartCheckIndexType(options, o))
                    rows[1]++;

                // o = 3 string, o = 4 int, o = 5 int
                if (checkIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || checkIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'w', 'u', 'v' }))
                    rows[2]++;

                // o = 6 int x 4
                if (checkIndexType(options, o, typeof(int)))
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
                if (checkIndexType(options, o, typeof(bool))) break;

                // o = 0 TIMELINE
                if (checkIndexType(options, o, typeof(TIMELINE)))
                {
                    phrase[0][rows[0]] = options[o];
                    rows[0]++;
                }

                // o = 1 bind/element/tranform, o = 2 int
                if (checkIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || checkIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"}))
                {
                    options[o] = options[o] is TLType ? options[o] as TLType : options[o].CastToElement(options);

                    phrase[1][rows[1]] = new object[]
                    {
                            options[o],
                            checkIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            null,
                            null
                    };
                    rows[1]++;
                }

                // o = 3 string, o = 4 int, o = 5 int
                if (checkIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || checkIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'w', 'u', 'v' }))
                {
                    phrase[2][rows[2]] = new object[]
                    {
                            options[o].ToString(),
                            checkIndexType(options, o+1, typeof(float)) || checkIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            checkIndexType(options, o+1, typeof(float)) || checkIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            null
                    };
                    rows[2]++;
                }
                

                // o = 6 int x 4
                if (checkIndexType(options, o, typeof(int)))
                {
                    phrase[3] = new object[]
                    {
                            options[o],
                            checkIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            checkIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            checkIndexType(options, o+1, typeof(int)) ? options[++o] : null
                    };
                }
            }

            return new object[]
            {
                    phrase[0],
                    phrase[1],
                    phrase[2],
                    phrase[3],
                    checkIndexType(options, options.Length-2, typeof(bool)) ? options[options.Length-2] : false,
                    checkIndexType(options, options.Length-1, typeof(float)) ? options[options.Length-1] : 0F
            };
        }
        public bool checkIndexType(object[] options, int i, System.Type type)
        {
            if (i < options.Length && options[i].GetType() == type)
                return true;
            else
                return false;
        }
        public T checkTypeCast<T>(object obj, T type)
        {
            return (T)obj;
        }
        public bool checkIndexFieldTypeByString(object[] options, int i, string[] fieldTypes, bool exact = false)
        {
            if (i < options.Length) {
                int e = 0;
                for (int f = 0; f < fieldTypes.Length; f++) {
                    if (options[i].GetType().GetProperty(fieldTypes[f]) != null || options[i].GetType().GetField(fieldTypes[f]) != null) {
                        e++;
                        if (exact == false) return true;
                    }
                }
                if (e == fieldTypes.Length) return true;
            }
            return false;
        }
        public bool checkIndexListTypes(object[] options, int i, System.Type[] list)
        {
            if (i < options.Length)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].GetType() == list[l]) 
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public System.Type checkIndexListTypesGet(object[] options, int i, System.Type Base,  System.Type[] list)
        {
            if (i < options.Length)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].GetType() == list[l]) 
                    return list[l];
                }
                return Base;
            }
            else
            {
                return Base;
            }
        }
        public bool checkIndexTypeList(object[] options, int i, System.Type type, object[] list)
        {
            if (i < options.Length && options[i].GetType() == type)
            {
                for (int l = 0; l < list.Length; l++)
                {
                    if (options[i].Equals(list[l])) return true; // compare Chars *.Equals
                    // if (options[i] == list[l]) return true;
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
                if (option.Equals(list[l])) return true; // compare Chars *.Equals
                // if (option == list[l]) return true;
            }
            return false;
        }
        public object checkListGet(string option, string[] list)
        {
            foreach (var pair in list)
            {
                string[] itemArr = pair.Split('=');
                if (option == itemArr[0]) return itemArr[1];
            }
            return "uniform";
        }
    }
}