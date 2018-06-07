using System;
using System.Collections.Generic;

public partial class TIMELINE
{
    public partial class CODE
    {
        public BUFFER buffer = new BUFFER();

        public partial class BUFFER
        {
            TIMELINE timeline;
            CODE code;
            ACCESS _access;

            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                this.code = timeline.code;
                this._access = timeline._access;
                TIMELINE.Log("Init Buffer");
            }

            public void build (Func<int> callback = null) {
                msg(SCENES.timeline);
                _build(SCENES.timeline);
                if (callback != null) callback();
            }
            public void build (TIMELINE timeline, Func<int> callback = null) {
                msg(timeline);
                _build(timeline);
                if (callback != null) callback();
            }
            public void build (TIMELINE[] timelines, Func<int> callback = null) {
                for (int t = 0; t < timelines.Length; t++) {
                    msg(timelines[t]);
                    _build(timelines[t]);
                }
                if (callback != null) callback();
            }
            void msg (TIMELINE timeline) {
                code.Log("("+timeline.name+")"+" Buffering Stream...");
            }
            void _build (TIMELINE timeline = null) {
                timeline._access.build();
            }
        }
    }
}
