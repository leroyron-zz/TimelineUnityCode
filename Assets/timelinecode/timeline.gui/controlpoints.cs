using UnityEngine;
using TLExtensions;
using TLMath;

public class controlpoints : MonoBehaviour {
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

	controlpoints () {
		index = ControlPointIndex.index;
		this.recomp(index);
	}
	public void update () {
		if (index >= cpList.Length) {
			cpList = cpList.ConcatFrom(new float[index+1][]);
		}
		if (index == _deltaIndex || index < 0) return;
		_deltaIndex = index;
		this.recomp(index);
	}
	private void recomp(int index) {
		if (cpList[index] == null) cpList[index] = cpList[0];
		cpPropLength = cpList[index].Length;
		controlPoints = new ControlPoint[cpList[index].Length/2];
		ControlPoint._lastTime[index] = 0;
		ControlPoint._index++;
		for (int c = 0; c < cpPropLength-1; c++) {
			bool first = c == 0;
        	bool last = c == cpPropLength - 2;
			float time = cpList[index][c];
			float value = cpList[index][++c];
			controlPoints[(int)c/2] = new ControlPoint(index, time, value, curve, first, last);
		}
		sample();
	}

	class CSPL {
		public static void solve (float[][] A, float[] x) {
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
				swapRows(A, k, iMax);

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
		public static float[][] zerosMat (int r, int c) {
			float[][] A = new float[r][];
			for (int i = 0; i < r; i++) {
				A[i] = new float[c];
			}
			return A;
		}
		public static void swapRows (float[][] m, int k, int l) {
			float[] p = m[k];
			m[k] = m[l]; m[l] = p;
		}
		public static void getNaturalKs (float[] xs, float[] ys, float[] ks) {
			int n = xs.Length - 1;
			float[][] A = zerosMat(n + 1, n + 2);

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

			solve(A, ks);
		}
		public static float evalSpline (float x, float[] xs, float[] ys, float[] ks) {
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
		public float time = 0F;
		public float value = 0F;
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
				lastTime += 0.01F;
				time = lastTime;
			}
			this.time = time > 1F ? 1F : time < 0F ? 0F : time;
			this.time = first ? 0F : last ? 1F : this.time;
			this.value = value > 1F ? 1F : value < 0F ? 0F : value;
			curve.AddKey(this.time, this.value);
		}
	}
	public string CurveToString (AnimationCurve curve) {
		string str = "";
		for (int k = 0; k < curve.keys.Length; k++) str += k < curve.keys.Length-1 
		? curve.keys[k].time.ToString()+", "+curve.keys[k].value.ToString()+", " : curve.keys[k].time.ToString()+", "+curve.keys[k].value.ToString();
		return str;
	}
	public float[] CurveToArray (AnimationCurve curve) {
		float[] arr = new float[curve.keys.Length *2];
		for (int k = 0, v = 0; k < curve.keys.Length; k++) 
		{ 
			arr[v++] = curve.keys[k].time;
			arr[v++] = curve.keys[k].value;
		}
		return arr;
	}
	public float[][] CurveTo2dArray (AnimationCurve curve) {
		float[][] arr = new float[curve.keys.Length][];
		for (int k = 0; k < curve.keys.Length; k++) 
		{ 
			arr[k] = new float[] {curve.keys[k].time, curve.keys[k].value};
		}
		return arr;
	}
	public float[][] ArrayTo2dArray (float[] array) {
		float[][] arr = new float[array.Length / 2][];
		for (int k = 0, i = 0; k < array.Length; k++, i++) 
		{ 
			arr[i] = new float[2] {array[k++], array[k]};
		}
		return arr;
	}
	public int sampleTimeMs = 100;
	public float sampleValue = 500F;
	public string sampleData;
	public bool isLooping = false;
	public Transform[] controlPointsList;

	public void sample () {
		sampleData = "";

		//float[] predata2 = evalData(this.curve, sampleTimeMs, 1);
		
		//float[] predata3 = evalData(index, sampleTimeMs, 1);
		
		if (this.cpList[this.index].Length < 2 || sampleTimeMs < 2) return;
		float[] predata = TMath.Poly.multiplyScalarReverse(evalData(this.cpList[this.index], sampleTimeMs, 1), sampleValue);
		sampleData = predata.FloatArrayToString();
	}
	float[] evalData(float[] array, int duration, float precision, bool catmull = false, bool display = false) {
		return !catmull ? _evalData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] evalData(AnimationCurve curve, int duration, float precision, bool catmull = false, bool display = false) {
		float[] array = CurveToArray(curve);
        return !catmull ? _evalData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] evalData(int index, int duration, float precision, bool catmull = false, bool display = false) {
		float[] array = cpList[index];
		return !catmull ? _evalData(array, duration, precision, display) : DataCatmullRomSpline(ArrayTo2dArray(array), duration, display);
	}
	float[] _evalData (float[] array, int duration, float precision, bool display) {
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

		CSPL.getNaturalKs(ts, ps, ks);

        int minT = (int)CPEval[0][0];
        int maxT = (int)CPEval[cplen - 1][0];

		if (maxT < 1) return new float[0];
        float[] data = new float[maxT + 1];
        for (int i = minT; i <= maxT; i++) {
            data[i] = CSPL.evalSpline(i, ts, ps, ks);// Scale numbers
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

			float frac = 1F/time;

			int resLoops = Mathf.FloorToInt(1F / frac);
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
		
		//Draw the Catmull-Rom spline between the points
		for (int i = 0; i < controlPointsList.Length; i++)
		{
			//Cant draw between the endpoints
			//Neither do we need to draw from the second to the last endpoint
			//...if we are not making a looping line
			if (( i == controlPointsList.Length - 1) && !isLooping)
			{
				continue;
			}
			Gizmos.color = Color.green;
			DisplayCatmullRomSpline(i, (1F / controlPointsList.Length) * this.sampleTimeMs);
			
			//int[] timeData = DataCatmullRomSpline(i);
		}
		Gizmos.color = Color.yellow;
		DisplaySpline();
	}

	void DisplaySpline()
	{
		int cpLength = controlPointsList.Count();
		float[] xPos = new float[cpLength * 2];
		float[] yPos = new float[cpLength * 2];
		float[] zPos = new float[cpLength * 2];
		for (int i = 0, x = 0, y = 0, z = 0; i < cpLength; i++) {
			// time / value
			xPos[x++] = (float)i/cpLength; xPos[x++] = controlPointsList[i].position.x;
			yPos[y++] = (float)i/cpLength; yPos[y++] = controlPointsList[i].position.y;
			zPos[z++] = (float)i/cpLength; zPos[z++] = controlPointsList[i].position.z;
		}
		if (cpLength < 2) return;
		float[] xData = _evalData(xPos, this.sampleTimeMs, 1, false);
		float[] yData = _evalData(yPos, this.sampleTimeMs, 1, false);
		float[] zData = _evalData(zPos, this.sampleTimeMs, 1, false);
		Vector3 p = new Vector3();
		Vector3 _deltap = new Vector3(xData[0], yData[0], zData[0]);
		for (int i = 1; i < xData.Length; i++) {
			p.x = xData[i];
			p.y = yData[i];
			p.z = zData[i];
			//Gizmos.DrawCube(_deltap, size);
			Gizmos.DrawLine(_deltap, p);
			_deltap = p;
		}
	}

	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	Vector3 size = new Vector3(0.1F, 0.1F, 0.1F);
	void DisplayCatmullRomSpline(int pos, float timeF)
	{
		int time = (int)timeF;
		//The 4 points we need to form a spline between p1 and p2
		int cpLength = controlPointsList.Count();
		if (cpLength < 2) return; 
		Vector3 p0 = controlPointsList[ClampListPos(pos - 1, cpLength)].position;
		Vector3 p1 = controlPointsList[pos].position;
		Vector3 p2 = controlPointsList[ClampListPos(pos + 1, cpLength)].position;
		Vector3 p3 = controlPointsList[ClampListPos(pos + 2, cpLength)].position;

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		//float resolution = 0.2F;
		float frac = 1F/time;

		//How many times should we loop?
		//By distance make segments *
		int resLoops = Mathf.FloorToInt(1F / frac);

		for (int i = 1; i <= resLoops; i++)
		{
			//Which t position are we at?
			float t = i * frac;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

			//Gizmos.DrawCube(newPos, size);

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
		Vector3 a = 2F * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2F * p0 - 5F * p1 + 4F * p2 - p3;
		Vector3 d = -p0 + 3F * p1 - 3F * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5F * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

	float GetCatmullRomPosition(float t, float p0, float p1, float p2, float p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		float a = 2F * p1;
		float b = p2 - p0;
		float c = 2F * p0 - 5F * p1 + 4F * p2 - p3;
		float d = -p0 + 3F * p1 - 3F * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		float pos = 0.5F * (a + (b * t) + (c * t * t) + (d * t * t * t));

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
		new float[]{0F, 1F, 1/3F, 2/3F, 2/3F, 1/3F, 1F, 0F},
        new float[]{0F, 1F, 0.17F, 0.96F, 1F, 0F},
        new float[]{0F, 1F, 0.83F, 0.03F, 1F, 0F},
        new float[]{0F, 1F, 0.19F, 0.92F, 0.81F, 0.08F, 1F, 0F},
        new float[]{0F, 1F, 0.32F, 0.89F, 0.75F, 0.44F, 1F, 0F},
        new float[]{0F, 1F, 0.17F, 0.69F, 0.74F, 0.06F, 1F, 0F},
        new float[]{0F, 1F, 0.17F, 0.93F, 0.35F, 0.75F, 0.55F, 0.4F, 0.8F, 0.07F, 1F, 0F},
        new float[]{0F, 1F, 0.17F, 0.99F, 0.42F, 0.92F, 0.66F, 0.71F, 0.86F, 0.35F, 1F, 0F},
        new float[]{0F, 1F, 0.15F, 0.59F, 0.33F, 0.3F, 0.57F, 0.08F, 0.83F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.22F, 0.96F, 0.38F, 0.77F, 0.5F, 0.5F, 0.64F, 0.18F, 0.82F, 0.02F, 1F, 0F},
        new float[]{0F, 1F, 0.21F, 1F, 0.42F, 0.96F, 0.63F, 0.84F, 0.82F, 0.54F, 0.92F, 0.28F, 1F, 0F},
        new float[]{0F, 1F, 0.12F, 0.58F, 0.27F, 0.28F, 0.45F, 0.09F, 0.66F, 0.01F, 0.81F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.15F, 1F, 0.28F, 0.94F, 0.41F, 0.76F, 0.5F, 0.49F, 0.57F, 0.26F, 0.68F, 0.08F, 0.84F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.24F, 1F, 0.43F, 0.98F, 0.64F, 0.89F, 0.81F, 0.64F, 0.93F, 0.28F, 1F, 0F},
        new float[]{0F, 1F, 0.09F, 0.61F, 0.23F, 0.26F, 0.37F, 0.1F, 0.59F, 0.01F, 0.78F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.16F, 0.99F, 0.28F, 0.96F, 0.45F, 0.7F, 0.52F, 0.39F, 0.6F, 0.17F, 0.75F, 0.01F, 0.88F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.09F, 0.99F, 0.56F, 0.94F, 0.72F, 0.85F, 0.83F, 0.69F, 0.92F, 0.42F, 1F, 0F},
        new float[]{0F, 1F, 0.05F, 0.7F, 0.22F, 0.21F, 0.48F, 0.03F, 0.64F, 0.01F, 0.93F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.11F, 0.99F, 0.21F, 0.99F, 0.3F, 0.96F, 0.38F, 0.9F, 0.47F, 0.67F, 0.53F, 0.33F, 0.6F, 0.12F, 0.72F, 0.02F, 0.79F, 0F, 0.85F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.33F, 0.94F, 0.54F, 0.83F, 0.81F, 0.58F, 0.94F, 0.35F, 0.99F, 0.1F, 1F, 0F},
        new float[]{0F, 1F, 0.02F, 0.9F, 0.06F, 0.66F, 0.14F, 0.48F, 0.26F, 0.32F, 0.41F, 0.19F, 0.6F, 0.08F, 0.79F, 0.02F, 1F, 0F},
        new float[]{0F, 1F, 0.17F, 0.97F, 0.27F, 0.92F, 0.37F, 0.83F, 0.44F, 0.74F, 0.47F, 0.67F, 0.49F, 0.6F, 0.5F, 0.5F, 0.51F, 0.41F, 0.53F, 0.31F, 0.57F, 0.22F, 0.62F, 0.17F, 0.71F, 0.09F, 0.82F, 0.03F, 1F, 0F},
        new float[]{0F, 1F, 0.04F, 1F, 0.64F, 1F, 0.86F, 0.53F, 1F, 0F},
        new float[]{0F, 1F, 0.03F, 0.86F, 0.35F, 0.01F, 0.95F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.03F, 1F, 0.46F, 0.69F, 0.51F, 0.44F, 0.56F, 0.22F, 0.97F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.08F, 0.99F, 0.15F, 0.99F, 0.22F, 1F, 0.32F, 1F, 0.41F, 0.98F, 0.47F, 0.99F, 0.63F, 0.99F, 0.74F, 0.9F, 0.93F, 0.93F, 0.95F, 0.6F, 1F, 0F},
        new float[]{0F, 1F, 0.03F, 0.65F, 0.07F, 0.02F, 0.26F, 0.11F, 0.38F, 0F, 0.56F, 0.01F, 0.69F, 0F, 0.75F, 0F, 0.83F, 0F, 0.9F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.13F, 1F, 0.23F, 0.99F, 0.33F, 0.99F, 0.45F, 0.95F, 0.48F, 0.67F, 0.51F, 0.41F, 0.55F, 0.04F, 0.67F, 0F, 0.78F, 0F, 0.87F, 0F, 1F, 0F},
        new float[]{0F, 1F, 0.04F, 0.98F, 0.08F, 1F, 0.12F, 0.96F, 0.23F, 0.97F, 0.26F, 1F, 0.28F, 0.96F, 0.6F, 0.95F, 0.63F, 1F, 0.68F, 0.8F, 0.87F, 0.13F, 1F, 0F},
        new float[]{0F, 1F, 0.15F, 0.83F, 0.28F, 0.4F, 0.33F, 0.17F, 0.36F, 0.02F, 0.38F, 0.04F, 0.71F, 0.04F, 0.73F, 0F, 0.77F, 0.04F, 0.88F, 0.03F, 0.91F, 0F, 0.94F, 0.01F, 1F, 0F},
        new float[]{0F, 1F, 0.03F, 0.98F, 0.05F, 0.99F, 0.09F, 0.97F, 0.13F, 0.99F, 0.16F, 0.94F, 0.28F, 0.94F, 0.31F, 0.98F, 0.37F, 0.77F, 0.44F, 0.55F, 0.55F, 0.46F, 0.64F, 0.19F, 0.68F, 0.01F, 0.72F, 0.06F, 0.82F, 0.07F, 0.86F, 0F, 0.91F, 0.03F, 0.95F, 0F, 0.97F, 0.01F, 1F, 0F},
		new float[]{0F, 1F, 0.2F, 0.8F, 0.4F, 0.6F, 0.6F, 0.4F, 0.8F, 0.2F, 1F, 0F},
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