using UnityEngine;

public class controlpoints : MonoBehaviour {
	private bool _preDefined = ControlPoint._preDefined;
	public int index = ControlPointIndex.index;
	public AnimationCurve curve = new AnimationCurve();
	private float[][] cpList = ControlPoint.cpList;
	private int cpPropLength = ControlPoint.cpList[ControlPointIndex.index].Length;
	[NamedArrayAttribute (new string[]{"cp"}, 100)]
	public ControlPoint[] controlPoints = new ControlPoint[ControlPoint.cpList[ControlPointIndex.index].Length/2];

	controlpoints () {
		ControlPoint._lastTime[index] = 0;
		ControlPoint._index++;
		for (int c = 0; c < cpPropLength-1; c++) {
			bool first = c == 0;
        	bool last = c == cpPropLength - 2;
			float time = (float)(cpList[index][c] / 100);
			float value = (float)(cpList[index][++c] / 100);
			controlPoints[(int)c/2] = new ControlPoint(index, time, value, curve, first, last);
		}
	}

	[System.Serializable]
	public class ControlPoint : ControlPointIndex
	{
		[Tooltip("time of control point")]
		public float time = 0F;
		[Tooltip("value of control point")]
		public float value = 0F;
		public int index = 0;
		public float lastTime {
			get { 
				return _lastTime[this.index]; 
			} 
			set { 
				_lastTime[this.index] = value; 
			}
		}
		public bool first, last;

		public ControlPoint(int index, float time, float value, AnimationCurve curve, bool first = false, bool last = false)
		{
			this.first = first;
			this.last = last;
			this.index = index;
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
}

public class ControlPointIndex
{
	public static int _index = 0;
	public static float[] _lastTime = new float[10000];
	public static bool _preDefined { get {return _index < cpList.Length;} }
	public static int index { get { return _preDefined ? _index : cpList.Length - 1;} }
	internal static float[][] cpList = new float[32][] {
		new float[]{0, 100, 50, 50, 100, 0},
        new float[]{0, 100, 17, 96, 100, 0},
        new float[]{0, 100, 83, 3, 100, 0},
        new float[]{0, 100, 19, 92, 81, 8, 100, 0},
        new float[]{0, 100, 32, 89, 75, 44, 100, 0},
        new float[]{0, 100, 17, 69, 74, 6, 100, 0},
        new float[]{0, 100, 17, 93, 35, 75, 55, 40, 80, 7, 100, 0},
        new float[]{0, 100, 17, 99, 42, 92, 66, 71, 86, 35, 100, 0},
        new float[]{0, 100, 15, 59, 33, 30, 57, 8, 83, 0, 100, 0},
        new float[]{0, 100, 22, 96, 38, 77, 50, 50, 64, 18, 82, 2, 100, 0},
        new float[]{0, 100, 21, 100, 42, 96, 63, 84, 82, 54, 92, 28, 100, 0},
        new float[]{0, 100, 12, 58, 27, 28, 45, 9, 66, 1, 81, 0, 100, 0},
        new float[]{0, 100, 15, 100, 28, 94, 41, 76, 50, 49, 57, 26, 68, 8, 84, 0, 100, 0},
        new float[]{0, 100, 24, 100, 43, 98, 64, 89, 81, 64, 93, 28, 100, 0},
        new float[]{0, 100, 9, 61, 23, 26, 37, 10, 59, 1, 78, 0, 100, 0},
        new float[]{0, 100, 16, 99, 28, 96, 45, 70, 52, 39, 60, 17, 75, 1, 88, 0, 100, 0},
        new float[]{0, 100, 9, 99, 56, 94, 72, 85, 83, 69, 92, 42, 100, 0},
        new float[]{0, 100, 5, 70, 22, 21, 48, 3, 64, 1, 93, 0, 100, 0},
        new float[]{0, 100, 11, 99, 21, 99, 30, 96, 38, 90, 47, 67, 53, 33, 60, 12, 72, 2, 79, 0, 85, 0, 100, 0},
        new float[]{0, 100, 33, 94, 54, 83, 81, 58, 94, 35, 99, 10, 100, 0},
        new float[]{0, 100, 1, 90, 6, 66, 14, 48, 26, 32, 41, 19, 60, 8, 79, 2, 100, 0},
        new float[]{0, 100, 17, 97, 27, 92, 37, 83, 44, 74, 47, 67, 49, 60, 50, 50, 51, 41, 53, 31, 57, 22, 62, 17, 71, 9, 82, 3, 100, 0},
        new float[]{0, 100, 4, 100, 64, 100, 86, 53, 100, 0},
        new float[]{0, 100, 3, 86, 35, 1, 95, 0, 100, 0},
        new float[]{0, 100, 3, 100, 46, 69, 51, 44, 56, 22, 97, 0, 100, 0},
        new float[]{0, 100, 8, 99, 15, 99, 22, 100, 32, 100, 41, 98, 47, 99, 63, 99, 74, 90, 93, 93, 95, 60, 100, 0},
        new float[]{0, 100, 3, 65, 7, 2, 26, 11, 38, 0, 56, 1, 69, 0, 75, 0, 83, 0, 90, 0, 100, 0},
        new float[]{0, 100, 13, 100, 23, 99, 33, 99, 45, 95, 48, 67, 51, 41, 55, 4, 67, 0, 78, 0, 87, 0, 100, 0},
        new float[]{0, 100, 4, 98, 8, 100, 12, 96, 23, 97, 26, 100, 28, 96, 60, 95, 63, 100, 68, 80, 87, 13, 100, 0},
        new float[]{0, 100, 15, 83, 28, 40, 33, 17, 36, 2, 38, 4, 71, 4, 73, 0, 77, 4, 88, 3, 91, 0, 94, 1, 100, 0},
        new float[]{0, 100, 3, 98, 5, 99, 9, 97, 13, 99, 16, 94, 28, 94, 31, 98, 37, 77, 44, 55, 55, 46, 64, 19, 68, 1, 72, 6, 82, 7, 86, 0, 91, 3, 95, 0, 97, 1, 100, 0},
		new float[]{0, 100, 20, 80, 40, 60, 60, 40, 80, 20, 100, 0},
	};
}
public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
	public string[] list;
    public NamedArrayAttribute(string[] names, int length = 0) {
		if (length == 0 || names.Length == 0) { this.names = names; return; }
		this.list = new string[names.Length * length];
		for (int n = 0; n < names.Length; n++)
			for (int i = 0; i < length; i++) this.list[n*names.Length+i] = names[n] + i;
		this.names = this.list;		
	}
}