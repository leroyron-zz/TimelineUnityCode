using System;
using System.Collections.Generic;

public partial class Timeline
{
    public partial class Access
    {
        private float[] data;
        public void Build(Func<int> CallBack = null)
        {
            try
            {
                if (binding.proxy == null) {
                    binding.proxy = binding.Agent();
                }
                Finalize(CallBack);
            }
            catch (System.IO.IOException e)
            {
                // handle(e)
                TimelineCode.Log("Streaming: No bindings - ref: http:// (" + e + ")");
            }
            finally
            {
                TimelineCode.Log("Streaming - ");
                // Debug.Log("Like what you see? We\'er looking for developers! https://github.com/leroyron/TimelineUnityCode");
            }
        }
        private bool continuance;
        private int skip;
        private int rCount;
        private int tCount;
        private bool revert;
        private int mCount;
        private int leap;
        private bool reset;

        // Non-Consequential
        public struct ARGUMENTS
        {
            public bool continuance;
            public int skip;
            public int rCount;
            public int tCount;
            public bool revert;
            public int mCount;
            public int leap;
            public bool reset;
        }
        public ARGUMENTS arguments;
        public delegate void DelegateRuntimeCallBackCount(int count);
        public struct Defaults
        {
            private string _timeframe;
            private bool _accessed;
            private string _runtime;
            public bool relative { get { return _timeframe == "thrust"; } }
            public string timeframe { get { return _timeframe; } set { _timeframe = value; } }
            public bool accessed { get { return _accessed; } }
            public DelegateRuntimeCallBackCount RuntimeCallbacks;
            public string runtime
            {
                get { return _runtime; }
                set
                {
                    _runtime = value == "forward" || value == "backward" || value == "instant" || value == "direction" ? value : "forward";
                }
            }
        }
        public Defaults defaults;
        private string currentRuntime = "forward";

        public delegate void DelegateProcessCallBackCount(int count);
        public delegate void DelegateUtilizeValues(float value, int node, int property);
        public delegate void DelegateRevertCallRevertPos(int revertPos);
        public struct Process
        {
            private string _option;
            private string _method;
            public DelegateProcessCallBackCount InvokeCall;
            public DelegateUtilizeValues UtilizeReadData;
            public DelegateUtilizeValues UtilizeThrustData;
            public DelegateUtilizeValues utilizeMeasureData;
            public DelegateRevertCallRevertPos OutputRevertCall;
            public string option
            {
                get { return _option; }
                set
                {
                    _option = value == "read" || value == "thrust" || value == "measure" ? value : "read";
                }
            }
            public string method
            {
                get { return _method; }
                set
                {
                    _method = value == "all" || value == "nodes" || value == "properties" || value == "mix" ? value : "all";
                }
            }
        }
        public Process process;
        private string currentOption = "read";
        private string currentMethod = "all";

        public int readCount = 0;
        public int thrustCount = 0;
        public int measureCount = 0;
        public void Update(bool setcontinuance, int setskip, int setrCount, int settCount, bool setrevert, int setmCount, int setleap, bool setreset)
        {
            continuance = arguments.continuance = setcontinuance;
            skip = arguments.skip = setskip;
            rCount = arguments.rCount = setrCount;
            tCount = arguments.tCount = settCount;
            revert = arguments.revert = setrevert;
            mCount = arguments.mCount = setmCount;
            leap = arguments.leap = setleap;
            reset = arguments.reset = setreset;
            UpdateCallbacks();
            DevertCallbacks();

            if (defaults.runtime == null || defaults.runtime != currentRuntime)
            {
                defaults.runtime = currentRuntime = defaults.runtime != null ? defaults.runtime : currentRuntime;

                switch (defaults.runtime)
                {
                    case "forward":
                        defaults.RuntimeCallbacks = forwardRuntimeCallbacks;
                        break;
                    case "backward":
                        //defaults.RuntimeCallbacks = optimizeRuntimeCallbacks;
                        break;
                    case "instant":
                        defaults.RuntimeCallbacks = instantRuntimeCallbacks;
                        break;
                    case "direction":
                        //defaults.RuntimeCallbacks = optimizeRuntimeCallbacks;
                        break;
                    default:
                        defaults.RuntimeCallbacks = forwardRuntimeCallbacks;
                        break;
                }
            }

            if (process.option == null || process.method == null || process.option != currentOption || process.method != currentMethod)
            {
                process.option = currentOption = process.option != null ? process.option : currentOption;
                process.method = currentMethod = process.method != null ? process.method : currentMethod;
                switch (process.option)
                {
                    case "read":
                        switch (process.method)
                        {
                            case "all":
                                process.InvokeCall = ReadAll;
                                break;
                            case "nodes":
                                process.InvokeCall = _readNodes;
                                break;
                            case "properties":
                                process.InvokeCall = _readProperties;
                                break;
                            case "mix":
                                process.InvokeCall = _readMix;
                                break;
                            default:
                                process.InvokeCall = ReadAll;
                                break;
                        }
                        break;
                    case "thrust":
                        switch (process.method)
                        {
                            case "all":
                                process.InvokeCall = _thrustAll;
                                break;
                            case "nodes":
                                process.InvokeCall = _thrustNodes;
                                break;
                            case "properties":
                                process.InvokeCall = _thrustProperties;
                                break;
                            case "mix":
                                process.InvokeCall = _thrustMix;
                                break;
                            default:
                                process.InvokeCall = _thrustAll;
                                break;
                        }
                        break;
                    // ToDo - for threading
                    case "measure":
                        switch (process.method)
                        {
                            case "all":
                                process.InvokeCall = _measureAll;
                                break;
                            case "nodes":
                                process.InvokeCall = _measureNodes;
                                break;
                            case "properties":
                                process.InvokeCall = _measureProperties;
                                break;
                            case "mix":
                                process.InvokeCall = _measureMix;
                                break;
                            default:
                                process.InvokeCall = _measureAll;
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void _measureMix(int count)
        {
            throw new NotImplementedException();
        }

        private void _measureProperties(int count)
        {
            throw new NotImplementedException();
        }

        private void _measureNodes(int count)
        {
            throw new NotImplementedException();
        }

        // ToDo - for threading
        private void _measureAll(int count)
        {
            throw new NotImplementedException();
        }

        private void _thrustMix(int count)
        {
            throw new NotImplementedException();
        }

        private void _thrustProperties(int count)
        {
            throw new NotImplementedException();
        }

        private void _thrustNodes(int count)
        {
            throw new NotImplementedException();
        }

        private void _thrustAll(int count)
        {
            throw new NotImplementedException();
        }

        private void _readMix(int count)
        {
            throw new NotImplementedException();
        }

        private void _readProperties(int count)
        {
            throw new NotImplementedException();
        }

        private void _readNodes(int count)
        {
            throw new NotImplementedException();
        }

        // CALLBACK Methods
        // update calls change (GUI, ect...)// not using !
        public void AddUpdateCallback(object variableBoxed, Func<int, int> func)
        {
            updateCalls[updateCallCount] = (int count) => { TimelineCode.Log(variableBoxed); return func(count); };
            updateCallCount++;
        }
        public Func<int, int>[] updateCalls = new Func<int, int>[10];
        private int updateCallCount = 0;
        private void UpdateCallbacks()
        {
            for (int u = 0; u < updateCallCount; u++)
            {
                updateCalls[u](UnityEngine.Random.Range(0, 100));
            }
        }

        public void AddDevertCallback(object variableBoxed, Func<object, int, int> func)
        {
            devertCalls[devertCallCount] = (int count) => { TimelineCode.Log(variableBoxed); return func(variableBoxed, count); };
            devertCallCount++;
        }
        public Func<int, int>[] devertCalls = new Func<int, int>[10];
        private int devertCallCount = 0;
        private void DevertCallbacks()
        {
            for (int u = 0; u < devertCallCount; u++)
            {
                devertCalls[u](UnityEngine.Random.Range(0, 100));
            }
        }
        //

        // runtime calls change (Timeframe, GUI, ect...)
        public void AddRuntimeCallback(Func<int, int, int, int> Func)
        {
            runtimeCalls[runtimeCallCount] = (int register, int count, int duration) => { return Func(register, count, duration); };
            runtimeCallCount++;
        }
        public Func<int, int, int, int>[] runtimeCalls = new Func<int, int, int, int>[10];
        private int runtimeCallCount = 0;
        private int nextRuntimeCallback = 0;
        public bool block = false;
        private void instantRuntimeCallbacks(int count)
        {
            int next = propDataLength;
            int check = 0;
            for (int r = 0; r < runtimeCallCount; r++)
            {
                // register, count, duration
                check = runtimeCalls[r](nextRuntimeCallback, count, 0);
                if (check < next) next = check;
            }
            nextRuntimeCallback = next;
            if (block)
            {
                readCount = thrustCount = measureCount = 0;
                block = false;
            }
        }
        private void forwardRuntimeCallbacks(int count)
        {
            // CurrentDataPos(2) get current data position continuance
            if (count + data[2] <= nextRuntimeCallback || count + data[2] > propDataLength) return;
            int next = propDataLength;
            int check = 0;
            for (int r = 0; r < runtimeCallCount; r++)
            {
                // register, count, duration
                check = runtimeCalls[r](nextRuntimeCallback, count, (int)data[2]);
                if (check < next) next = check;
            }
            nextRuntimeCallback = next;
            if (block)
            {
                readCount = thrustCount = measureCount = 0;
                block = false;
            }
        }
        //

        // revert calls change (Timeframe, GUI, ect...)
        public void AddRevertCallback(int key, Func<int, int, int> func)
        {
            revertCalls[revertCallCount] = (int register, int count) => { return func(register, count); };
            revertCallCount++;
        }
        public Func<int, int, int>[] revertCalls = new Func<int, int, int>[10];
        private int revertCallCount = 0;
        private void RevertCallbacks(int register, int count)
        {
            int next = propDataLength;
            int check = 0;
            for (int r = 0; r < revertCallCount; r++)
            {
                check = revertCalls[r](register, count);
                if (check < next) next = check;
            }
            nextRuntimeCallback = next;
        }
        //

        //// Timeline Runtime vars
        private int nodesPerStream, propsPerNode, propDataLength, continuancePosValData0, data0PropDataLength, nodeDataLength, streamDataLength;
        //TODO choose either dictionary of int array?
        private Dictionary<int, int> propsPerNodeList;
        // public Bind proxy;

        private void Finalize(Func<int> CallBack = null)
        {
            // Initialization from build
            nodesPerStream = binding.nodesPerStream;
            propsPerNode = binding.propsPerNode;
            //TODO choose either dictionary of int array?
            propsPerNodeList = binding.propsPerNodeList;
            propDataLength = binding.propDataLength;
            continuancePosValData0 = binding.continuancePosValData0 != null ? binding.continuancePosValData0 : 1;
            data0PropDataLength = continuancePosValData0 + propDataLength;
            nodeDataLength = data0PropDataLength * propsPerNode + propsPerNode + 1;
            // proxy = bindings.proxy;

            data = binding.data;

            //nodesPerStream = propsPerNode = /* propDataLength = continuancePosValData0 = */data0PropDataLength = // nodeDataLength = null
            //delete nodesPerStream;
            //delete propsPerNode;
            //delete propsPerNodeList;
            // delete propDataLength
            // delete continuancePosValData0
            //delete data0PropDataLength;
            // delete nodeDataLength

            // FOR PRODUCTION
            //! !!IMPORTANT hide data (private)... replace data with var data;-->
            // var data = data;
            // delete data;

            streamDataLength = data.Length;
            UpdateCallbacks();
            DevertCallbacks();
            if (CallBack != null) CallBack();
        }

        public void RevertFromTo(int from, int to)
        {
            propDataLength = propDataLength = from;
            continuancePosValData0 = continuancePosValData0 = to + 1;
        }

        public int Reversion(int dataPos)
        {
            return dataPos - (propDataLength * (dataPos / propDataLength << 0)) + continuancePosValData0;
        }

        public void ResetLeap()
        {
            IDictionary<int, object> setBind = binding.ids[nodeBsIK];
            Core.TLType setNode = (Core.TLType)setBind[0];
            Core.Bind setBindProperty = (Core.Bind)setBind[propBsIK];
            /* TO-DO Finish
            leapPos = setBind.node[stream][setBindProperty.binding].leapNext;
            setLeapList = setBind.node[stream][setBindProperty.binding].leap;
            if (!setLeapList) { return; }
            int setLeapLength = setLeapList.length;
                for (int l = 0; l < setLeapLength; l++)
            {
                if (setLeapList[l])
                {
                    setBind.node[stream][setBindProperty.binding].leapNext = l;
                    break;
                    }
                else
                {

                }
            }
            */
        }

        // //Common Vars and _functions that are used for thrust and measuring
        // "*I" indicates data index
        // "sI" indicates the stream index position
        // "*BsIK" indicates the bind index key in the stream array
        // cursor counts up start for offset reads
        // reads counts up for data reads
        // Procedure when edit programming stream:
        // When changes are being done, consider that syncing maybe triggered to adjust offset and continuance values for the data sets. // _syncOffsets
        int sI = 0;
        int partition = 0;
        int cursor = 0;
        int reads = 0;
        int partFrac = 0;
        int chunkStartI = 0;
        int dataPos, dataPosI, endPosI;
        int nodeBsIK = 0;
        int propBsIK = 0;
        int CurrentDataPos()
        {
            return (int)data[2];
        }
        int CheckInContinuance()
        {
            if (!continuance)
            {
                return sI;
            }
            sI += (int)data[dataPosI];
            UpdateDataPos();
            return sI;
        }
        void UpdateDataPos(int modsI = 0)
        {
            int mI = modsI == 0 ? sI : modsI;
            dataPos = ((mI - cursor) - chunkStartI);
            dataPosI = mI - dataPos;
        }
        int CheckOutRevert(int modskip = 0)
        {
            int mS = modskip == 0 ? skip : modskip;
            if (!revert)
            {
                return sI;
            }
            data[dataPosI] = dataPos = Reversion(dataPos + mS);
            return dataPos;
        }
        int CheckOutRevertCallback(int modskip = 0)
        {
            int mS = modskip == 0 ? skip : modskip;
            if (!revert)
            {
                return sI;
            }
            data[dataPosI] = dataPos = Reversion(dataPos + mS);
            process.OutputRevertCall(continuancePosValData0);
            RevertCallbacks(continuancePosValData0, dataPos);
            return dataPos;
        }
        
        //output_revertCall = 'Data entry portal for execute, ref this to outer functions'

        // values from stream pair up and bind
        int setBind, setBindProperty;
        int leapPos, setLeapNext, setLeapList, setLeapBind, leapPosI;
        void CallOutLeap(int nextPos)
        {
            IDictionary<int, object> setBind = binding.ids[nodeBsIK];
            Core.TLType setNode = (Core.TLType)setBind[0];
            Core.Bind setBindProperty = (Core.Bind)setBind[propBsIK];
            /* TO-DO Finish
            int setLeapNext = setBind.node[stream][setBindProperty.binding].leapNext;
            int setLeapList = setBind.node[stream][setBindProperty.binding].leap;
            if (!setLeapList[leapPos]) { return; }
            int setLeapBind = setLeapList[setLeapNext];
            int leapPosI = setLeapBind.dataPosI;
            // if (!setLeapBind) { return }
            data[leapPosI] = !setLeapBind.dispose ? arguments.leap : setLeapBind.zeroIn ? setLeapBind.zeroIn : data[leapPosI + 1];// b.Zero out data
            setLeapBind.CallBack.apply(setBind.node[stream]);
            if (setLeapBind.dispose)
            {
                setBind.node[stream][setBindProperty.binding].leapNext = undefined;
                setLeapList[leapPos] = null;
                delete setLeapList[leapPos];
            }

            int setLeapLength = setLeapList.length;
            for (int l = leapPos + 1; l < setLeapLength; l++)
            {
                if (setLeapList[l])
                {
                    setBind.node[stream][setBindProperty.binding].leapNext = l;
                    if (l <= nextPos)
                    {
                        leapPos = l;
                        CallOutLeap(l);
                    }
                    // break
                }
                else
                {

                }
            }
            */
            // leapPos
        }
        // //

        // //Reading stores
        void ReadAll(int count)
        {
            readCount = count + rCount;
            defaults.RuntimeCallbacks(readCount);
            for (
                sI = 0,
                partition = 0,
                partFrac = 0,
                cursor = 0,
                reads = 0;
                sI < streamDataLength;
                sI++
                )
            {
                // Node Level//
                if ((sI - partition) % nodeDataLength == 0)
                {
                    // let node_i = sI/nodeDataLength;//node index number
                    // ->> Node Selection > buffer identifier
                    nodeBsIK = (int)data[sI];
                    if (propsPerNodeList[nodeBsIK] != 0)
                    {
                        if ((sI - partition) != partition)
                        {
                            partition = sI;
                            partFrac = sI % data0PropDataLength;
                            reads = 0;
                        }
                        propsPerNode = propsPerNodeList[nodeBsIK];
                        nodeDataLength = data0PropDataLength * propsPerNode + propsPerNode + 1;
                    }
                    sI++;
                    cursor = 1;
                    reads++;
                }

                // Property Level//
                if ((sI - reads) % data0PropDataLength == partFrac)
                {
                    chunkStartI = (sI - cursor);
                    // var prop = ((sI-reads) - ( data0PropDataLength * propsPerNode * node_i)) / data0PropDataLength;//property index number of chunk
                    // ->> Property Selection > buffer identifier

                    propBsIK = (int)data[sI];
                    sI++;
                    cursor++;
                    reads++;

                    dataPos = ((sI - cursor) - chunkStartI);
                    dataPosI = sI - dataPos;
                    int endPos = (data0PropDataLength - dataPos) - 1;
                    endPosI = dataPosI + endPos;

                    if (reset)
                    {
                        data[dataPosI] = continuancePosValData0;
                    }

                    sI = CheckInContinuance();
                    // sI = CheckOutRevert(count)
                }

                if (nodeBsIK == -1)
                {
                    sI = endPosI;
                    UpdateDataPos();
                }
                else if (dataPos + count > propDataLength - 1)
                {
                    if (!revert)
                    {

                    }
                    else
                    {
                        if (nodeBsIK != 101) CheckOutRevert(count);
                        else CheckOutRevertCallback(count);
                    }
                    ResetLeap();
                }
                else if (count > 0)
                {
                    // Data Level//
                    IDictionary<int, object> setBind = binding.ids[nodeBsIK];
                    Core.TLType setNode = (Core.TLType)setBind[0];
                    Core.Bind setBindProperty = (Core.Bind)setBind[propBsIK];
                    /* ToDO - fix
                    leapPos = setBind.node[stream][setBindProperty.binding] ? setBind.node[stream][setBindProperty.binding].leapNext : setBind.node[stream][setBindProperty.property].leapNext;
                    */
                    process.UtilizeReadData(DataVal(count), nodeBsIK, propBsIK);
                }
                else
                {
                    return;
                }
            }
        }
        float DataVal(int count)
        {
            float val = data[sI];
            int next = count;
            int nextPos = dataPos + next;
            int nextPosI = dataPosI + nextPos;
            if (nextPos >= leapPos)
            {
                if (continuance)
                {
                    data[dataPosI] = nextPos;// a.store offset
                }
                CallOutLeap(nextPos);
                // data[nextPosI] = data[nextPosI + 1] // b.Zero out data
                // get data from previous
            }
            else
            {
                sI = nextPosI;
                UpdateDataPos();
                data[dataPosI] = nextPos;// a...
                val = data[sI];
            }
            sI = endPosI;
            UpdateDataPos();
            return val;
        }
        // //read

        public void SyncOffsets(int syncI) {
                int syncIS = 0;
                for (
                    sI = 0,
                    partition = 0,
                    partFrac = 0,
                    cursor = 0,
                    reads = 0;
                    sI < streamDataLength;
                    sI++
                    ) {
                    // Node Level//
                    if ((sI - partition) % nodeDataLength == 0) {
                        // var node_i = sI/nodeDataLength;//node index number
                        // ->> Node Selection > buffer identifier
                        nodeBsIK = (int)data[sI];
                        if (propsPerNodeList[nodeBsIK] != 0) {
                            if ((sI - partition) != partition) {
                                partition = sI;
                                partFrac = sI % data0PropDataLength;
                                reads = 0;
                            }
                            propsPerNode = propsPerNodeList[nodeBsIK];
                            nodeDataLength = data0PropDataLength * propsPerNode + propsPerNode + 1;
                        }
                        sI++;
                        cursor = 1;
                        reads++;
                    }

                    // Property Level//
                    if ((sI - reads) % data0PropDataLength == partFrac) {
                        chunkStartI = (sI - cursor);
                        // var prop = ((sI-reads) - ( data0PropDataLength * propsPerNode * node_i)) / data0PropDataLength;//property index number of chunk
                        // ->> Property Selection > buffer identifier
                        propBsIK = (int)data[sI];
                        sI++;
                        cursor++;
                        reads++;

                        dataPos = ((sI - cursor) - chunkStartI);
                        dataPosI = sI - dataPos;
                        int endPos = (data0PropDataLength - dataPos) - 1;
                        endPosI = dataPosI + endPos;

                        if (reset) {
                            this.data[dataPosI] = continuancePosValData0;
                        }

                        sI = this.CheckInContinuance();

                        syncIS = 0;
                        IDictionary<int, object> setBind = binding.ids[nodeBsIK];
                        Core.TLType setNode = (Core.TLType)setBind[0];
                        Core.Bind setBindProperty = (Core.Bind)setBind[propBsIK];
                        // To-Do fix / shift
                        //int shift = setBind.node[stream][setBindProperty.binding] ? setBind.node[stream][setBindProperty.binding]._shift : setBind.node[stream][setBindProperty.property]._shift;
                        // shift = shift > 0 ? shift : this.CheckOutRevert(propDataLength + shift)
                        syncIS = syncI;// + shift;
                        if (syncIS > propDataLength) {
                            syncIS = this.Reversion(syncIS);
                        }
                    }

                    this.data[dataPosI] = syncIS;// a.
                    sI = endPosI;
                    this.UpdateDataPos();
                }
            }
    }
}