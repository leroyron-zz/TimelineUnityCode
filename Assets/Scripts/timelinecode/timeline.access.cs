using System;

public partial class TIMELINE
{
    public partial class ACCESS
    {
        private float[] data;
        public void build(Func<int> callback = null)
        {
            try
            {
                _finalize(callback);
            }
            catch (System.IO.IOException e)
            {
                // handle(e)
                code.Log("Streaming: No bindings - ref: http:// (" + e + ")");
            }
            finally
            {
                code.Log("Streaming - ");
                // Debug.Log("Like what you see? We\'er looking for developers! https://github.com/leroyron/timeline-jscode");
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
        public delegate void delegateRuntimeCallBackCount(int count);
        public struct DEFAULTS
        {
            private string _timeframe;
            private bool _accessed;
            private string _runtime;
            public bool relative { get { return _timeframe == "thrust"; } }
            public string timeframe { get { return _timeframe; } set { _timeframe = value; } }
            public bool accessed { get { return _accessed; } }
            public delegateRuntimeCallBackCount runtimeCallbacks;
            public string runtime
            {
                get { return _runtime; }
                set
                {
                    _runtime = value == "forward" || value == "backward" || value == "instant" || value == "direction" ? value : "forward";
                }
            }
        }
        public DEFAULTS defaults;
        private string currentRuntime = "forward";

        public delegate void delegateProcessCallBackCount(int count);
        public delegate void delegateUtilizeValues(float value, int node, int property);
        public struct PROCESS
        {
            private string _option;
            private string _method;
            public delegateProcessCallBackCount invokeCall;
            public delegateUtilizeValues utilizeReadData;
            public delegateUtilizeValues utilizeThurstData;
            public delegateUtilizeValues utilizeMeasureData;
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
        public PROCESS process;
        private string currentOption = "read";
        private string currentMethod = "all";

        public int readCount = 0;
        public int thrustCount = 0;
        public int measureCount = 0;
        public void update(bool setcontinuance, int setskip, int setrCount, int settCount, bool setrevert, int setmCount, int setleap, bool setreset)
        {
            continuance = arguments.continuance = setcontinuance;
            skip = arguments.skip = setskip;
            rCount = arguments.rCount = setrCount;
            tCount = arguments.tCount = settCount;
            revert = arguments.revert = setrevert;
            mCount = arguments.mCount = setmCount;
            leap = arguments.leap = setleap;
            reset = arguments.reset = setreset;
            updateCallbacks();

            if (defaults.runtime == null || defaults.runtime != currentRuntime)
            {
                defaults.runtime = currentRuntime = defaults.runtime != null ? defaults.runtime : currentRuntime;

                switch (defaults.runtime)
                {
                    case "forward":
                        defaults.runtimeCallbacks = forwardRuntimeCallbacks;
                        break;
                    case "backward":
                        //defaults.runtimeCallbacks = optimizeRuntimeCallbacks;
                        break;
                    case "instant":
                        defaults.runtimeCallbacks = instantRuntimeCallbacks;
                        break;
                    case "direction":
                        //defaults.runtimeCallbacks = optimizeRuntimeCallbacks;
                        break;
                    default:
                        defaults.runtimeCallbacks = forwardRuntimeCallbacks;
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
                                process.invokeCall = _readAll;
                                break;
                            case "nodes":
                                process.invokeCall = _readNodes;
                                break;
                            case "properties":
                                process.invokeCall = _readProperties;
                                break;
                            case "mix":
                                process.invokeCall = _readMix;
                                break;
                            default:
                                process.invokeCall = _readAll;
                                break;
                        }
                        break;
                    case "thrust":
                        switch (process.method)
                        {
                            case "all":
                                process.invokeCall = _thrustAll;
                                break;
                            case "nodes":
                                process.invokeCall = _thrustNodes;
                                break;
                            case "properties":
                                process.invokeCall = _thrustProperties;
                                break;
                            case "mix":
                                process.invokeCall = _thrustMix;
                                break;
                            default:
                                process.invokeCall = _thrustAll;
                                break;
                        }
                        break;
                    // ToDo - for threading
                    case "measure":
                        switch (process.method)
                        {
                            case "all":
                                process.invokeCall = _measureAll;
                                break;
                            case "nodes":
                                process.invokeCall = _measureNodes;
                                break;
                            case "properties":
                                process.invokeCall = _measureProperties;
                                break;
                            case "mix":
                                process.invokeCall = _measureMix;
                                break;
                            default:
                                process.invokeCall = _measureAll;
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
        public void addUpdateCallback(string variableBoxed, Func<int, int> func)
        {
            updateCalls[updateCallCount] = (int count) => { code.Log(variableBoxed); return func(count); };
            updateCallCount++;
        }
        public Func<int, int>[] updateCalls = new Func<int, int>[10];
        private int updateCallCount = 0;
        private void updateCallbacks()
        {
            for (int u = 0; u < updateCallCount; u++)
            {
                updateCalls[u](UnityEngine.Random.Range(0, 100));
            }
        }
        //

        // runtime calls change (Timeframe, GUI, ect...)
        public void addRuntimeCallback(Func<int, int, int, int> action)
        {
            runtimeCalls[runtimeCallCount] = (int register, int count, int duration) => { return action(register, count, duration); };
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
            // _currentDataPos(2) get current data position continuance
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
        public void addRevertCallback(int key, Func<int, int, int> func)
        {
            revertCalls[revertCallCount] = (int register, int count) => { return func(register, count); };
            revertCallCount++;
        }
        public Func<int, int, int>[] revertCalls = new Func<int, int, int>[10];
        private int revertCallCount = 0;
        private void revertCallbacks(int register, int count)
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
        private int[] propsPerNodeList;
        // public BIND proxy;

        private void _finalize(Func<int> callback = null)
        {
            // Initialization from build
            nodesPerStream = binding.nodesPerStream;
            propsPerNode = binding.propsPerNode;
            //TODO choose either dictionary of int array?
            //propsPerNodeList = binding.propsPerNodeList;
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

            //streamDataLength = data.Length;
            updateCallbacks();
            if (callback != null) callback();
        }

        public void revertFromTo(int from, int to)
        {
            propDataLength = propDataLength = from;
            continuancePosValData0 = continuancePosValData0 = to + 1;
        }

        public int _reversion(int dataPos)
        {
            return dataPos - (propDataLength * (dataPos / propDataLength << 0)) + continuancePosValData0;
        }

        public void _resetLeap()
        {
            /* TO-DO Finish
            setBind = bindings.ids['_bi' + nodeBsIK];
            setBindProperty = setBind[propBsIK];
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
        int _currentDataPos()
        {
            return (int)data[2];
        }
        int _checkInContinuance()
        {
            if (!continuance)
            {
                return sI;
            }
            sI += (int)data[dataPosI];
            _updateDataPos();
            return sI;
        }
        void _updateDataPos(int modsI = 0)
        {
            int mI = modsI == 0 ? sI : modsI;
            dataPos = ((mI - cursor) - chunkStartI);
            dataPosI = mI - dataPos;
        }
        int _checkOutRevert(int modskip = 0)
        {
            int mS = modskip == 0 ? skip : modskip;
            if (!revert)
            {
                return sI;
            }
            data[dataPosI] = dataPos = _reversion(dataPos + mS);
            return dataPos;
        }
        int _checkOutRevertCallback(int modskip = 0)
        {
            int mS = modskip == 0 ? skip : modskip;
            if (!revert)
            {
                return sI;
            }
            data[dataPosI] = dataPos = _reversion(dataPos + mS);
            outputRevertCall(continuancePosValData0);
            revertCallbacks(continuancePosValData0, dataPos);
            return dataPos;
        }
        public delegate void delegateRevertCallRevertPos(int revertPos);
        public delegateRevertCallRevertPos outputRevertCall;
        //output_revertCall = 'Data entry portal for execute, ref this to outer functions'

        // values from stream pair up and bind
        int setBind, setBindProperty;
        int leapPos, setLeapNext, setLeapList, setLeapBind, leapPosI;
        void _callOutLeap(int nextPos)
        {
            // let setBind = bindings.ids['_bi' + nodeBsIK]
            // let setBindProperty = setBind[propBsIK]
            /* TO-DO Finish
            int setLeapNext = setBind.node[stream][setBindProperty.binding].leapNext;
            int setLeapList = setBind.node[stream][setBindProperty.binding].leap;
            if (!setLeapList[leapPos]) { return; }
            int setLeapBind = setLeapList[setLeapNext];
            int leapPosI = setLeapBind.dataPosI;
            // if (!setLeapBind) { return }
            data[leapPosI] = !setLeapBind.dispose ? arguments.leap : setLeapBind.zeroIn ? setLeapBind.zeroIn : data[leapPosI + 1];// b.Zero out data
            setLeapBind.callback.apply(setBind.node[stream]);
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
                        _callOutLeap(l);
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
        void _readAll(int count)
        {
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

                    sI = _checkInContinuance();
                    // sI = _checkOutRevert(count)
                }

                if (nodeBsIK == -1)
                {
                    sI = endPosI;
                    _updateDataPos();
                }
                else if (dataPos + count > propDataLength - 1)
                {
                    if (!revert)
                    {

                    }
                    else
                    {
                        if (nodeBsIK != 101) _checkOutRevert(count);
                        else _checkOutRevertCallback(count);
                    }
                    _resetLeap();
                }
                else if (count > 0)
                {
                    // Data Level//
                    /* ToDO - fix
                    setBind = bindings.ids['_bi' + nodeBsIK];
                    setBindProperty = setBind[propBsIK];
                    leapPos = setBind.node[stream][setBindProperty.binding] ? setBind.node[stream][setBindProperty.binding].leapNext : setBind.node[stream][setBindProperty.property].leapNext;
                    output_utilizeReadValues(_dataVal(count), nodeBsIK, propBsIK);
                    */
                }
                else
                {
                    return;
                }
            }
        }
        float _dataVal(int count)
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
                _callOutLeap(nextPos);
                // data[nextPosI] = data[nextPosI + 1] // b.Zero out data
                // get data from previous
            }
            else
            {
                sI = nextPosI;
                _updateDataPos();
                data[dataPosI] = nextPos;// a...
                val = data[sI];
            }
            sI = endPosI;
            _updateDataPos();
            return val;
        }
        // //read
    }
}