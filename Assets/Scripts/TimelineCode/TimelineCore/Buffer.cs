using System;
using System.Collections.Generic;
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
            Access _access;
            Interpolation _interpolation;

            private int buffOffset;
            private int deltaSetOffset;
            private float valueOffset;

            public void Init(Timeline timeline)
            {
                this._timeline = timeline;
                this._code = timeline.code;
                this._access = timeline.access;
                this._interpolation = timeline.buffer.interpolation;
                TimelineCode.Log("Init Buffer");
            }


            public float[] Eval(Timeline timeline, object[] options, bool relative = false, bool get = false, Func<int> leapCallback = null, bool reassign = false, bool dispose = true, float zeroIn = 0, bool skipLeap = true) {
                Timeline[] timelines = new Timeline[1]{timeline};
                return _Eval(timelines, options, relative, get, leapCallback, reassign, dispose, zeroIn, skipLeap);
            }
            public float[] Eval(Timeline[] timelines, object[] options, bool relative = false, bool get = false, Func<int> leapCallback = null, bool reassign = false, bool dispose = true, float zeroIn = 0, bool skipLeap = true) {
                return _Eval(timelines, options, relative, get, leapCallback, reassign, dispose, zeroIn, skipLeap);
            }
            
            float[] _Eval(Timeline[] timelines, object[] options, bool relative, bool get, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap)
            {
                // move to queue and make stream line
                relative = relative || _access.defaults.relative;
                float[] getData = new float[0];
                for(int e = 0, elen = options.Length; e < elen; e++)
                {
                    object[] option = (object[])options[e];
                    var nodes = option[0];
                    /*var nlen = nodes.Length;
                    let sets = option[2];
                    var slen = sets.Length;
                    let blend = option[3];
                    let propSet = [];

                    for(int n = 0; n < nlen; n++)
                    {
                        let node = nodes[n];

                        let propChain = option[1];
                        for(int pc = 0, pclen = propChain.Length; pc < pclen; pc++)
                        {
                            buffOffset = 0;
                            if (blend)
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

                            let props = propChain[pc];
                            propSet[pc] = propChain[pc][0];

                            for(int p = 0, plen = props.Length; p < plen; p++)
                            {
                                let prop = props[p];
                                propSet[pc] = prop[0];
                                let sumDurations = 0;

                                for(int s = 0; s < slen; s++)
                                {
                                    sets[s][1] <<= 0; // round off to plug in 32intArrayBuffer
                                    sumDurations += sets[s][1];
                                }
                                for(int s = 0; s < slen; s++)
                                {
                                    let getset = sets[s];
                                    deltaSetOffset = getset[1];
                                    prop[2] = prop[1] * (deltaSetOffset / sumDurations);
                                    prop[2] = TMath.Type.ConvertToType(node.timeline.conversion, prop[2]);
                                    Buff(timelines, node, prop, this._interpolation.EvalData(getset[0], deltaSetOffset, deltaSetOffset), relative, skipLeap);
                                    buffOffset += deltaSetOffset;
                                    valueOffset += prop[2];
                                }
                            }
                        }
                        // per-node
                        if (leapCallback != null)
                        {
                            that.AssignLeap(timelines, [node], propSet, false, buffOffset, leapCallback, reassign, dispose, zeroIn);
                        }

                        if (get)
                        {
                            getData[n] = that.GetData(timelines, [node], propSet, get);
                        }
                    }*/
                }
                //that.evals += nlen * slen;
                //that.Update();
                return getData;
            }

            /*public float[] Exec(Timeline[] timelines, object[] options, bool relative, bool get, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap) {
                
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

            /*public void ExecLerp(Timeline[] stream, TLType[] nodeSet, string[] propSet, TLType refnode, string refprop, float startVal, float flux, bool parallel, bool reach, int from, int to, string ease, bool exact, int at, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap) {
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
            }


            public void AssignLeap(Timeline[] stream, TLType[] nodeSet, string[] propSet, bool exact, int at, Func<int> func, bool reassign, bool dispose = true, float zeroIn = null, bool waitForRevert) {
                var state = _runtime[stream].state
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                var propDataLength = access.propDataLength + 1;

                for(int n = 0, nlen = nodeSet.Length; n<nlen; n++) {
                    let nodeBind = nodeSet[n][stream];

                    for(int p = 0, plen = propSet.Length; p<plen; p++) {
                        let prop = propSet[p];

                        let nodeBindProp = nodeBind[prop];
                        let data0PosI = nodeBindProp.data0PosI;

                        // byteOffset grabbed from the stream
        let byteOffset = access.data[data0PosI];// accuracy fix go forward (2) in offset

                        let dataPos = exact ? at : byteOffset + at;
                        dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                    let dataPosI = data0PosI + dataPos;
                    access.data[dataPosI] = access.arguments.leap;


                    let leapDataPosIDelta;
                        if (nodeBindProp.leap) {
            if (reassign)
            {
                leapDataPosIDelta = nodeBindProp.leap[dataPos].dataPosI;
                                access.data[leapDataPosIDelta] = 0;
                            }
        } else {
                            nodeBindProp.leap = [];
                        }

                        nodeBindProp.leap[dataPos] = {callback: func, dispose: dispose, zeroIn: zeroIn, dataPosI: dataPosI};

                        if (!waitForRevert && (!nodeBindProp.leapNext || (dataPos<nodeBindProp.leapNext && dataPos> byteOffset))) nodeBindProp.leapNext = dataPos;
                    }
                }
            } 

            public void Buff(Timeline[] timelines, TLType node, string prop, float[] evalData, bool relative, bool skipLeap, bool ahead) {
                var nodeVal = node[prop[0]].value;
                ? prop[0] == "value" ? node[prop[0]].value : node[prop[0]].value;
                : prop[0] == "value" ? node[prop[0]] : node[prop[0]];

                if (ahead) nodeVal = valueOffset; else nodeVal += valueOffset;
                var nodeBind = node[stream];
                var nodeBindProp = nodeBind[prop[0]];
                var data0PosI = nodeBindProp.data0PosI;

                var state = _runtime[stream].state;
                var access = state == "prebuff"
                ? _runtime[stream]
                : _access;

                // byteOffset grabbed from the stream
                var byteOffset = access.data[data0PosI];
                byteOffset += buffOffset;

                var propDataLength = access.propDataLength + 1;

                var valProp = prop[2];

                var data = evalData[0];
                //debugger;
                var precision = evalData[1];
                // Check if it fills the stream beyond its current position
                var length = data.Length;
                for(int ew = 0; ew<length; ew++) {
                    let dataPos = byteOffset + ew;
                    dataPos = dataPos<propDataLength? dataPos : access.Reversion(dataPos);
                let dataPosI = data0PosI + dataPos;

                    if (skipLeap) if (access.data[dataPosI] == access.arguments.leap) continue;
                    if (relative) {
                        access.data[dataPosI] += (data[ew] - (data[ew + 1] || 0)) / (precision / valProp);
                    } else {
                        access.data[dataPosI] = nodeVal + ((precision - data[ew]) / precision* valProp);
                    }
                }

                that.Update();
            }

            public void ZeroOut(Timeline[] timelines, int from, int to, TLType[] nodeSet, string[] propSet, bool skipLeap) {
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

            public void ValIn(Timeline[] stream, TLType[] nodeSet, string[] propSet, float val, int from, int to, bool exact, int at, Func<int> leapCallback, bool reassign, bool dispose, float zeroIn, bool skipLeap) {
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

            public void GetforwardData(Timeline[] stream, TLType[] nodeSet, string[] propSet, int get, bool skipLeap) {
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

            public void InjectData(Timeline[] stream, TLType[] nodeSet, string[] propSet, float[] data, int inject, bool blend, int min, int max, bool skipLeap) {
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

            public void BuffoutData(Timeline[] stream, TLType[] nodeSet, string[] propSet, int from, int to) {
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

            public void Build(Func<int> CallBack = null) {
                Msg(Scenes.timeline);
                BuildOff(Scenes.timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline timeline, Func<int> CallBack = null) {
                Msg(timeline);
                BuildOff(timeline);
                if (CallBack != null) CallBack();
            }
            public void Build(Timeline[] timelines, Func<int> CallBack = null) {
                for (int t = 0; t < timelines.Length; t++) {
                    Msg(timelines[t]);
                    BuildOff(timelines[t]);
                }
                if (CallBack != null) CallBack();
            }
            void Msg(Timeline timeline) {
                TimelineCode.Log("("+timeline.name+")"+" Buffering Stream...");
            }
            void BuildOff(Timeline timeline = null) {
                timeline.access.Build();
            }
        }
    }
}
