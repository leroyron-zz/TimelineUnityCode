using System;
using TLExtensions;
public partial class TIMELINE
{
    public partial class GUI
    {
        public TIMEFRAME timeframe = new TIMEFRAME();

        private delegate void delegateGUIUpdate();

        public partial class TIMEFRAME
        {
            private TIMELINE timeline;
            
            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                TIMELINE.Log("Init Dialog");
            }

            void _onupdate(object controller) {
                //controller.updateShelf();
            }

            private delegateGUIUpdate update;
            void _onchange(object controller/*, object change*/) {
                update();
            }
            void _onruntime(object controller) {
                //controller.updateShelf();
            }
            
            
            /*gui.controls.Streaming.__folders.access.addFolder("timeframe").open()

            var _running = gui.controls.Streaming.__folders.access.__folders.timeframe.add(that, "running")
            gui.addon.bind.onchange(_running, _onchange)
            gui.addon.bind.onupdate(_running, that, _onupdate)

            var _lapse = gui.controls.Streaming.__folders.access.__folders.timeframe.add(that, "lapse")
            gui.addon.bind.onchange(_lapse, _onchange)

            gui.controls.Streaming.__folders.access.__folders.timeframe.addFolder("runtime")
            var _read = gui.controls.Streaming.__folders.access.__folders.timeframe.__folders.runtime.add(that, "read")
            gui.addon.bind.onruntime(_read, that, _onruntime)

            var _thrust = gui.controls.Streaming.__folders.access.__folders.timeframe.__folders.runtime.add(that, "thrust")
            gui.addon.bind.onruntime(_thrust, that, _onruntime)*/
        }

        public static class BIND
        {
            public delegate void delegateCall();
            public delegate void delegateCallInValue(int value);
            public abstract class CALLS {
                public Func<int>[] Calls = new Func<int>[10];
                public int Length;

                public delegateCall CallBacks;
                public void defaultCall () {
                    for (var c = 0; c < Length; c++) {
                        Calls[c]();
                    }
                }
                public CALLS () {
                    CallBacks = defaultCall;
                }
            }
            public class INIT : CALLS {
                
            }
            public class READY : CALLS {

            }
            public class UPDATE : CALLS {

            }
            public class RUNTIME : CALLS {

            }
            public class REVERT : CALLS {
                public Func<int, int>[] Calls = new Func<int, int>[10];
                public delegateCallInValue CallBacks;
                public void valueCall (int count) {
                    for (var c = 0; c < Length; c++) {
                        Calls[c](count);
                    }
                }
                public REVERT () {
                    CallBacks = valueCall;
                }
            }
            public class PASS : CALLS {

            }
            public delegate void delegateOnChange(Func<object, int> func);
            public class CONTROLLER {
                public delegateOnChange onChange;
            }
            public static void onchange(CONTROLLER controller, Func<object, int> func)
            {
                controller.onChange((object val) =>
                {
                    // ToDo - !!do val check!!
                    func(controller);

                    return 0;
                });
            }
            public static void oninit(object controller, object _init, Func<object, int> action)
            {
                _init = _init.GetMember("oninit", new INIT(), false);
                if (_init == null) return;
                INIT init = _init as INIT;
                init.Calls[init.Length] = () => { return action(controller); };
                init.Length++;
            }
            public static void onready(object controller, object _ready, Func<object, int> action)
            {
                _ready = _ready.GetMember("onready", new READY(), false);
                if (_ready == null) return;
                READY ready = _ready as READY;
                ready.Calls[ready.Length] = () => { return action(controller); };
                ready.Length++;
            }
            public static void onupdate(object controller, object _update, Func<object, int> action)
            {
                _update = _update.GetMember("onupdate", new UPDATE(), false);
                if (_update == null) return;
                UPDATE update = _update as UPDATE;
                update.Calls[update.Length] = () => { return action(controller); };
                update.Length++;
            }
            public static void onruntime(ELEMENT controller, object _runtime, Func<ELEMENT, int> action)
            {
                _runtime = _runtime.GetMember("onruntime", new RUNTIME(), false);
                if (_runtime == null) return;
                RUNTIME runtime = _runtime as RUNTIME;
                runtime.Calls[runtime.Length] = () => { return action(controller); };
                runtime.Length++;
            }
            public static void onrevert(ELEMENT controller, object _revert, Func<ELEMENT, int, int> action)
            {
                _revert = _revert.GetMember("onrevert", new REVERT(), false);
                if (_revert == null) return;
                REVERT revert = _revert as REVERT;
                revert.Calls[revert.Length] = (int count) => { return action(controller, count); };
                revert.Length++;
            }
            public static void onpass(object controller, object _pass, Func<object, int> action)
            {
                _pass = _pass.GetMember("onpass", new PASS(), false);
                if (_pass == null) return;
                PASS pass = _pass as PASS;
                pass.Calls[pass.Length] = () => { return action(controller); };
                pass.Length++;
            }
        }
    }
}