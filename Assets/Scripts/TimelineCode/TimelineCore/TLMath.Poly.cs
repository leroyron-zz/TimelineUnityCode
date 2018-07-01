using UnityEngine;

namespace TLMath
{
    public partial class TMath
    {
        public static class Poly
        {
            public static object[] Polys(string type, float[] data, float precision = 1)
            {
                object[][] poly = new object[data.Length][];
                for (int di = 0; di < data.Length; di++)
                {
                    poly[di] = new object[]
                    {
                        di,
                        Type.ConvertToPrecisionType(type, data[di], precision)
                    };
                }
                return poly;
            }
            public static object[] Generate(string type, float[] data1, float[] data2, float precision = 1)
            {
                if (data1 == null && data2 != null) data1 = data2; else if (data1 == null && data2 == null) return new object[]{"No Data"};
                object[][] poly = new object[data1 != null ? data1.Length : 0][];
                for (int di = 0; di < poly.Length; di++)
                {
                    poly[di] = new object[]
                    {
                        di,
                        data2 != null ? Type.ConvertToPrecisionType(type, data2[di], precision) : data1 != null ? Type.ConvertToPrecisionType(type, data1[di], precision) : 0
                    };
                }
                return poly;
            }
            public static object[] GenerateKeys(object[] data, int start = 0)
            {
                object[] poly = new object[data.Length];
                for (int di = 0; di < data.Length; di++)
                {
                    poly[di] = start + di;
                }
                return poly;
            }
             public static object[] GenerateExec(string type, float[] data, float precision = 1)
            {
                object[][] poly = new object[data.Length][];
                for (int di = 0; di < data.Length; di++)
                {
                    poly[di] = new object[]
                    {
                        di,
                        Type.ConvertToPrecisionType(type, data[di], precision)
                    };
                }
                return poly;
            }
            public static float[] Add(float[] data1, float[] data2)
            {
                float[] poly = new float[data1.Length];
                for (int pi = 0; pi < data1.Length; pi++)
                {
                    poly[pi] = data1[pi] + data2[pi];
                }
                return poly;
            }
            public static float[] Subtract(float[] data1, float[] data2)
            {
                float[] poly = new float[data1.Length];
                for (int pi = 0; pi < data1.Length; pi++)
                {
                    poly[pi] = data1[pi] - data2[pi];
                }
                return poly;
            }
            public static float[] Multiply(float[] data1, float[] data2)
            {
                float[] poly = new float[data1.Length];
                for (int pi = 0; pi < data1.Length; pi++)
                {
                    poly[pi] = data1[pi] * data2[pi];
                }
                return poly;
            }
            public static float[] Divide(float[] data1, float[] data2)
            {
                float[] poly = new float[data1.Length];
                for (int pi = 0; pi < data1.Length; pi++)
                {
                    poly[pi] = data1[pi] / data2[pi];
                }
                return poly;
            }
            public static float[] AddScalar(float[] data, float val)
            {
                return Scalar("+", data, val, false);
            }
            public static float[] AddScalarReverse(float[] data, float val)
            {
                return Scalar("+", data, val, true);
            }
            public static float[] SubtractScalar(float[] data, float val)
            {
                return Scalar("-", data, val, false);
            }
            public static float[] SubtractScalarReverse(float[] data, float val)
            {
                return Scalar("-", data, val, true);
            }
            public static float[] MultiplyScalar(float[] data, float val)
            {
                return Scalar("*", data, val, false);
            }
            public static float[] MultiplyScalarReverse(float[] data, float val)
            {
                return Scalar("*", data, val, true);
            }
            public static float[] DivideScalar(float[] data, float val)
            {
                return Scalar("/", data, val, false);
            }
            public static float[] DivideScalarReverse(float[] data, float val)
            {
                return Scalar("/", data, val, true);
            }

            public static float[] Scalar(string operate, float[] data, float val, bool reverse)
            {
                float[] poly = new float[data.Length];
                if (operate == "+" && reverse) 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = val - (data[pi] + val);
                else if (operate == "+") 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = data[pi] + val;
                else if (operate == "-" && reverse) 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = val - (data[pi] - val);
                else if (operate == "-") 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = data[pi] - val;
                else if (operate == "*" && reverse) 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = val - (data[pi] * val);
                else if (operate == "*") 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = data[pi] * val;
                else if (operate == "/" && reverse) 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = val - (data[pi] / val);
                else if (operate == "/") 
                    for (int pi = 0; pi < data.Length; pi++) poly[pi] = data[pi] / val;
                return poly;
            }
            public static void addScalarVector(float[] data, Vector2 v)
            {
                ScalarVector("+=", data, new float?[2] { v.x, v.y });
            }
            public static void addScalarVector(float[] data, Vector3 v)
            {
                ScalarVector("+=", data, new float?[3] { v.x, v.y, v.z });
            }
            public static void addScalarVector(float[] data, Vector4 v)
            {
                ScalarVector("+=", data, new float?[4] { v.x, v.y, v.z, v.w });
            }
            public static void SubtractScalarVector(float[] data, Vector2 v)
            {
                ScalarVector("-=", data, new float?[2] { v.x, v.y });
            }
            public static void SubtractScalarVector(float[] data, Vector3 v)
            {
                ScalarVector("-=", data, new float?[3] { v.x, v.y, v.z });
            }
            public static void SubtractScalarVector(float[] data, Vector4 v)
            {
                ScalarVector("-=", data, new float?[4] { v.x, v.y, v.z, v.w });
            }
            public static void MultiplyScalarVector(float[] data, Vector2 v)
            {
                ScalarVector("*=", data, new float?[2] { v.x, v.y });
            }
            public static void MultiplyScalarVector(float[] data, Vector3 v)
            {
                ScalarVector("*=", data, new float?[3] { v.x, v.y, v.z });
            }
            public static void MultiplyScalarVector(float[] data, Vector4 v)
            {
                ScalarVector("*=", data, new float?[4] { v.x, v.y, v.z, v.w });
            }
            public static void DivideScalarVector(float[] data, Vector2 v)
            {
                ScalarVector("/=", data, new float?[2] { v.x, v.y });
            }
            public static void DivideScalarVector(float[] data, Vector3 v)
            {
                ScalarVector("/=", data, new float?[3] { v.x, v.y, v.z });
            }
            public static void DivideScalarVector(float[] data, Vector4 v)
            {
                ScalarVector("/=", data, new float?[4] { v.x, v.y, v.z, v.w });
            }
            public static void ScalarVector(string operate, float[] data, float?[] poly)
            {
                // Add
                if (operate == "+=")
                    for (
                        int pi = 0, qual = 0;
                        pi < data.Length;
                        pi++, qual = pi - (poly.Length * (pi / poly.Length << 0))
                    )
                        data[pi] += poly[qual] ?? 0f;

                // Subtract
                else if (operate == "-=")
                    for (
                        int pi = 0, qual = 0;
                        pi < data.Length;
                        pi++, qual = pi - (poly.Length * (pi / poly.Length << 0))
                    )
                        data[pi] -= poly[qual] ?? 0f;

                // Multiply
                else if (operate == "*=")
                    for (
                        int pi = 0, qual = 0;
                        pi < data.Length;
                        pi++, qual = pi - (poly.Length * (pi / poly.Length << 0))
                    )
                        data[pi] *= poly[qual] ?? 1f;

                //Divide
                else if (operate == "/=")
                    for (
                        int pi = 0, qual = 0;
                        pi < data.Length;
                        pi++, qual = pi - (poly.Length * (pi / poly.Length << 0))
                    )
                        data[pi] /= poly[qual] ?? 1f;
            }
            public static float[] SmoothOut(float[] data, float variance)
            {
                float tAvg = AverageArray(data) * variance;
                float[] poly = new float[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    float prev = i > 0 ? poly[i - 1] : data[i];
                    float next = i < data.Length ? data[i] : data[i - 1];
                    poly[i] = AverageArray(new float[] { tAvg, AverageArray(new float[] { prev, data[i], next }) });
                }
                return poly;
            }
            public static float SumArray(float[] array)
            {
                float result = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    result += array[i];
                }

                return result;
            }
            public static float AverageArray(float[] array)
            {
                float sumup = SumArray(array);
                float result = (float)sumup / array.Length;
                return result;
            }
            // var test = [[1, 2, 3, [1, 2, 3, [1, 2, 3], [11, 12, 13], [21, 22, 23]], [11, 12, 13], [21, 22, 23]], [11, 12, 13], [21, 22, 23]]
            // console.log(JSON.stringify(public static object[] Generate('poly', test, 10)))

        }
    }
}