using System;
using TLExtensions;

public partial class Timeline
{
    public void Take (/*obj, instructions*/)
    {

    }
    public partial class Core
    {
        public partial class ExecParams {
            public TLType parameter;

            public int binding;
            public int data0PosI;
            public int lastOffset;
            public float lastVal;
            //public TLType node;

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