using System;
using TLExtensions;
public partial class Timeline
{
    public partial class GUI
    {
        public Timeframe timeframe = new Timeframe();

        private delegate void DelegateGUIUpdate();

        public partial class Timeframe
        {
            Timeline _timeline;
            
            public void Init(Timeline timeline)
            {
                this._timeline = timeline;
                TimelineCode.Log("Init Dialog");
            }

            void OnUpdate(object controller) {
                //controller.updateShelf();
            }

            private DelegateGUIUpdate Update;
            void OnChange(object controller/*, object change*/) {
                Update();
            }
            void OnRuntime(object controller) {
                //controller.updateShelf();
            }
            
            
            /*gui.controls.Streaming.__folders.access.addFolder("timeframe").open()

            var _running = gui.controls.Streaming.__folders.access.__folders.timeframe.Add(that, "running")
            gui.addon.bind.onchange(_running, _onchange)
            gui.addon.bind.onupdate(_running, that, _onupdate)

            var _lapse = gui.controls.Streaming.__folders.access.__folders.timeframe.Add(that, "lapse")
            gui.addon.bind.onchange(_lapse, _onchange)

            gui.controls.Streaming.__folders.access.__folders.timeframe.addFolder("runtime")
            var _read = gui.controls.Streaming.__folders.access.__folders.timeframe.__folders.runtime.Add(that, "read")
            gui.addon.bind.onRuntime(_read, that, _onruntime)

            var _thrust = gui.controls.Streaming.__folders.access.__folders.timeframe.__folders.runtime.Add(that, "thrust")
            gui.addon.bind.onRuntime(_thrust, that, _onruntime)*/
        }

        public static class Bind
        {
            public delegate void DelegateCall();
            public delegate void DelegateCallInValue(int value);
            public abstract class Invokes {
                public Func<int>[] Calls = new Func<int>[10];
                public int Length;
                public DelegateCall CallBacks;
                public void DefaultCall() {
                    for (var c = 0; c < Length; c++) {
                        Calls[c]();
                    }
                }
                public Invokes() {
                    CallBacks = DefaultCall;
                }
            }
            public class Init : Invokes {
                
            }
            public class Ready : Invokes {

            }
            public class Update : Invokes {

            }
            public class Runtime : Invokes {

            }
            public class Revert : Invokes {
                public Func<int, int>[] Calls = new Func<int, int>[10];
                public DelegateCallInValue CallBacks;
                public void ValueCall(int count) {
                    for (var c = 0; c < Length; c++) {
                        Calls[c](count);
                    }
                }
                public Revert() {
                    CallBacks = ValueCall;
                }
            }
            public class Pass : Invokes {

            }
            public delegate void DelegateOnChange(Func<object, int> func);
            public class TLController {
                public DelegateOnChange OnChange;
            }
            public static void OnChange(TLController controller, Func<object, int> Func)
            {
                controller.OnChange((object val) =>
                {
                    // ToDo - !!do val check!!
                    Func(controller);

                    return 0;
                });
            }
            public static void OnInit(TLUIElement controller, object _init, Func<object, int> Func)
            {
                _init = _init.GetMember("oninit", new Init(), false);
                if (_init == null) return;
                Init init = _init as Init;
                init.Calls[init.Length] = () => { return Func(controller); };
                init.Length++;
            }
            public static void OnReady(TLUIElement controller, object _ready, Func<object, int> Func)
            {
                _ready = _ready.GetMember("onready", new Ready(), false);
                if (_ready == null) return;
                Ready ready = _ready as Ready;
                ready.Calls[ready.Length] = () => { return Func(controller); };
                ready.Length++;
            }
            public static void OnUpdate(TLUIElement controller, object _update, Func<object, int> Func)
            {
                _update = _update.GetMember("onupdate", new Update(), false);
                if (_update == null) return;
                Update update = _update as Update;
                update.Calls[update.Length] = () => { return Func(controller); };
                update.Length++;
            }
            public static void OnRuntime(TLUIElement controller, object _runtime, Func<TLUIElement, int> Func)
            {
                _runtime = _runtime.GetMember("onruntime", new Runtime(), false);
                if (_runtime == null) return;
                Runtime runtime = _runtime as Runtime;
                runtime.Calls[runtime.Length] = () => { return Func(controller); };
                runtime.Length++;
            }
            public static void OnRevert(TLUIElement controller, object _revert, Func<TLUIElement, int, int> Func)
            {
                _revert = _revert.GetMember("onrevert", new Revert(), false);
                if (_revert == null) return;
                Revert revert = _revert as Revert;
                revert.Calls[revert.Length] = (int count) => { return Func(controller, count); };
                revert.Length++;
            }
            public static void OnPass(TLUIElement controller, object _pass, Func<object, int> Func)
            {
                _pass = _pass.GetMember("onpass", new Pass(), false);
                if (_pass == null) return;
                Pass pass = _pass as Pass;
                pass.Calls[pass.Length] = () => { return Func(controller); };
                pass.Length++;
            }
        }
    }
}