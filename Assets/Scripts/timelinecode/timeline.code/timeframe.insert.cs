using System;

public partial class TIMELINE
{
    public partial class CODE
    {
        public partial class TIMEFRAME
        {
            public void runtimeAuthority(Func<int> Authority, string select, int position) {
                //_runtime[select].script[position] = Authority;
            }
            public void clearRuntimeAuthority(string select,  int at) {
                /*if (isNaN(at)) {
                    _runtime[select].script = [];
                } else {
                    if (_runtime[select].script[at]) _runtime[select].script.splice(at, 1);
                }*/
            }
            public void clearRuntimeAuthoritiesNear(string select,  int at,  int near) {
                /*this.timeline.removeInsertsNear(select, at, near);
                int from = at - near;
                int to = at + near;
                for (int ni = from; ni < to; ni++) {
                    this.clearRuntimeAuthority(select, ni);
                }*/
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
                int revert(int register, int count) 
                {
                    TIMELINE.Log("Reverted - ToDo: Check return error?");
                    if (_timeframe.control) { return -1; }
                    return this.checkNext(register);
                }
                public void init(TIMELINE timeline, Func<int>[] insert)
                {
                    this._access = timeline._access;
                    this._binding = timeline.binding;
                    this._timeframe = timeline.timeframe;
                    this.function = insert;
                    this._access.addRuntimeCallback(runtime);
                    this._access.addRevertCallback(0, revert);
                }
            }
        }
    }
}

