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
    public static class TExtensions
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static string FloatArrayToString(this float[] arr) {
            string str = "";
            for (int i = 0; i < arr.Length; i++) str += i < arr.Length-1 ? arr[i].ToString()+", " : arr[i].ToString();
            return str;
        }
        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }

        public static T[] ConcatFrom<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, y.Length);
            x[y.Length-1] = y[y.Length-1] == null ? x[oldLen-1] : y[y.Length-1];
            return x;
        }

        public static T[] Resize<T>(this T[] x, int idx)
        {
            if (x == null) throw new ArgumentNullException("x");
            Array.Resize<T>(ref x, idx);
            return x;
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
        public static object GetMember(this object param, string propName, object value = null, bool reAssign = true)
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
                        object obj = (object)property.GetValue(param, null);
                        if (obj == null && value != null) {
                            property.SetValue(param, value, null);
                            return property.GetValue(param, null);
                        }
                        System.Type type = property.GetValue(param, null).GetType();
                        if (reAssign && value != null && type == value.GetType()) {
                            property.SetValue(param, value, null);
                        }
                        return property.GetValue(param, null);
                    }

                    var field = param.GetType().GetField(propName);
                    if (field != null) {
                        object obj = (object)field.GetValue(param);
                        if (obj == null && value != null) { 
                            field.SetValue(param, value);
                            return field.IsStatic == false ? (object)field.GetValue(param) : null;
                        }
                        System.Type type = field.GetValue(param).GetType();
                        if (reAssign && value != null && field.IsStatic == false && type == value.GetType()) {
                            field.SetValue(param, value);
                        }
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
                        object obj = (object)property.GetValue(param, null);
                        if (obj == null) {
                            property.SetValue(param, value, null);
                            return true;
                        }
                        System.Type type = property.GetValue(param, null).GetType();
                        if (value != null && type == value.GetType()) {
                            property.SetValue(param, value, null);
                        }
                        return true;
                    }

                    var field = param.GetType().GetField(propName);
                    if (field != null) {
                        object obj = (object)field.GetValue(param);
                        if (obj == null) {
                            field.SetValue(param, value);
                            return true;
                        }
                        System.Type type = field.GetValue(param).GetType();
                        if (value != null && field.IsStatic == false && type == value.GetType()) {
                            field.SetValue(param, value);
                        }
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
                GetDimensions dimension = new GetDimensions(options);
                if (dimension.count == 1)
                    return null;//new Timeline.Core.TLVector1(dimension.x, (string)paramType);
                else if (dimension.count == 2)
                    return new Timeline.Core.TLVector2(dimension.x, dimension.y, (string)paramType);
                else if (dimension.count == 3)
                    return new Timeline.Core.TLVector3(dimension.x, dimension.y, dimension.z, (string)paramType);
                else if (dimension.count == 4)
                    return null;//new Timeline.Core.TLVector4(dimension.x, dimension.y, dimension.z, dimension.w, (string)paramType);
            }

            CheckGetFieldNamesValues type = new CheckGetFieldNamesValues(param, new string[] { "type" });

            CheckGetFieldNamesValues field = new CheckGetFieldNamesValues(param, new string[] { "value", "radius", "rotation", "alpha", "scale" });

            return new Timeline.Core.TLElement(type.names[0], field.names, field.floats);
        }
        private class CheckGetFieldNamesValues
        {
            public string[] names;
            public float[] floats;
            public string[] strings;
            public CheckGetFieldNamesValues(object options, string[] fieldTypes)
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
        private class GetDimensions
        {
            public float x, y, z, w, u, v;
            public int count = 0;
            public GetDimensions(object[] options)
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

public partial class Timeline
{
    public partial class Core
    {
        public partial class ExecParams {
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
            private Func<float, string, int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Assign;
            private Func<float, string, int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Then;
            public ExecParams Then(
                float value, 
                string ease, 
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Then(value, ease, duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<object, bool, bool, float?, bool, ExecParams, ExecParams> _Keep;
            public ExecParams Keep(
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Keep(leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Wait;
            public ExecParams Wait(
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Wait(duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Hold;// data offset back and flood with value
            public ExecParams Hold(
                int duration, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Hold(duration, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<float[], object, bool, bool, float?, bool, ExecParams, ExecParams> _Pair;// replace data with 
            public ExecParams Pair(
                float[] array, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Pair(array, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Shift;// data offset
            public ExecParams Shift(
                int shift, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Shift(shift, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, int, object, bool, bool, float?, bool, ExecParams, ExecParams> _Revert;// break// put back old data
            public ExecParams Revert(
                int from,
                int to, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._Revert(from, to, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<int, object, bool, bool, float?, bool, ExecParams, ExecParams> _At;// jump to/get from data
            public ExecParams At(
                int at, 
                object leapCallback = null, 
                bool reassign = false, 
                bool dispose = true, 
                float? zeroIn = null, 
                bool skipLeap = false, 
                ExecParams This = null) {
                    This = This ?? this;
                    this._At(at, leapCallback, reassign, dispose, zeroIn, skipLeap, This);
                return this;
            }
            private Func<float, int, int, ExecParams, ExecParams> _Flood;
            public ExecParams Flood(
                float value,
                int from,
                int to,
                ExecParams This = null) {
                    This = This ?? this;
                    this._Flood(value, from, to, This);
                return this;
            }
            public ExecParams(TLElement parameter) {
                this.parameter = parameter;
                Init();
            }
            public ExecParams(TLVector2 parameter) {
                this.parameter = parameter;
                Init();
            }
            public ExecParams(TLVector3 parameter) {
                this.parameter = parameter;
                Init();
            }
            public ExecParams(TLPoly parameter) {
                this.parameter = parameter;
                Init();
            }
            void Init() {
                this._Then = (float value, string ease, int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Then");
                    return This;
                };
                this._Keep = (object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Keep");
                    return This;
                };
                this._Wait = (int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Wait");
                    return This;
                };
                this._Hold = (int duration, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Hold");
                    return This;
                };
                this._Pair = (float[] array, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Pair");
                    return This;
                };
                this._Shift = (int shift, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Shift");
                    return This;
                };
                this._Revert = (int from, int to, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams Revert");
                    return this;
                };
                this._At = (int at, object leapCallback, bool reassign, bool dispose, float? zeroIn, bool skipLeap, ExecParams This) => {
                    TimelineCode.Log("ExecParams At");
                    return This;
                };
                this._Flood = (float value, int from, int to, ExecParams This) => {
                    TimelineCode.Log("ExecParams Flood");
                    return This;
                };
            }
        }
    }
}

public partial class Timeline
{
    public Core code = new Core();

    public partial class Core
    {
        Timeline _timeline;
        public Func<int, int> reversion;
        public void Init(Timeline timeline)
        {
            this._timeline = timeline;
            TimelineCode.Log("Code Started");
        }
        public class TLVector3 : TLType
        {
            public Vector3 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public float z { get { return transform.z; } set { transform.z = value; } }
            public struct Exec
            {
                public ExecParams x, y, z;
                public Exec(TLVector3 This)
                {
                    x = new ExecParams(This);
                    y = new ExecParams(This);
                    z = new ExecParams(This);
                }
            }
            public Exec timeline;
            public TLVector3(string type = "position", float x = 0, float y = 0, float z = 0) {
                Init(x, y, z, type);
            }
            public TLVector3(float x = 0, float y = 0, float z = 0, string type = "position") {
                Init(x, y, z, type);
            }
            void Init(float x, float y, float z, string type) {
                this.type = type;
                this.transform = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new Exec(this);
            }
        }
        public class TLVector2 : TLType
        {
            public Vector2 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public struct Exec
            {
                public ExecParams x, y;
                public Exec(TLVector2 This)
                {
                    x = new ExecParams(This);
                    y = new ExecParams(This);
                }
            }
            public Exec timeline;
            public TLVector2(string type = "position", float x = 0, float y = 0) {
                Init(x, y, type);
            }
            public TLVector2(float x = 0, float y = 0, string type = "position") {
                Init(x, y, type);
            }
            void Init(float x, float y, string type) {
                this.type = type;
                this.transform = new Vector2(this.x = x, this.y = y);
                this.timeline = new Exec(this);
            }
        }
        public class TLPoly : TLType
        {
            public ExecParams[] timeline;
            public TLPoly(float[] poly = null) {
                this.poly = Init(poly);
            }
            private float[] Init(float[] poly) {
                this.type = "poly";
                this.timeline = new ExecParams[poly != null ? poly.Length : 0];
                for (int p = 0; p < timeline.Length; p++) {
                    this.timeline[p] = new ExecParams(this);
                }
                return poly;
            }
        }
        const float _radianMax =  Mathf.PI * 2;
        public class TLVectors
        {
            public bool isRadian { get; set; }
            public float x, y, z, w, u, v;
            public TLVectors(object param) {
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
            public struct Exec
            {
                public ExecParams x, y, z, w, u, v, value, radius, rotation, alpha, scale;
            }
            public Exec timeline;
        }
        public class TLElement : TLType
        {
            public float? value, radius, rotation, alpha, scale;
            public struct Exec
            {
                public ExecParams value, radius, rotation, alpha, scale;
            }
            public Exec timeline;
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
                            this.timeline.value = new ExecParams(this);
                            break;
                        case "radius":
                            this.radius = value;
                            this.timeline.radius = new ExecParams(this);
                            break;
                        case "rotation":
                            this.rotation = value;
                            this.timeline.rotation = new ExecParams(this);
                            break;
                        case "alpha":
                            this.alpha = value;
                            this.timeline.alpha = new ExecParams(this);
                            break;
                        case "scale":
                            this.scale = value;
                            this.timeline.scale = new ExecParams(this);
                            break;
                        default :
                            break;
                    }
                }
            }
        }
        public bool IsVector(object value) {
            return value.MemberExists("x") && value.MemberExists("y") || value.MemberExists("u") && value.MemberExists("v");
        }
        public bool SmartCheckIndexType(object[] options, int i) {
            if (IsVector(options[i]) && i < options.Length) {
                TLVectors tempVect = new TLVectors(options[i]);
                if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y", "z"}, true)) 
                {
                    options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y"}, true)) 
                {
                    options[i] = new TLVector2(tempVect.x, tempVect.y, tempVect.isRadian ? "radian" : "translate");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"x", "y", "z", "w"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.x, tempVect.y, tempVect.z, tempVect.w, tempVect.isRadian ? "translate" : "radian");
                    return true;
                }
                else if (CheckIndexFieldTypeByString(options, i, new string[]{"u", "v"}, true)) 
                {
                    //options[i] = new TLVector3(tempVect.u, tempVect.v);
                    return true;
                };
            }
            return false;
        }
        public object[] InstructionSet(object[] options)
        {
            int optionsLen = options.Length;

            // prepare rows
            int[] rows = new int[6];
            for (int o = 0; o < options.Length; o++)
            {
                if (options[o] == null) break;

                if (CheckIndexType(options, o, typeof(bool))) break;

                // o = 0 Timeline
                if (CheckIndexType(options, o, typeof(Timeline)))
                    rows[0]++;

                if (CheckIndexType(options, o, typeof(Timeline[])))
                    rows[0] += ((Timeline[])options[o]).Length;

                // o = 1 bind/element/transform, o = 2 int
                if (CheckIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || CheckIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"})
                 || SmartCheckIndexType(options, o))
                    rows[1]++;

                // o = 3 string, o = 4 int, o = 5 int
                if (CheckIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || CheckIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'w', 'u', 'v' }))
                    rows[2]++;

                // o = 6 int x 4
                if (CheckIndexType(options, o, typeof(int)))
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
                if (options[o] == null) break;

                if (CheckIndexType(options, o, typeof(bool))) break;

                // o = 0 Timeline
                if (CheckIndexType(options, o, typeof(Timeline)))
                {
                    phrase[0][rows[0]] = options[o];
                    rows[0]++;
                }

                if (CheckIndexType(options, o, typeof(Timeline[]))) {
                    for (int t = 0; t < ((object[])options[o]).Length; t++) {
                        phrase[0][rows[0]] = ((object[])options[o])[t];
                        rows[0]++;
                    }
                }

                // o = 1 bind/element/tranform, o = 2 int
                if (CheckIndexListTypes(options, o, new System.Type[] { typeof(TLVector2), typeof(TLVector3), typeof(TLPoly), typeof(TLElement) })
                 || CheckIndexFieldTypeByString(options, o, new string[]{"type", "value", "radius", "position", "rotation", "alpha", "scale"}))
                {
                    options[o] = options[o] is TLType ? options[o] as TLType : options[o].CastToElement(options);

                    phrase[1][rows[1]] = new object[]
                    {
                            options[o],
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            null,
                            null
                    };
                    rows[1]++;
                }

                // o = 3 string, o = 4 int, o = 5 int
                if (CheckIndexTypeList(options, o, typeof(string), new object[] { "x", "y", "z", "w", "u", "v", "value", "radius", "rotation", "alpha", "scale", "poly" })
                || CheckIndexTypeList(options, o, typeof(char), new object[] { 'x', 'y', 'z', 'w', 'u', 'v' }))
                {
                    phrase[2][rows[2]] = new object[]
                    {
                            options[o].ToString(),
                            CheckIndexType(options, o+1, typeof(float)) || CheckIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(float)) || CheckIndexType(options, o+1, typeof(float[])) ? options[++o] : null,
                            null
                    };
                    rows[2]++;
                }
                
                // o = 6 int x 4
                if (CheckIndexType(options, o, typeof(int)))
                {
                    phrase[3] = new object[]
                    {
                            options[o],
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null,
                            CheckIndexType(options, o+1, typeof(int)) ? options[++o] : null
                    };
                }
            }

            return new object[]
            {
                    phrase[0],
                    phrase[1],
                    phrase[2],
                    phrase[3],
                    CheckIndexType(options, options.Length-2, typeof(bool)) ? options[options.Length-2] : false,
                    CheckIndexType(options, options.Length-1, typeof(float)) ? options[options.Length-1] : 1F
            };
        }
        public bool CheckIndexType(object[] options, int i, System.Type type)
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
        public bool CheckIndexFieldTypeByString(object[] options, int i, string[] fieldTypes, bool exact = false)
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
        public bool CheckIndexListTypes(object[] options, int i, System.Type[] list)
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
        public System.Type CheckIndexListTypesGet(object[] options, int i, System.Type Base,  System.Type[] list)
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
        public bool CheckIndexTypeList(object[] options, int i, System.Type type, object[] list)
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
        public bool CheckList(object option, object[] list)
        {
            for (int l = 0; l < list.Length; l++)
            {
                if (option.Equals(list[l])) return true; // compare Chars *.Equals
                // if (option == list[l]) return true;
            }
            return false;
        }
        public object CheckListGet(string option, string[] list)
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