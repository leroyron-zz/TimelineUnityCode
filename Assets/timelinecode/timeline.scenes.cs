using System;

partial class TIMELINE
{
    public SCENES scenes = new SCENES();

    public partial class SCENES
    {
        // timeline properties changed by scene selection
        private static TIMELINE timeline;
        public void init(TIMELINE timeline)
        {
            SCENES.timeline = timeline;
            TIMELINE.Log("Scene Started with buffer");
        }

        public class SCENE
        {
            public CODE.TIMEFRAME.ACTION insertAction = new CODE.TIMEFRAME.ACTION();
            public CODE.TIMEFRAME.COMMENT insertComment = new CODE.TIMEFRAME.COMMENT();
            public CODE.TIMEFRAME.SEGMENT insertSegment = new CODE.TIMEFRAME.SEGMENT();
            public CODE.TIMEFRAME.SOUND insertSound = new CODE.TIMEFRAME.SOUND();
            public Func<int>[] action;
            public Func<int>[] comment;
            public Func<int>[] segment;
            public Func<int>[] sound;
            public Func<int> loadActions;
            public Func<int> loadComments;
            public Func<int> loadSegments;
            public Func<int> loadSounds;
            public TIMELINE timeline;
            public int length;
            public SCENE(int length)
            {
                this.length = length;
                this.action = new Func<int>[length];
                this.comment = new Func<int>[length];
                this.segment = new Func<int>[length];
                this.sound = new Func<int>[length];
                // base SCENES
                //base.init(length);
            }

            private void unInit ()
            {
                // remove classes and inserts
            }

            // start clear data and start scene
            public void init()
            {
                this.timeline = SCENES.timeline;
                this.timeline.length = this.length;
                this.initInserts();
            }
            public void init(int length)
            {
                this.timeline = SCENES.timeline;
                this.length = length;
                this.timeline.length = length;
                this.initInserts();
            }
            public void initInserts ()
            {
                this.loadActions();
                this.loadComments();
                this.loadSegments();
                this.loadSounds();
                this.insertAction.init(this.timeline, action);
                this.insertComment.init(this.timeline, comment);
                this.insertSegment.init(this.timeline, segment);
                this.insertSound.init(this.timeline, sound);
            }
        }
    }
}



