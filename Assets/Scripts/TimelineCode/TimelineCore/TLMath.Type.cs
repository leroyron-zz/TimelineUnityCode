namespace TLMath
{
    public partial class TMath
    {
        public class Precisions
        {
            public const float translation = 1000f;
            public const float radian = 10000f;
            public const float uniform = 1000f;
        }
        
        public static class Type
        {
            public static Precisions precisions = new Precisions();
            // Return precision or type conversion value for a specific type ///
            public static T Cast<T>(T typeHolder, object x)
            {
                // typeHolder above is just for compiler magic
                // to infer the type to cast x to
                return (T)x;
            }
            public static float ConvertToPrecision(string type, float val)
            {
                return val * Precision(type);
            }
            public static float ConvertFromPrecision(string type, float val)
            {
                return val / Precision(type);
            }
            public static float ConvertToType(string type, float val)
            {
                if (type == "radian")
                {
                    return Radians(val);
                }
                else
                {
                    return val;
                }
            }
            public static float ConvertFromType(string type, float val)
            {
                if (type == "radian")
                {
                    return Degrees(val);
                }
                else
                {
                    return val;
                }
            }
            public static float ConvertToPrecisionType(string type, float val = 0, float precision = 1)
            {
                return Type.ConvertToType(type, val);
                //return Type.ConvertToType(type, precision != 1 ? val * precision : val * Type.Precision(type));
            }
            public static float ConvertFromPrecisionType(string type, float val, float precision = 1)
            {
                return Type.ConvertFromType(type, val);
                //return Type.ConvertFromType(type, precision != 1 ? val / precision : val / Type.Precision(type));
            }
            // Returns the precision value for integer conversions
            // TO-DO cache

            public static float Precision(string type)
            {
                return (float)precisions.GetType().GetField(type).GetValue(precisions);
            }
            public static float CheckListGet(string option, string[] list)
            {
                foreach (var pair in list)
                {
                    string[] itemArr = pair.Split('=');
                    if (option == itemArr[0])
                    {
                        return float.Parse(itemArr[1]);
                    }
                }
                return 1;
            }
            //Data / Poly
            public static object[][] ConvertToPrecisionData(string type, object[] param, int start, int end, float precision = 1)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 1 ? (float)data[di][ci] * precision : ConvertToPrecision(type, (float)data[di][ci]); // ConvertToPrecisionType(type, data[di][ci], precision);
                    }
                }
                return data;
            }
            public static object[][] ConvertFromPrecisionData(string type, object[] param, int start, int end, float precision = 1)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 1 ? (float)data[di][ci] / precision : ConvertFromPrecision(type, (float)data[di][ci]); //ConvertFromPrecisionType(type, data[di][ci], precision);)
                    }
                }
                return data;
            }
            public static object[][] ConvertToTypeData(string type, object[] param, int start, int end)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = ConvertToType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] ConvertFromTypeData(string type, object[] param, int start, int end)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = ConvertFromType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] ConvertToPrecisionDataType(string type, object[] param, int start, int end, float precision = 1)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 1 ? (float)data[di][ci] * precision : ConvertToPrecision(type, (float)data[di][ci]); //ConvertToPrecisionType(type, (float)data[di][ci], precision);
                        data[di][ci] = ConvertToType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] ConvertFromPrecisionDataType(string type, object[] param, int start, int end, float precision = 1)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 1 ? (float)data[di][ci] / precision : ConvertFromPrecision(type, (float)data[di][ci]); //ConvertFromPrecisionType(type, (float)data[di][ci], precision);
                        data[di][ci] = ConvertFromType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
        }
    }
}