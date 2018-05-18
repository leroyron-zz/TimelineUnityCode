namespace math
{
    public partial class Math
    {
        public static class Type
        {
            // Return precision or type conversion value for a specific type ///
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

            public static float convertToPrecisionType(string type, float val, float precision = 1)
            {
                return Type.convertToType(type, precision != 1 ? val * precision : val * Type.precision(type));
            }

            public static float convertFromPrecisionType(string type, float val, float precision = 1)
            {
                return Type.convertFromType(type, precision != 1 ? val / precision : val / Type.precision(type));
            }

            // Returns the precision value for integer conversions
            // TO-DO cache
            public static float precision(string type)
            {
                return (float)checkListGet(type, new
                                        string[]{
                                            "translation=1000",
                                            "radian=10000",
                                            "uniform=1000"
                                        }); ;
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
            public static float[][] convertToPrecisionData(string type, float[][] data, int start, int end, float precision)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertToPrecisionType(type, data[di][ci], precision); //precision ? data[di][ci] * precision : convertToPrecision(type, data[di][ci])
                        }
                    }
                }
                return data;
            }

            public static float[][] convertFromPrecisionData(string type, float[][] data, int start, int end, float precision)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertFromPrecisionType(type, data[di][ci], precision); //precision ? data[di][ci] / precision : convertFromPrecision(type, data[di][ci])
                        }
                    }
                }
                return data;
            }

            public static float[][] convertToTypeData(string type, float[][] data, int start, int end)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertToType(type, data[di][ci]);
                        }
                    }
                }
                return data;
            }

            public static float[][] convertFromTypeData(string type, float[][] data, int start, int end)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertFromType(type, data[di][ci]);
                        }
                    }
                }
                return data;
            }

            public static float[][] convertToPrecisionDataType(string type, float[][] data, int start, int end, float precision)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertToPrecisionType(type, data[di][ci], precision);//precision ? data[di][ci] * precision : convertToPrecision(type, data[di][ci])
                            data[di][ci] = convertToType(type, data[di][ci]);
                        }
                    }
                }
                return data;
            }

            public static float[][] convertFromPrecisionDataType(string type, float[][] data, int start, int end, float precision)
            {
                for (int di = 0, dlen = data.Length; di < dlen; di++)
                {
                    for (int ci = start; ci < start + end; ci++)
                    {
                        if (data[di][ci] != null)
                        {
                            data[di][ci] = convertFromPrecisionType(type, data[di][ci], precision);//precision ? data[di][ci] / precision : convertFromPrecision(type, data[di][ci])
                            data[di][ci] = convertFromType(type, data[di][ci]);
                        }
                    }
                }
                return data;
            }
        }
    }
}