using System;

public partial class Timeline
{
    public Scenes scenes = new Scenes();

    public partial class Scenes
    {
        // timeline properties changed by scene selection
        public static Timeline timeline;
        public void Init(Timeline timeline)
        {
            Scenes.timeline = timeline;
            TimelineCode.Log("Scene Starting...");
        }

        public abstract class Scene
        {
            public Core.Timeframe.Action insertAction = new Core.Timeframe.Action();
            public Core.Timeframe.Comment insertComment = new Core.Timeframe.Comment();
            public Core.Timeframe.Segment insertSegment = new Core.Timeframe.Segment();
            public Core.Timeframe.Sound insertSound = new Core.Timeframe.Sound();
            public Func<int>[] actions;
            public Func<int>[] comments;
            public Func<int>[] segments;
            public Func<int>[] sounds;
            public Func<int> LoadActions;
            public Func<int> LoadComments;
            public Func<int> LoadSegments;
            public Func<int> LoadSounds;

            public abstract int Actions();
            public abstract int Comments();
            public abstract int Segments();
            public abstract int Sounds();
            public Timeline _timeline;
            public Timeline[] _timelines;
            public int length;
            public Scene(int length = 0)
            {
                this.length = length <= 0 ? Scenes.timeline.length : length;
                // base Scenes
                //base.Init(length);
            }
            
            private void UnInit()
            {
                // remove classes and inserts
            }
            public void Init()
            {
                Initialize();
            }
            public void Init(Timeline timeline = null, int length = 0)
            {
                Initialize(new Timeline[] {timeline}, length <= 0 ? this.length : length);
            }
            public void Init(int length = 0, Timeline timeline = null)
            {
                Initialize(new Timeline[] {timeline}, length <= 0 ? this.length : length);
            }
            public void Init(Timeline[] timelines = null, int length = 0)
            {
                Initialize(timelines, length <= 0 ? this.length : length);
            }
            public void Init(int length = 0, Timeline[] timelines = null)
            {
                Initialize(timelines, length <= 0 ? this.length : length);
            }

            public abstract void Start(Timeline[] timelines);
            // start clear data and start scene
            private void Initialize(Timeline[] timelines = null, int length = 0)
            {
                this.length = length <= 0 ? this.length : length;
                this.actions = new Func<int>[length];
                this.comments = new Func<int>[length];
                this.segments = new Func<int>[length];
                this.sounds = new Func<int>[length];
                this.LoadActions = Actions;
                this.LoadComments = Comments;
                this.LoadSegments = Segments;
                this.LoadSounds = Sounds;
                this._timelines = timelines != null ? timelines : new Timeline[] {Scenes.timeline};
                this._timeline = this._timelines[0];
                Scenes.timeline = this._timelines[0];
                for (int t = 0; t < this._timelines.Length; t++) {
                    this._timelines[t].length = length;
                }
                this.InitInserts();
            }
            public void InitInserts()
            {
                this.LoadActions();
                this.LoadComments();
                this.LoadSegments();
                this.LoadSounds();
                this.insertAction.Init(this._timelines[0], actions);
                this.insertComment.Init(this._timelines[0], comments);
                this.insertSegment.Init(this._timelines[0], segments);
                this.insertSound.Init(this._timelines[0], sounds);
            }
        }
    }
}



