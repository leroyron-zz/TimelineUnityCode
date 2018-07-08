using System;

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
        public static T[] Concat<T>(this T[] x, T y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            //int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + 1);
            x[x.Length - 1] = y;
            return x;
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

        public static T[][] Concat<T>(this T[][] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            //int oldLen = x.Length;
            Array.Resize<T[]>(ref x, x.Length + 1);
            Array.Resize<T>(ref x[x.Length - 1], y.Length);
            Array.Copy(y, 0, x[x.Length - 1], 0, y.Length);
            return x;
        }

        public static T[][] Concat<T>(this T[][] x, T[][] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            //int oldLen = x.Length;
            for (int i = 0; i < y.Length; i++) {
                Array.Resize<T[]>(ref x, x.Length + 1);
                Array.Resize<T>(ref x[x.Length - 1], y[i].Length);
                Array.Copy(y[i], 0, x[x.Length - 1], 0, y[i].Length);
            }
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
