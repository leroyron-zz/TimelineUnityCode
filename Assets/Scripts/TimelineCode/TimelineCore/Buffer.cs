using System;
using System.Collections.Generic;
using TLExtensions;
using TLMath;

public partial class Timeline
{
    public partial class Core
    {
        public Buffer buffer = new Buffer();

        public partial class Buffer
        {
            Timeline _timeline;
            Core _code;
            Binding _binding;
            Access _access;
            Interpolation _interpolation;

            private int buffOffset;
            private int deltaSetOffset;
            private float valueOffset;

            public void Init(Timeline timeline)
            {
                this._timeline = timeline;
                this._code = timeline.code;
                this._binding = timeline.code.binding;
                this._access = timeline.access;
                this._interpolation = timeline.buffer.interpolation;
                TimelineCode.Log("Init Buffer");
            }

            float[][] InterpolationDataPreCache(object[] options)
            {
                // Data Collections for interpolations store
                // To-Do make expiries for data
                // Have queue run initial data runs -->
                float[][] interpolationData = new float[0][];
                for (int e = 0; e < options.Length; e++)
                {
                    if (
                        options[e].GetType() == typeof(string)
                        && Enum.IsDefined(typeof(Interpolation.controlPoints.Names), options[e])
                        && options[e + 1].GetType() == typeof(int)
                    )
                    {
                        interpolationData = interpolationData.Concat(_interpolation.EvalData((string)options[e], (int)options[e + 1]));
                    }
                    else if (
                      options[e].GetType() == typeof(int)
                      && (int)options[e] < Interpolation.controlPoints.cpList.Length
                      && options[e + 1].GetType() == typeof(int)
                  )
                    {
                        interpolationData = interpolationData.Concat(_interpolation.EvalData((int)options[e], (int)options[e + 1]));
                    }
                }
                return interpolationData;
            }
            public void Eval(Timeline timeline, object[] options, bool relative = false, bool get = false, Func<TLType.Exec?, int> leapCallback = null, bool reassign = false, bool dispose = true, float zeroIn = 0, bool skipLeap = true)
            {
                Timeline[] timelines = new Timeline[1] { timeline };
                _Eval(timelines, options, relative, get, leapCallback, reassign, dispose, zeroIn, skipLeap);
            }
            public void Eval(Timeline[] timelines, object[] options, bool relative = false, bool get = false, Func<TLType.Exec?,int> leapCallback = null, bool reassign = false, bool dispose = true, float zeroIn = 0, bool skipLeap = true)
            {
                _Eval(timelines, options, relative, get, leapCallback, reassign, dispose, zeroIn, skipLeap);
            }

            void _Eval(Timeline[] timelines, object[] options, bool relative, bool get, Func<TLType.Exec?, int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap)
            {
                TLType[] nodes = new TLType[0];
                object[][] props = new object[0][];
                string[] propSet = new string[0];
                //float[][] dataCollection = InterpolationDataPreCache(options); // move to queue
                object[][] sets = new object[0][];
                float[][] dataSets = new float[0][];
                int blend = 0;

                /*int interLen = timelines == null ? 1 : timelines.Length;
                for (int t = 0; t < interLen; t++) {
                    Timeline timeline = timelines == null ? null : timelines[t];*/

                for (int o = 0; o < options.Length; o++)
                {
                    if (options[o] is TLType)
                    {
                        nodes = nodes.Concat(options[o++] as TLType);
                    }
                    if (CheckIndexListTypes(options, o, new System.Type[] { typeof(string), typeof(char) }) && CheckIndexType(options, o + 1, typeof(float)))
                    {
                        string propName = options[o++].ToString();
                        propName = propName == "v" ? "value"
                        : propName == "r" ? "rotation"
                        : propName == "a" ? "alpha"
                        : propName == "s" ? "scale" :
                        propName == "x" || propName == "y" || propName == "z" || propName == "w" || propName == "u" || propName == "v" ? propName : null;
                        float propValue = (float)options[o++];
                        props = props.Concat(new object[] { propName, propValue, 0 });
                        propSet = propSet.Concat(propName);
                    }
                    if (CheckIndexType(options, o, typeof(string)) && CheckIndexType(options, o + 1, typeof(int)))
                    {
                        string cpName = (string)options[o++];
                        int cpDuration = (int)options[o++];
                        sets = sets.Concat(new object[] { cpName, cpDuration });
                        dataSets = dataSets.Concat(_interpolation.EvalData(cpName, cpDuration));
                    }
                    if (CheckIndexType(options, o, typeof(int)))
                    {
                        blend = (int)options[o++];
                    }

                    if (nodes.Length > 0 && props.Length > 0 && sets.Length > 0)
                    {
                        o--;
                        for (int n = 0; n < nodes.Length; n++)
                        {
                            TLType node = nodes[n];

                            buffOffset = 0;
                            if (blend > 0)
                            {
                                if (blend == 1)
                                { // true or 1
                                    buffOffset -= deltaSetOffset;
                                }
                                else if (blend > 1 || blend < 1)
                                {
                                    buffOffset += blend;
                                }
                            }
                            valueOffset = 0;
                            for (int p = 0; p < props.Length; p++)
                            {
                                object[] prop = props[p];

                                int sumDurations = 0;
                                for (int s = 0; s < sets.Length; s++)
                                {
                                    sumDurations += (int)sets[s][1];
                                }
                                for (int s = 0; s < sets.Length; s++)
                                {
                                    object[] getset = sets[s];
                                    deltaSetOffset = (int)getset[1];
                                    float value = (float)prop[1] * (deltaSetOffset / sumDurations);
                                    //value = TMath.Type.ConvertToType(node.timeline.conversion, value);
                                    prop[2] = value;
                                    Buff(timelines, node, prop, dataSets[s], relative, skipLeap);
                                    buffOffset += deltaSetOffset;
                                    valueOffset += value;
                                }
                            }
                            if (leapCallback != null)
                            {
                                AssignLeap(timelines, nodes, propSet, false, buffOffset, leapCallback, reassign, dispose, zeroIn);
                            }

                            if (get)
                            {
                                //getData[n] = GetData(timelines, [node], propSet, get);
                            }

                        }
                        propSet = new string[0];
                        props = new object[0][];
                        sets = new object[0][];
                        dataSets = new float[0][];
                    }
                }
                //that.evals += nlen * slen;
                //that.Update();
                //return getData;
            }

            /*public float[] Exec(Timeline[] timelines, object[] options, bool relative, bool get, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap = true) {
                
                relative = relative || _access.defaults.relative;
                var getData = [];
                for(int e = 0; e < options.Length; e++) {
                    object[] option = options[e];
                    TLType[] nodes = option[0];
                    object[] chains = option[1];
                    object propSet = [];

                    for(int n = 0, nlen = nodes.Length; n<nlen; n++) {
                        TLType node = nodes[n];
                        buffOffset = 0;
                        valueOffset = 0;

                        for(int ci = 0; ci < chains.Length; ci++) {
                            let chain = chains[ci];
                            let blend = chain[0];
                            let prop = [propSet[ci] = chain[1], chain[2]];
                            let getset = [chain[4], chain[5]];
                            valueOffset += TMath.Type.ConvertToType(node[stream].conversion, chain[3]) || 0;

                            buffOffset = 0;
                            if (blend) {
                                if (blend == 1) { // true or 1
                                    buffOffset -= deltaSetOffset;
                                } else if (blend > 1 || blend< 1) {
                                    buffOffset += blend;
                                }
                            }

                            deltaSetOffset = getset[1];
                            prop[2] = TMath.Type.ConvertToType(node[stream].conversion, prop[1]);
                            Buff(timeline, node, prop, EvalData(getset[0], deltaSetOffset, deltaSetOffset), relative, skipLeap, true);
                            buffOffset += deltaSetOffset;
                        }
                        // per-node
                        if (leapCallback != null) {
                            that.AssignLeap(timelines[0], [node], [propSet], false, buffOffset, leapCallback, reassign, dispose, zeroIn);
                        }

                        if (get) {
                            getData[n] = that.GetData(timelines, [node], [propSet], get);
                        }
                    }
                    that.evals += chains.Length;
                }
                that.Update();
                return getData;
            }

            /*public void ExecLerp(Timeline[] timelines, TLType[] nodeSet, string[] propSet, TLType refnode, string refprop, float startVal, float flux, bool parallel, bool reach, int from, int to, string ease, bool exact, int at, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap = true) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength + 1;
                var refNodeBindProp = refnode[stream][refprop];
                var refData0PosI = refNodeBindProp.data0PosI;

                var valDuration = to - from;
                ease = ease || "linear";
                var easeData = EvalData(ease, valDuration, valDuration);
                if (nodeSet && propSet) {
                    for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                        let nodeBind = nodeSet[n][stream];

                        for(int p = 0, plen = propSet.Length; p<plen; p++) {
                            let nodeBindProp = nodeBind[propSet[p]];

                            let data0PosI = nodeBindProp.data0PosI;
                            let lerpVal = access.data[data0PosI + from + 1];
                            for(int ew = 0; ew<valDuration; ew++) {
                                let dataPos = from + ew;
                                dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                            let refDataPosI = refData0PosI + dataPos;
                            let dataPosI = data0PosI + dataPos;
                                if (skipLeap) if (access.data[dataPosI + 1] == access.arguments.leap) continue;
                                // console.log(1 - ((easeData[1] - easeData[0][ew]) / easeData[1]))
                                let fluxin = TMath.LerpSubject(lerpVal, access.data[refDataPosI + 1], flux * (1 - ((easeData[1] - easeData[0][ew]) / easeData[1])));
                                // if (fluxin < 0.001) continue
                                // fluxin *= easeData[ew]
        lerpVal -= fluxin;
                                access.data[dataPosI + 1] = lerpVal;
                            }
                        }
                    }
                }
                if (leapCallback != null) that.AssignLeap(timelines[0], [nodeSet[0]], propSet, exact, at, leapCallback, reassign, dispose, zeroIn);
                that.evals += nodeSet.Length* propSet.Length;
                that.Update();
            }*/


            public void AssignLeap(Timeline[] timelines, TLType[] nodeSet, string[] propSet, bool exact, int at, Func<TLType.Exec?, int> func, bool reassign, bool dispose = true, float zeroIn = 0, bool waitForRevert = false)
            {
                // Check if timeline is thrusting
                for (int t = 0; t < timelines.Length; t++)
                {
                    Timeline timeline = timelines[t];
                    string state = _binding.state;
                    Binding access = state == "prebuff"
                    ? timeline.code.binding
                    : _binding;

                    var propDataLength = access.propDataLength + 1;

                    for (int n = 0; n < nodeSet.Length; n++)
                    {
                        TLType.Exec nodeBind = nodeSet[n].timeline;

                        for (int p = 0, plen = propSet.Length; p < plen; p++)
                        {
                            string prop = propSet[p];

                            ExecParams nodeBindProp = (ExecParams)nodeBind.GetMember(prop);
                            int data0PosI = nodeBindProp.data0PosI;

                            int byteOffset = (int)access.data[data0PosI];

                            int dataPos = exact ? at : byteOffset + at;
                            dataPos = dataPos < propDataLength ? dataPos : _code.Reversion(dataPos);
                            int dataPosI = data0PosI + dataPos;
                            access.data[dataPosI] = timeline.access.arguments.leap;

                            int leapDataPosIDelta;
                            if (nodeBindProp.leap.Length > 0)
                            {
                                if (reassign)
                                {
                                    leapDataPosIDelta = nodeBindProp.leap[dataPos].dataPosI;
                                    access.data[leapDataPosIDelta] = 0;
                                }
                            }
                            else
                            {
                                // To-Do optimize
                                nodeBindProp.leap = new ExecParams.Leap[timeline.length];
                            }

                            nodeBindProp.leap[dataPos] = new ExecParams.Leap(func, dispose, zeroIn, dataPosI);

                            if (!waitForRevert && (nodeBindProp.leapNext == -1 || (dataPos < nodeBindProp.leapNext && dataPos > byteOffset))) nodeBindProp.leapNext = dataPos;
                        }
                    }
                }
            }

            void BindType(TLType node)
            {
                if (node is TLElement)
                {
                    node = node as TLElement;
                    TLElement.Exec nodeBind = ((TLElement)node).timeline;
                }
                else if (node is TLVector3)
                {
                    node = node as TLVector3;
                    TLVector3.Exec nodeBind = ((TLVector3)node).timeline;
                }
                else if (node is TLVector2)
                {
                    node = node as TLVector2;
                    TLVector2.Exec nodeBind = ((TLVector2)node).timeline;
                }
                else if (node is TLPoly)
                {
                    node = node as TLPoly;
                    ExecParams[] nodeBind = ((TLPoly)node).timeline;
                }
            }
            public void Buff(Timeline[] timelines, TLType node, object[] prop, float[] evalData, bool relative, bool skipLeap = true, bool ahead = false)
            {
                //TLType.Exec nodeBind = ((TLType)node).timeline;

                for (int t = 0; t < timelines.Length; t++)
                {
                    Timeline timeline = timelines[t];
                    relative = relative || timeline.access.defaults.relative;
                    string propName = (string)prop[0];
                    object nodeProp = node.GetMember(propName);
                    TLElement nodePropElement = node.GetMember(propName) as TLElement;
                    float nodeVal = nodeProp is TLElement ?
                        propName == "value" ? (float)nodePropElement.value : (float)nodePropElement.value
                    : propName == "value" ? (float)nodeProp : (float)nodeProp;

                    if (ahead) nodeVal = valueOffset; else nodeVal += valueOffset;
                    //TLVector3.Exec nodeBind = ((TLVector3)node).timeline;
                    ExecParams nodeBindProp = (ExecParams)((TLVector3)node).timeline.GetMember(propName);
                    int data0PosI = nodeBindProp.data0PosI;

                    string state = _binding.state;
                    Binding access = state == "prebuff"
                    ? timeline.code.binding
                    : _binding;

                    // byteOffset grabbed from the stream
                    int byteOffset = (int)access.data[data0PosI];
                    byteOffset += buffOffset;

                    var propDataLength = access.propDataLength + 1;

                    float valProp = (float)prop[2];

                    //float[] data = evalData[0];
                    //debugger;
                    float precision = evalData.Length;
                    // Check if it fills the stream beyond its current position
                    int length = evalData.Length;
                    evalData = evalData.Concat(0f);
                    for (int ew = 0; ew < length; ew++)
                    {
                        int dataPos = byteOffset + ew;
                        dataPos = dataPos < propDataLength ? dataPos : _code.Reversion(dataPos);
                        int dataPosI = data0PosI + dataPos;

                        if (skipLeap) if (access.data[dataPosI] == timeline.access.arguments.leap) continue;
                        if (relative)
                        {
                            access.data[dataPosI] += (evalData[ew] - evalData[ew + 1]) * valProp;
                        }
                        else
                        {
                            access.data[dataPosI] = nodeVal + (1 - evalData[ew]) * valProp;
                        }
                    }

                    /*that.Update();*/
                }
            }

            /*public void ZeroOut(Timeline[] timelines, int from, int to, TLType[] nodeSet, string[] propSet, bool skipLeap = true) {
                var state = _runtime[stream].state
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength + 1;// lapse

                var zeroDuration = to - from;

                if (nodeSet && propSet) {
                    for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                        let nodeBind = nodeSet[n][stream];

                        for(int p = 0, plen = propSet.Length; p<plen; p++) {
                            let nodeBindProp = nodeBind[propSet[p]];

                            let data0PosI = nodeBindProp.data0PosI;

                            for(int ew = 0; ew<zeroDuration; ew++) {
                                let dataPos = from + ew;
                                dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                            let dataPosI = data0PosI + dataPos;
                                if (skipLeap) if (access.data[dataPosI + 1] == access.arguments.leap) continue;
                                access.data[dataPosI + 1] = 0;
                            }
                        }
                    }
                } else {
                    for(int id in access.bindings.ids) {
                        let nodeBinds = access.bindings.ids[id].node[stream];

                        for(int prop in nodeBinds) {
                            if (nodeBinds[prop].data0PosI) {
                                let data0PosI = nodeBinds[prop].data0PosI;

                                for(int ew = 0; ew<zeroDuration; ew++) {
                                    let dataPos = from + ew;
                                    dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                                let dataPosI = data0PosI + dataPos;

                                    if (skipLeap) if (access.data[dataPosI + 1] == access.arguments.leap) continue;
                                    access.data[dataPosI + 1] = 0;
                                }
                            }
                        }
                    }
                }
            }

            public void ValIn(Timeline[] timelines, TLType[] nodeSet, string[] propSet, float val, int from, int to, bool exact, int at, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap = true) {
                var state = _runtime[stream].state
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength + 1;

                if (nodeSet && propSet) {
                    for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                        let nodeBind = nodeSet[n][stream];

                        for(int p = 0, plen = propSet.Length; p<plen; p++) {
                            let nodeBindProp = nodeBind[propSet[p]];

                            let data0PosI = nodeBindProp.data0PosI;

                            let valDuration = to - from;

                            for(int ew = 0; ew<valDuration; ew++) {
                                let dataPos = from + ew;
                                dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                            let dataPosI = data0PosI + dataPos;
                                if (skipLeap) if (access.data[dataPosI + 1] == access.arguments.leap) continue;
                                access.data[dataPosI + 1] = TMath.Type.ConvertToType(nodeBind.conversion, val);
                            }
                        }
                    }
                }
                if (leapCallback != null) {
                    that.AssignLeap(timelines, [nodeSet[0]], propSet, exact, at, leapCallback, reassign, dispose, zeroIn);
                }
            }

            public void GetforwardData(Timeline[] timelines, TLType[] nodeSet, string[] propSet, int get, bool skipLeap = true) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength + 1;

                var obj = [];

                for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                    let nodeBind = nodeSet[n][stream];
                    obj[n] = {};

                    for(int p = 0, plen = propSet.Length; p<plen; p++) {
                        let prop = propSet[p];

                        let nodeBindProp = nodeBind[prop];
                        let data0PosI = nodeBindProp.data0PosI;

                        // byteOffset grabbed from the stream
        let byteOffset = access.data[data0PosI] + 2;// accuracy fix go forward (2) in offset

                        obj[n][prop] = 0;

                        for(int ew = 0; ew<get; ew++) {
                            let dataPos = byteOffset + ew;
                            dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                        let dataPosI = data0PosI + dataPos;
                            if (skipLeap) if (access.data[dataPosI + 1] == access.arguments.leap) continue;
                            obj[n][prop] += access.data[dataPosI + 1];
                        }
                    }
                }
                return obj;
            }

            public void InjectData(Timeline[] timelines, TLType[] nodeSet, string[] propSet, float[] data, int inject, bool blend, int min, int max, bool skipLeap = true) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength;
                for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                    let nodeBind = nodeSet[n][stream];

                    for(int p = 0, plen = propSet.Length; p<plen; p++) {
                        let nodeBindProp;
                        let data0PosI;
                        let byteOffset;
                        let dataPosI;
                        let prop;

                        if (nodeBind.conversion == "poly") {
                            prop = 0;
                            nodeBindProp = nodeBind[prop];
                            data0PosI = nodeBindProp.data0PosI;
                            byteOffset = access.data[data0PosI];
                        } else {
                            prop = propSet[p];
                            nodeBindProp = nodeBind[prop];
                            data0PosI = nodeBindProp.data0PosI;
                            byteOffset = access.data[data0PosI];
                        }

                        for(int ew = 0; ew<inject; ew++) {
                            let dataPos = byteOffset + ew;
                            dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                            if (nodeBind.conversion == "poly") {
                                for(int pi = 0, plen = data.Length; pi<plen; pi++) {
                                    let polyPropBind = nodeBind[pi];
                                    data0PosI = polyPropBind.data0PosI;
                                    dataPosI = data0PosI + dataPos;

                                    if (skipLeap) if (access.data[dataPosI] == access.arguments.leap) continue;

                                    access.data[dataPosI] = blend? data[pi] + access.data[dataPosI] : data[pi];

                                access.data[dataPosI] =
                                    access.data[dataPosI] < min ? min
                                    : access.data[dataPosI] > max ? max
                                    : access.data[dataPosI];
                                }
                            } else {
                                dataPosI = data0PosI + dataPos;
                                if (skipLeap) if (access.data[dataPosI] == access.arguments.leap) continue;
                                access.data[dataPosI] = blend? data + access.data[dataPosI] : data;

                            access.data[dataPosI] =
                                access.data[dataPosI] < min ? min
                                : access.data[dataPosI] > max ? max
                                : access.data[dataPosI];
                            }
                        }
                    }
                }
            }

            public Dictionary<int, object[]> list = new Dictionary<int, object[]>();
        public void Queue() {
                that.list = that.list;
                that.list.push(arguments);
                return that.list.Length - 1;
            }

            public void Runcallback() {
                let list = that.list;
                while (list.Length > 0) {
                    let method = Array.prototype.shift.apply(list[0]);// shift arguments array list[0].shift() doesn"t work
                    that[method].apply(this, list[0]);
                    list.shift();
                }
                if (callback) callback();
            }

            public void loadData(Timeline[] timelines, string src, int offset, Func<int> callback, int nodeDataLength, int propDataLength) {
                var script = new window.XMLHttpRequest();
                script.open("GET", src, true);
                script.responseType = "text";
                script.callback = callback;
                script.onload = function () {
            var data = processData(this.response);
                    that.buffinData(timelines, data.data, offset, nodeDataLength || data.nodeDataLength, propDataLength || data.propDataLength);

                    if (this.callback) this.callback();
                }
        script.send();
            }

            void ProcessData(string csv) {
                csv = csv.split(",");
                var nodeDataLength = Array.prototype.shift.apply(csv);
                var propDataLength = Array.prototype.shift.apply(csv);
                return {data: csv, nodeDataLength: nodeDataLength << 0, propDataLength: propDataLength << 0};
            }

            public void BuffinData(Timeline[] timelines, float[] data, int offset, int nodeDataLength, int propDataLength) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;
                let continuancePosValData0 = 1;
                let data0PropDataLength = continuancePosValData0 + propDataLength;
                for (
                    let dI = 0,
                        partition = 0,
                        partFrac = 0,
                        cursor = 0,
                        reads = 0;
                    dI<data.Length;
                    dI++
                    ) {
                    // Node Level//
                    if ((dI - partition) % nodeDataLength == 0) {
                        // var node_i = dI/nodeDataLength;//node index number
                        // ->> Node Selection > buffer identifier
                        var nodeBdIK = data[dI];
                        dI++;
                        cursor = 1;
                        reads++;
                    }

                    // Property Level//
                    if ((dI - reads) % data0PropDataLength == partFrac) {
                        let chunkStartI = (dI - cursor);
                        // var prop = ((dI-reads) - ( data0PropDataLength * propsPerNode * node_i)) / data0PropDataLength;//property index number of chunk
                        // ->> Property Selection > buffer identifier

        var propBdIK = data[dI];
                        dI++;
                        cursor++;
                        reads++;

                        let dataPos = ((dI - cursor) - chunkStartI);
                        let dataPosI = dI - dataPos;
                        let endPos = (data0PropDataLength - dataPos) - 1;
                        let endPosI = dataPosI + endPos;
                    }

                    let setBind = state == "prebuff" ? access.ids["_bi" + nodeBdIK] : access.bindings.ids["_bi" + nodeBdIK];
                    let setBindProperty = setBind[propBdIK];
                    let data0PosI = setBind.node[stream][setBindProperty.binding].data0PosI;
                    for(int l = 0; l<propDataLength; l++) {
                        access.data[data0PosI + offset + l] = data[dI];// a...
                        dI++;
                    }
                    // dI = endPosI
                    // this._updateDataPos()
                }
            }

            public void GetData(Timeline timeline, TLType[] nodeSet, string[] propSet, int from, int to, int nodeDataLength, int propDataLength) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                to = to
                ? (to<from) || to> access.propDataLength
                ? access.propDataLength
                    : to
                : access.propDataLength;

            from = from
            ? (to && from > to) || from > access.propDataLength
                ? 0
                : from
            : 0;


            var data = that.BuffoutData(timeline, nodeSet, propSet, from, to);

                return data;
            }

            public void SaveData(float[] data, string fileName) {
                var a = document.createElement("a");
                document.body.appendChild(a);
                a.style = "display: none";
                return function (data, fileName) {
            var json = [data];
                    var blob = new window.Blob([json], { type: "octet/stream"});
                    var url = window.URL.createObjectURL(blob);
                    a.href = url;
                    a.download = fileName;
                    a.click();
                    window.URL.revokeObjectURL(url);
                }
            }

            public void BuffoutData(Timeline[] timelines, TLType[] nodeSet, string[] propSet, int from, int to) {
                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                let valDuration = to - from;
                let continuancePosValData0 = 1;
                                    let data0PropDataLength = continuancePosValData0 + valDuration;
                                    let nodeDataLength = propSet ? data0PropDataLength * propSet.Length + propSet.Length + 1 : 0;

                var data = [nodeDataLength, valDuration];

                if (nodeSet) {
                    for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                        let nodeBind = nodeSet[n][stream];

                        if (!propSet) {
                            propSet = [];
                            for(int propKey in nodeBind) {
                                if (typeof nodeSet[n][propKey] != "undefined") propSet.push(propKey);
                            }
                        }
                        if (nodeDataLength == 0) {
                            nodeDataLength = data0PropDataLength* propSet.Length + propSet.Length + 1;
                            data = [nodeDataLength, valDuration];
                        }

                        data.push(nodeBind.binding.replace("_bi", "") << 0);

                        for(int p = 0, plen = propSet.Length; p<plen; p++) {
                            let nodeBindProp = nodeBind[propSet[p]];
                            data.push(nodeBindProp.binding);

                            let data0PosI = nodeBindProp.data0PosI;
                            data.push(access.data[data0PosI + from]);

                            for(int ew = 0; ew<valDuration; ew++) {
                                let dataPos = from + ew;
                                dataPos = dataPos<access.propDataLength? dataPos : access.Reversion(dataPos);
                            let dataPosI = data0PosI + dataPos;
                            data.push(access.data[dataPosI + 1]);
                            }
                        }
                    }
                }
                return data;
            }*/

            public void Build(Func<int> CallBack = null)
            {
                Msg(Scenes.timeline);
                BuildOff(Scenes.timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline timeline, Func<int> CallBack = null)
            {
                Msg(timeline);
                BuildOff(timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline[] timelines, Func<int> CallBack = null)
            {
                for (int t = 0; t < timelines.Length; t++)
                {
                    Msg(timelines[t]);
                    BuildOff(timelines[t]);
                }
                if (CallBack != null) CallBack();
            }
            void Msg(Timeline timeline)
            {
                TimelineCode.Log("(" + timeline.name + ")" + " Buffering Stream...");
            }
            void BuildOff(Timeline timeline = null)
            {
                timeline.access.Build();
            }
        }
    }
}
