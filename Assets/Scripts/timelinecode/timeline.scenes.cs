using System;

public partial class TIMELINE
{
    public SCENES scenes = new SCENES();

    public partial class SCENES
    {
        // timeline properties changed by scene selection
        public static TIMELINE timeline;
        public void init(TIMELINE timeline)
        {
            SCENES.timeline = timeline;
            TIMELINE.Log("Scene Starting...");
        }

        public abstract class SCENE
        {
            public CODE.TIMEFRAME.ACTION insertAction = new CODE.TIMEFRAME.ACTION();
            public CODE.TIMEFRAME.COMMENT insertComment = new CODE.TIMEFRAME.COMMENT();
            public CODE.TIMEFRAME.SEGMENT insertSegment = new CODE.TIMEFRAME.SEGMENT();
            public CODE.TIMEFRAME.SOUND insertSound = new CODE.TIMEFRAME.SOUND();
            public Func<int>[] actions;
            public Func<int>[] comments;
            public Func<int>[] segments;
            public Func<int>[] sounds;
            public Func<int> loadActions;
            public Func<int> loadComments;
            public Func<int> loadSegments;
            public Func<int> loadSounds;

            public abstract int Actions();
            public abstract int Comments();
            public abstract int Segments();
            public abstract int Sounds();
            public TIMELINE timeline;
            public TIMELINE[] timelines;
            public int length;
            public SCENE(int length = 0)
            {
                this.length = length <= 0 ? SCENES.timeline.length : length;
                // base SCENES
                //base.init(length);
            }
            
            private void unInit ()
            {
                // remove classes and inserts
            }
            public void init()
            {
                _init();
            }
            public void init(TIMELINE timeline = null, int length = 0)
            {
                _init(new TIMELINE[] {timeline}, length <= 0 ? this.length : length);
            }
            public void init(int length = 0, TIMELINE timeline = null)
            {
                _init(new TIMELINE[] {timeline}, length <= 0 ? this.length : length);
            }
            public void init(TIMELINE[] timelines = null, int length = 0)
            {
                var type = base.GetType();
                _init(timelines, length <= 0 ? this.length : length);
            }
            public void init(int length = 0, TIMELINE[] timelines = null)
            {
                _init(timelines, length <= 0 ? this.length : length);
            }

            public void start() {

            }
            // start clear data and start scene
            private void _init(TIMELINE[] timelines = null, int length = 0)
            {
                this.length = length <= 0 ? this.length : length;
                this.actions = new Func<int>[length];
                this.comments = new Func<int>[length];
                this.segments = new Func<int>[length];
                this.sounds = new Func<int>[length];
                this.loadActions = Actions;
                this.loadComments = Comments;
                this.loadSegments = Segments;
                this.loadSounds = Sounds;
                this.timelines = timelines != null ? timelines : new TIMELINE[] {SCENES.timeline};
                this.timeline = this.timelines[0];
                SCENES.timeline = this.timelines[0];
                for (int t = 0; t < this.timelines.Length; t++) {
                    this.timelines[t].length = length;
                }
                this.initInserts();
            }
            public void initInserts ()
            {
                this.loadActions();
                this.loadComments();
                this.loadSegments();
                this.loadSounds();
                this.insertAction.init(this.timelines[0], actions);
                this.insertComment.init(this.timelines[0], comments);
                this.insertSegment.init(this.timelines[0], segments);
                this.insertSound.init(this.timelines[0], sounds);
            }
        }
    }
}



