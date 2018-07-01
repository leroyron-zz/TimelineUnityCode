using System;
using System.Collections.Generic;

public partial class Timeline
{
    public partial class Core
    {
        public Buffer buffer = new Buffer();

        public partial class Buffer
        {
            Timeline _timeline;
            Core _code;
            Access _access;

            public void Init(Timeline timeline)
            {
                this._timeline = timeline;
                this._code = timeline.code;
                this._access = timeline.access;
                TimelineCode.Log("Init Buffer");
            }

            public void Build(Func<int> CallBack = null) {
                Msg(Scenes.timeline);
                BuildOff(Scenes.timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline timeline, Func<int> CallBack = null) {
                Msg(timeline);
                BuildOff(timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline[] timelines, Func<int> CallBack = null) {
                for (int t = 0; t < timelines.Length; t++) {
                    Msg(timelines[t]);
                    BuildOff(timelines[t]);
                }
                if (CallBack != null) CallBack();
            }
            void Msg(Timeline timeline) {
                TimelineCode.Log("("+timeline.name+")"+" Buffering Stream...");
            }
            void BuildOff(Timeline timeline = null) {
                timeline.access.Build();
            }
        }
    }
}
