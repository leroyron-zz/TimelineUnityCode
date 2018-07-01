using UnityEngine;
using TLExtensions;
using TLMath;

public class ControlPoints : MonoBehaviour {
	private bool _preDefined = ControlPoint._preDefined;
	public int index;
	private int _deltaIndex;
	[Tooltip("Graphic Curve (Read Only)")]
	public AnimationCurve curve = new AnimationCurve();
	private float[][] cpList = ControlPoint.cpList;
	private int cpPropLength;
	[ControlPointAttribute (new string[]{"cp"}, 100)]
	[Tooltip("Control Points - Interpolation")]
	public ControlPoint[] controlPoints;
	public string list;

	ControlPoints() {
		index = ControlPointIndex.index;
		this.ReComp(index);
		ControlPoint._index++;
	}
	public void Update() {
		if (index >= cpList.Length) {
			cpList = cpList.ConcatFrom(new float[index+1][]);
		}
		if (index == _deltaIndex || index < 0) return;
		_deltaIndex = index;
		this.ReComp(index);
	}
	private void ReComp(int index) {
		if (cpList[index] == null) cpList[index] = cpList[0];
		cpPropLength = cpList[index].Length;
		controlPoints = new ControlPoint[cpList[index].Length/2];
		ControlPoint._lastTime[index] = 0;
		for (int c = 0; c < cpPropLength-1; c++) {
			bool first = c == 0;
        	bool last = c == cpPropLength - 2;
			float time = cpList[index][c];
			float value = cpList[index][++c];
			controlPoints[(int)c/2] = new ControlPoint(index, time, value, curve, first, last);
		}
		Sample();
	}

	class CSPL {
		public static void Solve(float[][] A, float[] x) {
			int m = A.Length;
			// column
			for (int k = 0; k < m; k++) {
				// pivot for column
				int iMax = 0; float vali = Mathf.NegativeInfinity;
				for (int i = k; i < m; i++) {
					if (A[i][k] > vali) {
						iMax = i;
						vali = A[i][k];
					}
				}
				SwapRows(A, k, iMax);

				if (A[iMax][k] == 0) Debug.Log("matrix is singular!");

				// for all rows below pivot
				for (int i = k + 1; i < m; i++) {
					for (int j = k + 1; j < m + 1; j++) {
						A[i][j] = A[i][j] - A[k][j] * (A[i][k] / A[k][k]);
					}
					A[i][k] = 0;
				}
			}

			// rows = columns
			for (int i = m - 1; i >= 0; i--) {
				float v = A[i][m] / A[i][i];
				x[i] = v;
				// rows
				for (int j = i - 1; j >= 0; j--) {
					A[j][m] -= A[j][i] * v;
					A[j][i] = 0;
				}
			}
		}
		public static float[][] ZerosMat(int r, int c) {
			float[][] A = new float[r][];
			for (int i = 0; i < r; i++) {
				A[i] = new float[c];
			}
			return A;
		}
		public static void SwapRows(float[][] m, int k, int l) {
			float[] p = m[k];
			m[k] = m[l]; m[l] = p;
		}
		public static void GetNaturalKs(float[] xs, float[] ys, float[] ks) {
			int n = xs.Length - 1;
			float[][] A = ZerosMat(n + 1, n + 2);

			// rows
			for (int i = 1; i < n; i++) {
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
		public static float EvalSpline(float x, float[] xs, float[] ys, float[] ks) {
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

	[System.Serializable]
	public class ControlPoint : ControlPointIndex
	{
		public float time = 0f;
		public float value = 0f;
		public int pos = 0;
		public float lastTime {
			get { 
				return _lastTime[this.pos]; 
			} 
			set { 
				_lastTime[this.pos] = value; 
			}
		}
		public bool first, last;

		public ControlPoint(int index, float time, float value, AnimationCurve curve, bool first = false, bool last = false)
		{
			this.first = first;
			this.last = last;
			this.pos = index;
			if (time <= lastTime) {
				lastTime += 0.01f;
				time = lastTime;
			}
			this.time = time > 1f ? 1f : time < 0f ? 0f : time;
			this.time = first ? 0f : last ? 1f : this.time;
			this.value = value > 1f ? 1f : value < 0f ? 0f : value;
			curve.AddKey(this.time, this.value);
		}
	}
	public string CurveToString(AnimationCurve curve) {
		string str = "";
		for (int k = 0; k < curve.keys.Length; k++) str += k < curve.keys.Length-1 
		? curve.keys[k].time.ToString()+", "+curve.keys[k].value.ToString()+", " : curve.keys[k].time.ToString()+", "+curve.keys[k].value.ToString();
		return str;
	}
	public float[] CurveToArray(AnimationCurve curve) {
		float[] arr = new float[curve.keys.Length *2];
		for (int k = 0, v = 0; k < curve.keys.Length; k++) 
		{ 
			arr[v++] = curve.keys[k].time;
			arr[v++] = curve.keys[k].value;
		}
		return arr;
	}
	public float[][] CurveTo2dArray(AnimationCurve curve) {
		float[][] arr = new float[curve.keys.Length][];
		for (int k = 0; k < curve.keys.Length; k++) 
		{ 
			arr[k] = new float[] {curve.keys[k].time, curve.keys[k].value};
		}
		return arr;
	}
	public float[][] ArrayTo2dArray(float[] array) {
		float[][] arr = new float[array.Length / 2][];
		for (int k = 0, i = 0; k < array.Length; k++, i++) 
		{ 
			arr[i] = new float[2] {array[k++], array[k]};
		}
		return arr;
	}
	public int sampleTimeMs = 100;
	public float sampleValue = 500f;
	public string sampleData;
	public bool isLooping = false;
	public Transform[] controlPointsList;

	public void Sample() {
		sampleData = "";

		//float[] predata2 = EvalData(this.curve, sampleTimeMs, 1);
		
		//float[] predata3 = EvalData(index, sampleTimeMs, 1);
		
		if (this.index < 0 || this.index >= this.cpList.Length || this.cpList[this.index].Length < 2 || sampleTimeMs < 2) return;
		float[] predata = TMath.Poly.MultiplyScalarReverse(EvalData(this.cpList[this.index], sampleTimeMs, 1), sampleValue);
		sampleData = predata.FloatArrayToString();
	}
	float[] EvalData(float[] array, int duration, float precision, bool catmull = false, bool display = false) {
		return !catmull ? SplineData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] EvalData(AnimationCurve curve, int duration, float precision, bool catmull = false, bool display = false) {
		float[] array = CurveToArray(curve);
        return !catmull ? SplineData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] EvalData(int index, int duration, float precision, bool catmull = false, bool display = false) {
		float[] array = cpList[index];
		return !catmull ? SplineData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] SplineData(float[] array, int duration, float precision, bool display) {
		int cplen = array.Length / 2;
		float[][] CPEval = new float[cplen][];
        for (int cp = 0, i = 0; cp < array.Length; cp++, i++) {
            float curTFrac = array[cp++];
            float curPFrac = array[cp];
            CPEval[i] = new float[]{duration * curTFrac, precision * curPFrac};
        }

		// CPs.sort(function(a,b){return a.t-b.t;});
        float[] ts = new float[cplen];
        float[] ps = new float[cplen];
        float[] ks = new float[cplen];
        for (int cp = 0; cp < cplen; cp++) {
            float[] d = CPEval[cp];
            ts[cp] = d[0]; ps[cp] = d[1]; ks[cp] = 1;
        }

		CSPL.GetNaturalKs(ts, ps, ks);

        int minT = (int)CPEval[0][0];
        int maxT = (int)CPEval[cplen - 1][0];

		if (maxT < 1) return new float[0];
        float[] data = new float[maxT + 1];
        for (int i = minT; i <= maxT; i++) {
            data[i] = CSPL.EvalSpline(i, ts, ps, ks);// Scale numbers
        }

        return data; 
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	//Has to be at least 4 points
	float[] DataCatmullRomSpline(float[][] array, int duration, bool display) {
		float[] predata = new float[0];
		for (int i = 0; i < array.Length; i++)
		{
			//Cant draw between the endpoints
			//Neither do we need to draw from the second to the last endpoint
			//...if we are not making a looping line
			if (( i == array.Length - 1) && !isLooping)
			{
				continue;
			}

			//DisplayCatmullRomSpline(i);
			float time = i+1 < array.Length ? ((array[i+1][0] - array[i][0]) * this.sampleTimeMs) : 1;
			int cpLength = array.Length;
			float[] p0 = array[ClampListPos(i - 1, cpLength)];
			float[] p1 = array[i];
			float[] p2 = array[ClampListPos(i + 1, cpLength)];
			float[] p3 = array[ClampListPos(i + 2, cpLength)];

			//float lastPos = p0[1];

			float frac = 1f/time;

			int resLoops = Mathf.FloorToInt(1f / frac);
			float[] data = new float[resLoops+1];

			for (int d = 1; d <= resLoops; d++)
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

			// float[] temp = DataCatmullRomSpline(this.index, time);
			predata = predata.Concat(data);
			//int[] timeData = DataCatmullRomSpline(i);
		}
		predata[0] = 1;// 
		return predata;
	}

	//Display without having to press play
	void OnDrawGizmos()
	{
		if (controlPointsList == null || controlPointsList.Length == 0) return;
		float[] cpSubTime = SplineData(cpList[index], controlPointsList.Length - 1, 1, false);
		float[] time = new float[controlPointsList.Length];
		//Draw the Catmull-Rom spline between the points
		for (int i = 0; i < controlPointsList.Length; i++)
		{
			//Cant draw between the endpoints
			//Neither do we need to draw from the second to the last endpoint
			//...if we are not making a looping line
			time[i] = i+1 < cpSubTime.Length ? ((cpSubTime[i] - cpSubTime[i+1]) * this.sampleTimeMs) : cpSubTime[cpSubTime.Length-1] - cpSubTime[0];
			if (( i == controlPointsList.Length - 1) && !isLooping)
			{
				continue;
			}
			Gizmos.color = Color.green;
			DisplayCatmullRomSpline(i, time[i]);
			
			//int[] timeData = DataCatmullRomSpline(i);
		}
		Gizmos.color = Color.yellow;
		DisplaySpline(cpSubTime);
	}

	void DisplaySpline(float[] cpSubTime)
	{
		int cpLength = controlPointsList.Count();
		float[] xPos = new float[cpLength * 2];
		float[] yPos = new float[cpLength * 2];
		float[] zPos = new float[cpLength * 2];
		for (int i = 0, x = 0, y = 0, z = 0; i < cpLength; i++) {
			// time / value
			xPos[x++] = 1 - cpSubTime[i]; xPos[x++] = controlPointsList[i].position.x;
			yPos[y++] = 1 - cpSubTime[i]; yPos[y++] = controlPointsList[i].position.y;
			zPos[z++] = 1 - cpSubTime[i]; zPos[z++] = controlPointsList[i].position.z;
		}
		if (cpLength < 2) return;
		float[] xData = SplineData(xPos, this.sampleTimeMs, 1, false);
		float[] yData = SplineData(yPos, this.sampleTimeMs, 1, false);
		float[] zData = SplineData(zPos, this.sampleTimeMs, 1, false);
		Vector3 p = new Vector3();
		Vector3 _deltap = new Vector3(xData[0], yData[0], zData[0]);
		for (int i = 1; i < xData.Length; i++) {
			p.x = xData[i];
			p.y = yData[i];
			p.z = zData[i];
			Gizmos.DrawCube(_deltap, size);
			Gizmos.DrawLine(_deltap, p);
			_deltap = p;
		}
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
	void DisplayCatmullRomSpline(int pos, float timeF)
	{
		int time = (int)timeF;
		//The 4 points we need to form a spline between p1 and p2
		int cpLength = controlPointsList.Count();
		if (cpLength < 2 || pos >= cpLength) return;
		Vector3 p0 = controlPointsList[ClampListPos(pos - 1, cpLength)].position;
		Vector3 p1 = controlPointsList[pos].position;
		Vector3 p2 = controlPointsList[ClampListPos(pos + 1, cpLength)].position;
		Vector3 p3 = controlPointsList[ClampListPos(pos + 2, cpLength)].position;

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		//float resolution = 0.2f;
		float frac = 1f/time;

		//How many times should we loop?
		//By distance make segments *
		int resLoops = Mathf.FloorToInt(1f / frac);

		for (int i = 1; i <= resLoops; i++)
		{
			//Which t position are we at?
			float t = i * frac;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

			//Draw this point
			Gizmos.DrawCube(newPos, size);

			//Draw this line segment
			Gizmos.DrawLine(lastPos, newPos);

			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
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
}

public class ControlPointIndex
{
	public static int _index = 0;
	public static float[] _lastTime = new float[10000];
	public static bool _preDefined { get {return _index < cpList.Length;} }
	public static int index { get { return _preDefined ? _index : cpList.Length - 1;} }
	public static float[][] cpList = new float[32][] {
		new float[]{0f, 1f, 1/3f, 2/3f, 2/3f, 1/3f, 1f, 0F},
        new float[]{0f, 1f, 0.17f, 0.96f, 1f, 0F},
        new float[]{0f, 1f, 0.83f, 0.03f, 1f, 0F},
        new float[]{0f, 1f, 0.19f, 0.92f, 0.81f, 0.08f, 1f, 0F},
        new float[]{0f, 1f, 0.32f, 0.89f, 0.75f, 0.44f, 1f, 0F},
        new float[]{0f, 1f, 0.17f, 0.69f, 0.74f, 0.06f, 1f, 0F},
        new float[]{0f, 1f, 0.17f, 0.93f, 0.35f, 0.75f, 0.55f, 0.4f, 0.8f, 0.07f, 1f, 0F},
        new float[]{0f, 1f, 0.17f, 0.99f, 0.42f, 0.92f, 0.66f, 0.71f, 0.86f, 0.35f, 1f, 0F},
        new float[]{0f, 1f, 0.15f, 0.59f, 0.33f, 0.3f, 0.57f, 0.08f, 0.83f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.22f, 0.96f, 0.38f, 0.77f, 0.5f, 0.5f, 0.64f, 0.18f, 0.82f, 0.02f, 1f, 0F},
        new float[]{0f, 1f, 0.21f, 1f, 0.42f, 0.96f, 0.63f, 0.84f, 0.82f, 0.54f, 0.92f, 0.28f, 1f, 0F},
        new float[]{0f, 1f, 0.12f, 0.58f, 0.27f, 0.28f, 0.45f, 0.09f, 0.66f, 0.01f, 0.81f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.15f, 1f, 0.28f, 0.94f, 0.41f, 0.76f, 0.5f, 0.49f, 0.57f, 0.26f, 0.68f, 0.08f, 0.84f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.24f, 1f, 0.43f, 0.98f, 0.64f, 0.89f, 0.81f, 0.64f, 0.93f, 0.28f, 1f, 0F},
        new float[]{0f, 1f, 0.09f, 0.61f, 0.23f, 0.26f, 0.37f, 0.1f, 0.59f, 0.01f, 0.78f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.16f, 0.99f, 0.28f, 0.96f, 0.45f, 0.7f, 0.52f, 0.39f, 0.6f, 0.17f, 0.75f, 0.01f, 0.88f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.09f, 0.99f, 0.56f, 0.94f, 0.72f, 0.85f, 0.83f, 0.69f, 0.92f, 0.42f, 1f, 0F},
        new float[]{0f, 1f, 0.05f, 0.7f, 0.22f, 0.21f, 0.48f, 0.03f, 0.64f, 0.01f, 0.93f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.11f, 0.99f, 0.21f, 0.99f, 0.3f, 0.96f, 0.38f, 0.9f, 0.47f, 0.67f, 0.53f, 0.33f, 0.6f, 0.12f, 0.72f, 0.02f, 0.79f, 0f, 0.85f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.33f, 0.94f, 0.54f, 0.83f, 0.81f, 0.58f, 0.94f, 0.35f, 0.99f, 0.1f, 1f, 0F},
        new float[]{0f, 1f, 0.02f, 0.9f, 0.06f, 0.66f, 0.14f, 0.48f, 0.26f, 0.32f, 0.41f, 0.19f, 0.6f, 0.08f, 0.79f, 0.02f, 1f, 0F},
        new float[]{0f, 1f, 0.17f, 0.97f, 0.27f, 0.92f, 0.37f, 0.83f, 0.44f, 0.74f, 0.47f, 0.67f, 0.49f, 0.6f, 0.5f, 0.5f, 0.51f, 0.41f, 0.53f, 0.31f, 0.57f, 0.22f, 0.62f, 0.17f, 0.71f, 0.09f, 0.82f, 0.03f, 1f, 0F},
        new float[]{0f, 1f, 0.04f, 1f, 0.64f, 1f, 0.86f, 0.53f, 1f, 0F},
        new float[]{0f, 1f, 0.03f, 0.86f, 0.35f, 0.01f, 0.95f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.03f, 1f, 0.46f, 0.69f, 0.51f, 0.44f, 0.56f, 0.22f, 0.97f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.08f, 0.99f, 0.15f, 0.99f, 0.22f, 1f, 0.32f, 1f, 0.41f, 0.98f, 0.47f, 0.99f, 0.63f, 0.99f, 0.74f, 0.9f, 0.93f, 0.93f, 0.95f, 0.6f, 1f, 0F},
        new float[]{0f, 1f, 0.03f, 0.65f, 0.07f, 0.02f, 0.26f, 0.11f, 0.38f, 0f, 0.56f, 0.01f, 0.69f, 0f, 0.75f, 0f, 0.83f, 0f, 0.9f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.13f, 1f, 0.23f, 0.99f, 0.33f, 0.99f, 0.45f, 0.95f, 0.48f, 0.67f, 0.51f, 0.41f, 0.55f, 0.04f, 0.67f, 0f, 0.78f, 0f, 0.87f, 0f, 1f, 0F},
        new float[]{0f, 1f, 0.04f, 0.98f, 0.08f, 1f, 0.12f, 0.96f, 0.23f, 0.97f, 0.26f, 1f, 0.28f, 0.96f, 0.6f, 0.95f, 0.63f, 1f, 0.68f, 0.8f, 0.87f, 0.13f, 1f, 0F},
        new float[]{0f, 1f, 0.15f, 0.83f, 0.28f, 0.4f, 0.33f, 0.17f, 0.36f, 0.02f, 0.38f, 0.04f, 0.71f, 0.04f, 0.73f, 0f, 0.77f, 0.04f, 0.88f, 0.03f, 0.91f, 0f, 0.94f, 0.01f, 1f, 0F},
        new float[]{0f, 1f, 0.03f, 0.98f, 0.05f, 0.99f, 0.09f, 0.97f, 0.13f, 0.99f, 0.16f, 0.94f, 0.28f, 0.94f, 0.31f, 0.98f, 0.37f, 0.77f, 0.44f, 0.55f, 0.55f, 0.46f, 0.64f, 0.19f, 0.68f, 0.01f, 0.72f, 0.06f, 0.82f, 0.07f, 0.86f, 0f, 0.91f, 0.03f, 0.95f, 0f, 0.97f, 0.01f, 1f, 0F},
		new float[]{0f, 1f, 0.2f, 0.8f, 0.4f, 0.6f, 0.6f, 0.4f, 0.8f, 0.2f, 1f, 0F},
	};
}

public class ControlPointAttribute : PropertyAttribute
{
    public readonly string[] names;
	public string[] list;
    public ControlPointAttribute(string[] names, int length = 0) {
		if (length == 0 || names.Length == 0) { this.names = names; return; }
        int namesLength = names.Length - 1;
		this.list = new string[namesLength + length];
		for (int n = 0; n < namesLength; n++) this.list[n] = names[n];
		for (int i = 0; i < length; i++) this.list[namesLength+i] = names[namesLength] + i;
		this.names = this.list;	
	}
}