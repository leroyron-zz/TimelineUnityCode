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
        public delegate void DelegateUtilizeValues(float value, Core.Bind.Property setBindProperty, int node, int property);
        public delegate void DelegateRevertCallRevertPos(int revertPos);
        public struct Process
        {
            private string _option;
            private string _method;
            public DelegateProcessCallBackCount InvokeCall;
            public DelegateUtilizeValues UtilizeReadData;
            public DelegateUtilizeValues UtilizeThrustData;
            public DelegateUtilizeValues UtilizeMeasureData;
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

            UpdateOptions();
        }

        public void Update()
        {
            continuance = arguments.continuance;
            skip = arguments.skip;
            rCount = arguments.rCount;
            tCount = arguments.tCount;
            revert = arguments.revert;
            mCount = arguments.mCount;
            leap = arguments.leap;
            reset = arguments.reset;
            UpdateCallbacks();
            DevertCallbacks();

            UpdateOptions();
        }

        public void UpdateOptions() {
            if (defaults.runtime == null || defaults.runtime != currentRuntime)
            {
                defaults.runtime = currentRuntime = defaults.runtime != null ? defaults.runtime : currentRuntime;

                switch (defaults.runtime)
                {
                    case "forward":
                        defaults.RuntimeCallbacks = ForwardRuntimeCallbacks;
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
                        defaults.RuntimeCallbacks = ForwardRuntimeCallbacks;
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
                                process.InvokeCall = ThrustAll;
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
                                process.InvokeCall = ThrustAll;
                                break;
                        }
                        break;
                    // ToDo - for threading
                    case "measure":
                        switch (process.method)
                        {
                            case "all":
                                process.InvokeCall = MeasureAll;
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
                                process.InvokeCall = MeasureAll;
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
        /*private void MeasureAll(int count)
        {
            throw new NotImplementedException();
        }*/
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
        public void AddUpdateCallback(object variableBoxed, Func<int, int> Func)
        {
            updateCalls[updateCallCount] = (int count) => { TimelineCode.Log(variableBoxed); return Func(count); };
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

        public void AddDevertCallback(object variableBoxed, Func<object, int, int> Func)
        {
            devertCalls[devertCallCount] = (int count) => { TimelineCode.Log(variableBoxed); return Func(variableBoxed, count); };
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
        private void ForwardRuntimeCallbacks(int count)
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
        public void AddRevertCallback(int key, Func<int, int, int> Func)
        {
            revertCalls[revertCallCount] = (int register, int count) => { return Func(register, count); };
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
            //IDictionary<int, object> setBind = binding.ids[nodeBsIK];
            //Core.TLType param = setBind[0] as Core.TLType;
            //Core.TLType.Exec setNode = (Core.TLType.Exec)((Core.TLType)setBind[0]).timeline;
            Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
             //TO-DO Finish
            //int setLeapNext = setBind.node[stream][setBindProperty.binding].leapNext;
            int setLeapNext = setBindProperty.leapNext ?? 0;
            //int setLeapList = setBind.node[stream][setBindProperty.binding].leap;
            Core.Bind.Leap[] setLeapList = setBindProperty.leap;
            
            // TO-DO Finish
            leapPos = setBindProperty.leapNext;
            setLeapList = setBindProperty.leap;
            if (setLeapList == null) { return; }
            int setLeapLength = setLeapList.Length;
            for (int l = 0; l < setLeapLength; l++)
            {
                if (setLeapList[l] != null)
                {
                    setBindProperty.leapNext = l;
                    break;
                }
                else
                {

                }
            }
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
        int? leapPos;
        int setLeapNext, setLeapList, setLeapBind, leapPosI;
        void CallOutLeap(int nextPos)
        {
            IDictionary<int, object> setBind = binding.ids[nodeBsIK];
            //Core.TLType param = setBind[0] as Core.TLType;
            Core.TLType.Exec setNode = (Core.TLType.Exec)((Core.TLType)setBind[0]).timeline;
            Core.Bind.Property setBindProperty = (Core.Bind.Property)setBind[propBsIK];
             //TO-DO Finish
            //int setLeapNext = setBind.node[stream][setBindProperty.binding].leapNext;
            int? setLeapNext = setBindProperty.leapNext;
            //int setLeapList = setBind.node[stream][setBindProperty.binding].leap;
            Core.Bind.Leap[] setLeapList = setBindProperty.leap;

            if (setLeapList[leapPos ?? 0] == null) { return; }
            Core.Bind.Leap setLeapBind = setLeapList[setLeapNext ?? 0];
            int leapPosI = setLeapBind.dataPosI;
            // if (!setLeapBind) { return }
            data[leapPosI] = !setLeapBind.dispose ? arguments.leap : setLeapBind.zeroIn != leap ? setLeapBind.zeroIn : data[leapPosI + 1];// b.Zero out data
            setLeapBind.CallBack/*.apply*/(setBindProperty);
            if (setLeapBind.dispose)
            {
                setBindProperty.leapNext = null;
                setLeapList[leapPos ?? 0] = null;
                //delete setLeapList[leapPos];
            }

            int setLeapLength = setLeapList.Length;
            for (int l = (leapPos ?? 0) + 1; l < setLeapLength; l++)
            {
                if (setLeapList[l] == null)
                {
                    setBindProperty.leapNext = l;
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
                Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
                leapPos = setBindProperty.leapNext;

                if (nodeBsIK == -1)
                {
                    sI = endPosI;
                    UpdateDataPos();
                }
                else if (dataPos + count > propDataLength - 1)
                {
                    if (dataPos + count >= leapPos)
                    {
                        CallOutLeap(dataPos + count);
                        // data[nextPosI] = data[nextPosI + 1] // b.Zero out data
                        // get data from previous
                    }
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
                    //IDictionary<int, object> setBind = binding.ids[nodeBsIK];
                    //Core.TLType param = setBind[0] as Core.TLType;
                    //Core.TLType.Exec setNode = (Core.TLType.Exec)((Core.TLType)setBind[0]).timeline;
                    //Core.Bind.Property setBindProperty = (Core.Bind.Property)setBind[propBsIK];
                    //Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
                    //if (setBindProperty.property != null && setBindProperty.property == "x") 
                    //param.x /*[setBindProperty.property][setBindProperty.binding]*/ = setBindProperty.value; 
                    //else param.value/*[setBindProperty.binding]*/ = setBindProperty.value;
                    // ToDO - fix
                    //leapPos = setNode.x.leapNext; //else leapPos = setBind.node[stream][setBindProperty.binding].leapNext : setBind.node[stream][setBindProperty.property].leapNext;
                    //leapPos = setBind.node[stream][setBindProperty.binding] ? setBind.node[stream][setBindProperty.binding].leapNext : setBind.node[stream][setBindProperty.property].leapNext;
                    /**/
                    //leapPos = setBindProperty.leapNext;
                    process.UtilizeReadData(DataVal(count), setBindProperty, nodeBsIK, propBsIK);
                    //process.UtilizeReadData(DataVal(count), nodeBsIK, propBsIK);
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

        // //Thrusting stores dataoffsets and zeros out data
        void ThrustAll(int count) {
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
                    // let node_i = sI/nodeDataLength;//node index number
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
                        data[dataPosI] = continuancePosValData0;
                    }

                    sI = CheckInContinuance();
                    // sI = CheckOutRevert()
                }

                Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
                leapPos = setBindProperty.leapNext;
                if (nodeBsIK == -1) {
                    sI = endPosI;
                    UpdateDataPos();
                } else if (dataPos + count > propDataLength - 1) {
                    if (dataPos + count >= leapPos)
                    {
                        CallOutLeap(dataPos + count);
                        // data[nextPosI] = data[nextPosI + 1] // b.Zero out data
                        // get data from previous
                    }
                    if (!revert) {

                    } else {
                        if (nodeBsIK != 101)
                            CheckOutRevert(count);
                        else
                            CheckOutRevertCallback(count);
                    }
                    ResetLeap();
                } else if (count > 0) {
                    // Data Level//
                    process.UtilizeThrustData(_dataSum(count), setBindProperty, nodeBsIK, propBsIK);
                } else {
                    return;
                }
            }
        }
        float _dataSum(int count) {
            float sums = 0;
            int next = count;
            int nextPos = dataPos + next;
            int nextPosI = dataPosI + nextPos;
            if (nextPos >= leapPos) {
                if (continuance) {
                    data[dataPosI] = nextPos;// a.store offset
                }
                this.CallOutLeap(nextPos);
                // data[nextPosI] = data[nextPosI + 1]; // b.Zero out data
                // get data from previous
            }
            for (int d = 0; d < count; d++) {
                next = d + 1;
                nextPos = dataPos + next;
                nextPosI = dataPosI + nextPos;
                sI = nextPosI;
                sums += data[sI];
                // data[sI] = 0;// b...
            }
            data[dataPosI] = nextPos;// a...
            sI = endPosI;
            this.UpdateDataPos();
            return sums;
        }
        // //thrust

        // //Measuring gathers data for use
        int mI = 0;
        float[] measureData;
        // ToDo - for threading
        void MeasureAll(int count) {
            measureData = new float[count * propsPerNode + propsPerNode + 1 * nodesPerStream];
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
                    // let node_i = sI/nodeDataLength;//node index number
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
                    measureData[mI++] = propBsIK;
                    sI++;
                    cursor++;
                    reads++;

                    dataPos = ((sI - cursor) - chunkStartI);
                    dataPosI = sI - dataPos;
                    int endPos = (data0PropDataLength - dataPos) - 1;
                    endPosI = dataPosI + endPos;

                    if (reset) {
                        data[dataPosI] = continuancePosValData0;
                    }

                    sI = this.CheckInContinuance();
                    // sI = this.CheckOutRevert();
                }

                Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
                leapPos = setBindProperty.leapNext;
                if (nodeBsIK == -1) {
                    sI = endPosI;
                    this.UpdateDataPos();
                } else if (dataPos + count > propDataLength - 1) {
                    if (!revert) {

                    } else {
                        if (nodeBsIK != 101)
                            this.CheckOutRevert(count);
                        else
                            this.CheckOutRevertCallback(count);
                    }
                    this.ResetLeap();
                } else if (count > 0) {
                    // Data Level//
                    // process.UtilizeMeasureData(this.DataReturn(count), setBindProperty, nodeBsIK, propBsIK);
                } else {
                    return;
                }
            }

            //return measureData;
        }
        void DataReturn(int count) {
            int next = count;
            int nextPos = dataPos + next;
            int nextPosI = dataPosI + nextPos;
            if (nextPos >= leapPos) {
                if (continuance) {
                    data[dataPosI] = nextPos;// a.store offset
                }
                this.CallOutLeap(nextPos);
                // data[nextPosI] = data[nextPosI + 1] // b.Zero out data
                // get data from previous
            }
            for (int d = 0; d < count; d++) {
                next = d + 1;
                nextPos = dataPos + next;
                nextPosI = dataPosI + nextPos;
                sI = nextPosI;
                measureData[mI++] = data[sI];
            }
            data[dataPosI] = nextPos;// a...
            sI = endPosI;
            this.UpdateDataPos();
        }
        // //measure

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
                            data[dataPosI] = continuancePosValData0;
                        }

                        sI = this.CheckInContinuance();

                        syncIS = 0;
                        //IDictionary<int, object> setBind = binding.ids[nodeBsIK];
                        //Core.TLType setNode = (Core.TLType)setBind[0];
                        //Core.Bind.Property setBindProperty = (Core.Bind.Property)setBind[propBsIK];
                        Core.Bind.Property setBindProperty = (Core.Bind.Property)binding.ids[nodeBsIK][propBsIK];
                        // To-Do fix / shift
                        int shift = setBindProperty._shift;
                        shift = shift > 0 ? shift : this.CheckOutRevert(propDataLength + shift);
                        syncIS = syncI + shift;
                        if (syncIS > propDataLength) {
                            syncIS = this.Reversion(syncIS);
                        }
                    }

                    data[dataPosI] = syncIS;// a.
                    sI = endPosI;
                    this.UpdateDataPos();
                }
            }
    }
}