using System;

partial class TIMELINE
{
    public partial class CODE
    {
        public TIMEFRAME timeframe = new TIMEFRAME();

        public partial class TIMEFRAME
        {
            public bool _init = false;
            public bool ready = false;
            public bool running = false;
            public bool control = false;
            private TIMELINE timeline;
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                timeline.Log("Init Timeframe");
            }

            public class INSERT
            {
                ACCESS _access;
                BINDING _binding;
                TIMEFRAME _timeframe;
                Func<int>[] function;
                int runtime(int register, int count, int duration)
                {
                    if (_timeframe.control) { return 0; }
                    int end = count + duration;
                    for (int i = register; i < end; i++)
                    {
                        if (function[i] == null)
                        {
                            function[i]();
                        }
                    }
                    return checkNext(end);
                }
                int checkNext(int end)
                {
                    int next = _binding.propDataLength;
                    for (int i = end; i < function.Length; i++)
                    {
                        if (function[i] == null)
                        {
                            if (i < next || next < end) next = i;
                        }
                    }
                    return next;
                }
                public void init(TIMELINE timeline, Func<int>[] insert)
                {
                    this._access = timeline._access;
                    this._binding = timeline.binding;
                    this._timeframe = timeline.timeframe;
                    this.function = insert;
                    this._access.addRuntimeCallback(runtime);
                }
            }
        }
    }
}

