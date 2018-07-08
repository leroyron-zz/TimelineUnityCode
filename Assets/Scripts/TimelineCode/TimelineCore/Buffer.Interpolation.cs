using System.Collections.Generic;
using UnityEngine;
using TLExtensions;
using TLMath;

public partial class Timeline
{
    public partial class Core
    {
        public partial class Buffer
        {
            public Interpolation interpolation = new Interpolation();
            public class Interpolation
            {
                private bool _preDefined = ControlPoint._preDefined;
                private float[][] cpList = ControlPoint.cpList;
                public class ControlPoint : ControlPointIndex
                {
                    public float time = 0f;
                    public float value = 0f;
                    public int pos = 0;
                    public float lastTime
                    {
                        get
                        {
                            return _lastTime[this.pos];
                        }
                        set
                        {
                            _lastTime[this.pos] = value;
                        }
                    }
                    public bool first, last;

                    public ControlPoint(int index, float time, float value, AnimationCurve curve, bool first = false, bool last = false)
                    {
                        this.first = first;
                        this.last = last;
                        this.pos = index;
                        if (time <= lastTime)
                        {
                            lastTime += 0.01f;
                            time = lastTime;
                        }
                        this.time = time > 1f ? 1f : time < 0f ? 0f : time;
                        this.time = first ? 0f : last ? 1f : this.time;
                        this.value = value > 1f ? 1f : value < 0f ? 0f : value;
                        curve.AddKey(this.time, this.value);
                    }
                }
                public string CurveToString(AnimationCurve curve)
                {
                    string str = "";
                    for (int k = 0; k < curve.keys.Length; k++) str += k < curve.keys.Length - 1
                    ? curve.keys[k].time.ToString() + ", " + curve.keys[k].value.ToString() + ", " : curve.keys[k].time.ToString() + ", " + curve.keys[k].value.ToString();
                    return str;
                }
                public float[] CurveToArray(AnimationCurve curve)
                {
                    float[] arr = new float[curve.keys.Length * 2];
                    for (int k = 0, v = 0; k < curve.keys.Length; k++)
                    {
                        arr[v++] = curve.keys[k].time;
                        arr[v++] = curve.keys[k].value;
                    }
                    return arr;
                }
                public float[][] CurveTo2dArray(AnimationCurve curve)
                {
                    float[][] arr = new float[curve.keys.Length][];
                    for (int k = 0; k < curve.keys.Length; k++)
                    {
                        arr[k] = new float[] { curve.keys[k].time, curve.keys[k].value };
                    }
                    return arr;
                }
                public float[][] ArrayTo2dArray(float[] array)
                {
                    float[][] arr = new float[array.Length / 2][];
                    for (int k = 0, i = 0; k < array.Length; k++, i++)
                    {
                        arr[i] = new float[2] { array[k++], array[k] };
                    }
                    return arr;
                }

                // Data Collections for interpolations store
                // To-Do make expiries for data Cache
                Dictionary<int, IDictionary<int, float[]>> DataCache = new Dictionary<int, IDictionary<int, float[]>>();
                public float[] EvalData(string cpName, int duration, bool catmull = false, bool display = false)
                {
                    int index = (int)(controlPoints.Names)System.Enum.Parse(typeof(controlPoints.Names), cpName);
                    return CacheEvalData(index, duration, catmull, display);
                }
                public float[] EvalData(float[] array, int duration, bool catmull = false, bool display = false)
                {
                    return !catmull ? SplineData(array, duration, display) : CatmullData(ArrayTo2dArray(array), duration, display);
                }
                public float[] EvalData(AnimationCurve curve, int duration, bool catmull = false, bool display = false)
                {
                    float[] array = CurveToArray(curve);
                    return !catmull ? SplineData(array, duration, display) : CatmullData(ArrayTo2dArray(array), duration, display);
                }
                public float[] EvalData(int index, int duration, bool catmull = false, bool display = false)
                {
                    return CacheEvalData(index, duration, catmull, display);
                }
                float[] CacheEvalData(int index, int duration, bool catmull = false, bool display = false)
                {
                    if (DataCache.ContainsKey(index) && DataCache[index].ContainsKey(duration)) return DataCache[index][duration];
                    float[] data = !catmull ? SplineData(controlPoints.cpList[index], duration, display) : CatmullData(ArrayTo2dArray(controlPoints.cpList[index]), duration, display);
                    if (!DataCache.ContainsKey(index))
                        DataCache.Add(index, new Dictionary<int, float[]>());
                        DataCache[index].Add(duration, data);
                    return data;
                }
                float[] SplineData(float[] array, int duration, bool display)
                {
                    int cplen = array.Length / 2;
                    float[][] CPEval = new float[cplen][];
                    for (int cp = 0, i = 0; cp < array.Length; cp++, i++)
                    {
                        float curTFrac = array[cp++];
                        float curPFrac = array[cp];
                        CPEval[i] = new float[] { duration * curTFrac, curPFrac };
                    }

                    // CPs.sort(function(a,b){return a.t-b.t;});
                    float[] ts = new float[cplen];
                    float[] ps = new float[cplen];
                    float[] ks = new float[cplen];
                    for (int cp = 0; cp < cplen; cp++)
                    {
                        float[] d = CPEval[cp];
                        ts[cp] = d[0]; ps[cp] = d[1]; ks[cp] = 1;
                    }

                    CSPL.GetNaturalKs(ts, ps, ks);

                    int minT = (int)CPEval[0][0];
                    int maxT = (int)CPEval[cplen - 1][0];

                    if (maxT < 1) return new float[0];
                    float[] data = new float[maxT];
                    for (int i = minT; i < maxT; i++)
                    {
                        data[i] = CSPL.EvalSpline(i, ts, ps, ks);// Scale numbers
                    }

                    return data;
                }

                //Display a spline between 2 points derived with the Catmull-Rom spline algorithm
                //Has to be at least 4 points
                float[] CatmullData(float[][] array, int duration, bool display = false, bool isLooping = false)
                {
                    float[] predata = new float[0];
                    int first = 1;
                    for (int i = 0; i < array.Length; i++)
                    {
                        //Cant draw between the endpoints
                        //Neither do we need to draw from the second to the last endpoint
                        //...if we are not making a looping line
                        if ((i == array.Length - 1) && !isLooping)
                        {
                            continue;
                        }

                        //DisplayCatmullRomSpline(i);
                        float time = i + 1 < array.Length ? ((array[i + 1][0] - array[i][0]) * duration) : 1;
                        int cpLength = array.Length;
                        float[] p0 = array[ClampListPos(i - 1, cpLength)];
                        float[] p1 = array[i];
                        float[] p2 = array[ClampListPos(i + 1, cpLength)];
                        float[] p3 = array[ClampListPos(i + 2, cpLength)];

                        //float lastPos = p0[1];

                        float frac = 1f / time;
                        int resLoops = Mathf.FloorToInt(1f / frac);
                        // fix to first val
                        resLoops += first;
                        float[] data = new float[resLoops];

                        for (int d = first; d < resLoops; d++)
                        {
                            //Which t position are we at?
                            float t = d * frac;

                            //Find the coordinate between the end points with a Catmull-Rom spline
                            //float newPos = GetCatmullRomPosition(t, p0.value, p1.value, p2.value, p3.value);
                            data[d] = GetCatmullRomPosition(t, p0[1], p1[1], p2[1], p3[1]);
                            //Draw this line segment
                            //if (display) Gizmos.DrawLine(lastPos, data[d]);

                            //Save this pos so we can draw the next line segment
                            //lastPos = data[d];
                        }
                        first = 0;

                        // float[] temp = CatmullData(this.index, time);
                        predata = predata.Concat(data);
                        //int[] timeData = CatmullData(i);
                    }
                    predata[0] = 1;//  first
                    return predata;
                }

                //Clamp the list positions to allow looping
                int ClampListPos(int pos, int len)
                {
                    if (pos < 0)
                    {
                        pos = len - 1;
                    }

                    if (pos > len)
                    {
                        pos = 1;
                    }
                    else if (pos > len - 1)
                    {
                        pos = 0;
                    }

                    return pos;
                }

                //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
                //http://www.iquilezles.org/www/articles/minispline/minispline.htm
                Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
                {
                    //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
                    Vector3 a = 2f * p1;
                    Vector3 b = p2 - p0;
                    Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
                    Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

                    //The cubic polynomial: a + b * t + c * t^2 + d * t^3
                    Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

                    return pos;
                }

                float GetCatmullRomPosition(float t, float p0, float p1, float p2, float p3)
                {
                    //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
                    float a = 2f * p1;
                    float b = p2 - p0;
                    float c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
                    float d = -p0 + 3f * p1 - 3f * p2 + p3;

                    //The cubic polynomial: a + b * t + c * t^2 + d * t^3
                    float pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

                    return pos;
                }

                public static class controlPoints
                {
                    public static int _index = 0;
                    public static float[] _lastTime = new float[10000];
                    public static bool _preDefined { get { return _index < cpList.Length; } }
                    public static int index { get { return _preDefined ? _index : cpList.Length - 1; } }
                    public static float[][] cpList = new float[32][] 
                    {
                        new float[]{0f, 1f, 1/3f, 2/3f, 2/3f, 1/3f, 1f, 0F},
                        // Sine
                        new float[]{0f, 1f, 0.17f, 0.96f, 1f, 0F},
                        new float[]{0f, 1f, 0.83f, 0.03f, 1f, 0F},
                        new float[]{0f, 1f, 0.19f, 0.92f, 0.81f, 0.08f, 1f, 0F},
                        // Quad
                        new float[]{0f, 1f, 0.32f, 0.89f, 0.75f, 0.44f, 1f, 0F},
                        new float[]{0f, 1f, 0.17f, 0.69f, 0.74f, 0.06f, 1f, 0F},
                        new float[]{0f, 1f, 0.17f, 0.93f, 0.35f, 0.75f, 0.55f, 0.4f, 0.8f, 0.07f, 1f, 0F},
                        // Cubic
                        new float[]{0f, 1f, 0.17f, 0.99f, 0.42f, 0.92f, 0.66f, 0.71f, 0.86f, 0.35f, 1f, 0F},
                        new float[]{0f, 1f, 0.15f, 0.59f, 0.33f, 0.3f, 0.57f, 0.08f, 0.83f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.22f, 0.96f, 0.38f, 0.77f, 0.5f, 0.5f, 0.64f, 0.18f, 0.82f, 0.02f, 1f, 0F},
                        // Quart
                        new float[]{0f, 1f, 0.21f, 1f, 0.42f, 0.96f, 0.63f, 0.84f, 0.82f, 0.54f, 0.92f, 0.28f, 1f, 0F},
                        new float[]{0f, 1f, 0.12f, 0.58f, 0.27f, 0.28f, 0.45f, 0.09f, 0.66f, 0.01f, 0.81f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.15f, 1f, 0.28f, 0.94f, 0.41f, 0.76f, 0.5f, 0.49f, 0.57f, 0.26f, 0.68f, 0.08f, 0.84f, 0f, 1f, 0F},
                        // Quint
                        new float[]{0f, 1f, 0.24f, 1f, 0.43f, 0.98f, 0.64f, 0.89f, 0.81f, 0.64f, 0.93f, 0.28f, 1f, 0F},
                        new float[]{0f, 1f, 0.09f, 0.61f, 0.23f, 0.26f, 0.37f, 0.1f, 0.59f, 0.01f, 0.78f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.16f, 0.99f, 0.28f, 0.96f, 0.45f, 0.7f, 0.52f, 0.39f, 0.6f, 0.17f, 0.75f, 0.01f, 0.88f, 0f, 1f, 0F},
                        // Expo
                        new float[]{0f, 1f, 0.09f, 0.99f, 0.56f, 0.94f, 0.72f, 0.85f, 0.83f, 0.69f, 0.92f, 0.42f, 1f, 0F},
                        new float[]{0f, 1f, 0.05f, 0.7f, 0.22f, 0.21f, 0.48f, 0.03f, 0.64f, 0.01f, 0.93f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.11f, 0.99f, 0.21f, 0.99f, 0.3f, 0.96f, 0.38f, 0.9f, 0.47f, 0.67f, 0.53f, 0.33f, 0.6f, 0.12f, 0.72f, 0.02f, 0.79f, 0f, 0.85f, 0f, 1f, 0F},
                        // Circ
                        new float[]{0f, 1f, 0.33f, 0.94f, 0.54f, 0.83f, 0.81f, 0.58f, 0.94f, 0.35f, 0.99f, 0.1f, 1f, 0F},
                        new float[]{0f, 1f, 0.02f, 0.9f, 0.06f, 0.66f, 0.14f, 0.48f, 0.26f, 0.32f, 0.41f, 0.19f, 0.6f, 0.08f, 0.79f, 0.02f, 1f, 0F},
                        new float[]{0f, 1f, 0.17f, 0.97f, 0.27f, 0.92f, 0.37f, 0.83f, 0.44f, 0.74f, 0.47f, 0.67f, 0.49f, 0.6f, 0.5f, 0.5f, 0.51f, 0.41f, 0.53f, 0.31f, 0.57f, 0.22f, 0.62f, 0.17f, 0.71f, 0.09f, 0.82f, 0.03f, 1f, 0F},
                        // Back
                        new float[]{0f, 1f, 0.04f, 1f, 0.64f, 1f, 0.86f, 0.53f, 1f, 0F},
                        new float[]{0f, 1f, 0.03f, 0.86f, 0.35f, 0.01f, 0.95f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.03f, 1f, 0.46f, 0.69f, 0.51f, 0.44f, 0.56f, 0.22f, 0.97f, 0f, 1f, 0F},
                        // Elastic
                        new float[]{0f, 1f, 0.08f, 0.99f, 0.15f, 0.99f, 0.22f, 1f, 0.32f, 1f, 0.41f, 0.98f, 0.47f, 0.99f, 0.63f, 0.99f, 0.74f, 0.9f, 0.93f, 0.93f, 0.95f, 0.6f, 1f, 0F},
                        new float[]{0f, 1f, 0.03f, 0.65f, 0.07f, 0.02f, 0.26f, 0.11f, 0.38f, 0f, 0.56f, 0.01f, 0.69f, 0f, 0.75f, 0f, 0.83f, 0f, 0.9f, 0f, 1f, 0F},
                        new float[]{0f, 1f, 0.13f, 1f, 0.23f, 0.99f, 0.33f, 0.99f, 0.45f, 0.95f, 0.48f, 0.67f, 0.51f, 0.41f, 0.55f, 0.04f, 0.67f, 0f, 0.78f, 0f, 0.87f, 0f, 1f, 0F},
                        // Bounce
                        new float[]{0f, 1f, 0.04f, 0.98f, 0.08f, 1f, 0.12f, 0.96f, 0.23f, 0.97f, 0.26f, 1f, 0.28f, 0.96f, 0.6f, 0.95f, 0.63f, 1f, 0.68f, 0.8f, 0.87f, 0.13f, 1f, 0F},
                        new float[]{0f, 1f, 0.15f, 0.83f, 0.28f, 0.4f, 0.33f, 0.17f, 0.36f, 0.02f, 0.38f, 0.04f, 0.71f, 0.04f, 0.73f, 0f, 0.77f, 0.04f, 0.88f, 0.03f, 0.91f, 0f, 0.94f, 0.01f, 1f, 0F},
                        new float[]{0f, 1f, 0.03f, 0.98f, 0.05f, 0.99f, 0.09f, 0.97f, 0.13f, 0.99f, 0.16f, 0.94f, 0.28f, 0.94f, 0.31f, 0.98f, 0.37f, 0.77f, 0.44f, 0.55f, 0.55f, 0.46f, 0.64f, 0.19f, 0.68f, 0.01f, 0.72f, 0.06f, 0.82f, 0.07f, 0.86f, 0f, 0.91f, 0.03f, 0.95f, 0f, 0.97f, 0.01f, 1f, 0F},
                        // Linear
                        new float[]{0f, 1f, 0.2f, 0.8f, 0.4f, 0.6f, 0.6f, 0.4f, 0.8f, 0.2f, 1f, 0F},
                    };
                    public enum Names
                    {
                        linear,
                        easeInSine,
                        easeOutSine,
                        easeInOutSine,
                        easeInQuad,
                        easeOutQuad,
                        easeInOutQuad,
                        easeInCubic,
                        easeOutCubic,
                        easeInOutCubic,
                        easeInQuart,
                        easeOutQuart,
                        easeInOutQuart,
                        easeInQuint,
                        easeOutQuint,
                        easeInOutQuint,
                        easeInExpo,
                        easeOutExpo,
                        easeInOutExpo,
                        easeInCirc,
                        easeOutCirc,
                        easeInOutCirc,
                        easeInBack,
                        easeOutBack,
                        easeInOutBack,
                        easeInElastic,
                        easeOutElastic,
                        easeInOutElastic,
                        easeInBounce,
                        easeOutBounce,
                        easeInOutBounce
                    }
                }
                class CSPL
                {
                    public static void Solve(float[][] A, float[] x)
                    {
                        int m = A.Length;
                        // column
                        for (int k = 0; k < m; k++)
                        {
                            // pivot for column
                            int iMax = 0; float vali = Mathf.NegativeInfinity;
                            for (int i = k; i < m; i++)
                            {
                                if (A[i][k] > vali)
                                {
                                    iMax = i;
                                    vali = A[i][k];
                                }
                            }
                            SwapRows(A, k, iMax);

                            if (A[iMax][k] == 0) Debug.Log("matrix is singular!");

                            // for all rows below pivot
                            for (int i = k + 1; i < m; i++)
                            {
                                for (int j = k + 1; j < m + 1; j++)
                                {
                                    A[i][j] = A[i][j] - A[k][j] * (A[i][k] / A[k][k]);
                                }
                                A[i][k] = 0;
                            }
                        }

                        // rows = columns
                        for (int i = m - 1; i >= 0; i--)
                        {
                            float v = A[i][m] / A[i][i];
                            x[i] = v;
                            // rows
                            for (int j = i - 1; j >= 0; j--)
                            {
                                A[j][m] -= A[j][i] * v;
                                A[j][i] = 0;
                            }
                        }
                    }
                    public static float[][] ZerosMat(int r, int c)
                    {
                        float[][] A = new float[r][];
                        for (int i = 0; i < r; i++)
                        {
                            A[i] = new float[c];
                        }
                        return A;
                    }
                    public static void SwapRows(float[][] m, int k, int l)
                    {
                        float[] p = m[k];
                        m[k] = m[l]; m[l] = p;
                    }
                    public static void GetNaturalKs(float[] xs, float[] ys, float[] ks)
                    {
                        int n = xs.Length - 1;
                        float[][] A = ZerosMat(n + 1, n + 2);

                        // rows
                        for (int i = 1; i < n; i++)
                        {
                            A[i][i - 1] = 1 / (xs[i] - xs[i - 1]);
                            A[i][i] = 2 * (1 / (xs[i] - xs[i - 1]) + 1 / (xs[i + 1] - xs[i]));
                            A[i][i + 1] = 1 / (xs[i + 1] - xs[i]);
                            A[i][n + 1] = (
                                3 * (
                                    (ys[i] - ys[i - 1]) / (
                                        (xs[i] - xs[i - 1]) * (xs[i] - xs[i - 1])
                                    ) + (ys[i + 1] - ys[i]) / (
                                        (xs[i + 1] - xs[i]) * (xs[i + 1] - xs[i])
                                    )
                                )
                            );
                        }

                        A[0][0] = 2 / (xs[1] - xs[0]);
                        A[0][1] = 1 / (xs[1] - xs[0]);
                        A[0][n + 1] = 3 * (ys[1] - ys[0]) / ((xs[1] - xs[0]) * (xs[1] - xs[0]));

                        A[n][n - 1] = 1 / (xs[n] - xs[n - 1]);
                        A[n][n] = 2 / (xs[n] - xs[n - 1]);
                        A[n][n + 1] = (
                            3 * (
                                ys[n] - ys[n - 1]
                            ) / (
                                (xs[n] - xs[n - 1]) * (xs[n] - xs[n - 1])
                            )
                        );

                        Solve(A, ks);
                    }
                    public static float EvalSpline(float x, float[] xs, float[] ys, float[] ks)
                    {
                        var i = 1;
                        while (xs[i] < x) i++;

                        var t = (x - xs[i - 1]) / (xs[i] - xs[i - 1]);

                        var a = ks[i - 1] * (xs[i] - xs[i - 1]) - (ys[i] - ys[i - 1]);
                        var b = -ks[i] * (xs[i] - xs[i - 1]) + (ys[i] - ys[i - 1]);

                        var q = (
                            (1 - t) * ys[i - 1] + t * ys[i] + t * (1 - t) * (a * (1 - t) + b * t)
                        );
                        return q;
                    }
                }
            }
        }
    }
}