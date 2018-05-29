namespace TLMath
{
    public partial class Math
    {
        public class Precisions
        {
            public const float translation = 1000F;
            public const float radian = 10000F;
            public const float uniform = 1000F;
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
            public static float convertToPrecision(string type, float val)
            {
                return val * precision(type);
            }
            public static float convertFromPrecision(string type, float val)
            {
                return val / precision(type);
            }
            public static float convertToType(string type, float val)
            {
                if (type == "radian")
                {
                    return radians(val);
                }
                else
                {
                    return val;
                }
            }
            public static float convertFromType(string type, float val)
            {
                if (type == "radian")
                {
                    return degrees(val);
                }
                else
                {
                    return val;
                }
            }
            public static float convertToPrecisionType(string type, float val = 0, float precision = 0)
            {
                return Type.convertToType(type, precision != 0 ? val * precision : val * Type.precision(type));
            }
            public static float convertFromPrecisionType(string type, float val, float precision = 0)
            {
                return Type.convertFromType(type, precision != 0 ? val / precision : val / Type.precision(type));
            }
            // Returns the precision value for integer conversions
            // TO-DO cache

            public static float precision(string type)
            {
                return (float)precisions.GetType().GetField(type).GetValue(precisions);
            }
            public static float checkListGet(string option, string[] list)
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
            public static object[][] convertToPrecisionData(string type, object[] param, int start, int end, float precision = 0)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 0 ? (float)data[di][ci] * precision : convertToPrecision(type, (float)data[di][ci]); // convertToPrecisionType(type, data[di][ci], precision);
                    }
                }
                return data;
            }
            public static object[][] convertFromPrecisionData(string type, object[] param, int start, int end, float precision = 0)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 0 ? (float)data[di][ci] / precision : convertFromPrecision(type, (float)data[di][ci]); //convertFromPrecisionType(type, data[di][ci], precision);)
                    }
                }
                return data;
            }
            public static object[][] convertToTypeData(string type, object[] param, int start, int end)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = convertToType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] convertFromTypeData(string type, object[] param, int start, int end)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = convertFromType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] convertToPrecisionDataType(string type, object[] param, int start, int end, float precision = 0)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 0 ? (float)data[di][ci] * precision : convertToPrecision(type, (float)data[di][ci]); //convertToPrecisionType(type, (float)data[di][ci], precision);
                        data[di][ci] = convertToType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
            public static object[][] convertFromPrecisionDataType(string type, object[] param, int start, int end, float precision = 0)
            {
                object[][] data = new object[param.Length][];
                for (int di = 0; di < param.Length; di++)
                {
                    data[di] = (object[])param[di];
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] == null) continue;
                        data[di][ci] = precision != 0 ? (float)data[di][ci] / precision : convertFromPrecision(type, (float)data[di][ci]); //convertFromPrecisionType(type, (float)data[di][ci], precision);
                        data[di][ci] = convertFromType(type, (float)data[di][ci]);
                    }
                }
                return data;
            }
        }
    }
}