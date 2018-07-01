using System;
using TLExtensions;

public partial class Timeline
{
    public Scenes scenes = new Scenes();

    public partial class Scenes
    {
        // timeline properties changed by scene selection
        public static Timeline timeline;
        public Scene current;
        public Scene Init(Timeline timeline, Scene scene)
        {
            Scenes.timeline = timeline;
            this.current = scene;
            TimelineCode.Log("Scene Starting...");
            return scene;
        }

        public abstract class Scene
        {
            public Core.Timeframe.Action actionInserts = new Core.Timeframe.Action();
            public Core.Timeframe.Comment commentInserts = new Core.Timeframe.Comment();
            public Core.Timeframe.Segment segmentInserts = new Core.Timeframe.Segment();
            public Core.Timeframe.Sound soundInserts = new Core.Timeframe.Sound();
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
            public bool _init;
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
                this.actionInserts.Init(this._timelines[0], actions);
                this.commentInserts.Init(this._timelines[0], comments);
                this.segmentInserts.Init(this._timelines[0], segments);
                this.soundInserts.Init(this._timelines[0], sounds);
                this._init = true;
            }
        }
        public void RuntimeAuthority(Func<int> Authority, string select, int at) {
            Core.Insert insert = (Core.Insert)this.current.GetMember(select+"Inserts");
            insert._functions[at] = Authority;
        }
        public void ClearRuntimeAuthority(string select,  int at = -1) {
            Core.Insert insert = (Core.Insert)this.current.GetMember(select+"Inserts");
            if (at == -1) {
                insert._functions = new Func<int>[timeline.length];
            } else {
                insert._functions[at] = null;
            }
        }
        public void ClearRuntimeAuthoritiesNear(string select,  int at,  int near) {
            timeline.gui.timeframe.control.RemoveInsertsNear(select, at, near);
            int from = at - near;
            int to = at + near;
            for (int ni = from; ni < to; ni++) {
                this.ClearRuntimeAuthority(select, ni);
            }
        }
    }
}



