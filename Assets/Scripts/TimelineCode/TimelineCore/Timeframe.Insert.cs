﻿using System;

public partial class Timeline
{
    public partial class Core
    {
        public void RuntimeAuthority(Func<int> Authority, string select, int position) {
            //_runtime[select].script[position] = Authority;
        }
        public void ClearRuntimeAuthority(string select,  int at) {
            /*if (isNaN(at)) {
                _runtime[select].script = [];
            } else {
                if (_runtime[select].script[at]) _runtime[select].script.splice(at, 1);
            }*/
        }
        public void ClearRuntimeAuthoritiesNear(string select,  int at,  int near) {
            /*this._timeline.removeInsertsNear(select, at, near);
            int from = at - near;
            int to = at + near;
            for (int ni = from; ni < to; ni++) {
                this.ClearRuntimeAuthority(select, ni);
            }*/
        }
        public class Insert
        {
            Timeline _timeline;
            Access _access;
            Binding _binding;
            Timeframe _timeframe;
            Func<int>[] _functions;
            public void Init(Timeline timeline, Func<int>[] inserts)
            {
                this._timeline = timeline;
                this._access = timeline.access;
                this._binding = timeline.binding;
                this._timeframe = timeline.timeframe;
                this._functions = inserts;
                this._access.AddRuntimeCallback(Runtime);
                this._access.AddRevertCallback(0, Revert);
            }
            int Runtime(int register, int count, int duration)
            {
                //if (_timeframe.control) { return 0; }
                int end = count + duration;
                for (int i = register; i < end; i++)
                {
                    if (_functions[i] != null)
                    {
                        _functions[i]();
                    }
                }
                return CheckNext(end);
            }
            int CheckNext(int end)
            {
                int next = _binding.propDataLength;
                for (int i = end; i < _functions.Length; i++)
                {
                    if (_functions[i] != null)
                    {
                        if (i < next || next < end) next = i;
                    }
                }
                return next;
            }
            int Revert(int register, int count) 
            {
                //TimelineCode.Log("Reverted - ToDo: Check return error?");
                //if (_timeframe.control) { TimelineCode.Log("Reverted - ToDo: Check return error?"); return -1; }
                return this.CheckNext(register);
            }
        }
    }
}

