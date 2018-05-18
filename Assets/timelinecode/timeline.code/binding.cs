using UnityEngine;
using math;

partial class TIMELINE
{
    public partial class CODE
    {
        public BINDING binding = new BINDING();

        public class BINDING
        {
            TIMELINE timeline;
            CODE code;
            ACCESS _access;
            
            public float[] data;
            public int nodesPerStream;
            public int propsPerNode;
            public int[] propsPerNodeList;
            public int propDataLength;
            public int continuancePosValData0;
            public int data0PropDataLength;
            public int nodeDataLength;
            public int streamDataLength;
            public object proxy = 0;

            public string state;

            public void init(TIMELINE timeline)
            {
                this.timeline = timeline;
                this.code = timeline.code;
                this._access = timeline._access;
                
                test();
                timeline.Log("Init Binding");
            }

            public void test()
            {
                TLVector3 transform = new TLVector3(timeline, 10.0F, 2.0F, 3.0F, "translate");
                /*add(
                    new object[]{
                            timeline,
                            timeline,
                            timeline,
                            timeline,
                            transform, 101,
                            "x", 0F, 100F,
                            "y", 0F, 200F,
                            102,
                            false,
                            1F
                    }
                );*/
                float[] audioFreqData = new float[]{1F,1F,1F};
                TLPoly freq = new TLPoly(timeline, new float[]{1F,1F,1F});
                add(
                    new object[]{
                            timeline,
                            timeline,
                            timeline,
                            timeline,
                            freq, 800,
                            "poly", audioFreqData,
                            802,
                            false,
                            1F
                    }
                );
            }

            object[] ids;
            object[] instructions;
            int buffIdKey = 800;
            int propIdKey;
            void add(object[] options)
            {
                instructions = code.instructionSet(options);

                object[] timelines = (object[])instructions[0];
                int timelineCount = code.count(timelines);

                object[] objs = (object[])instructions[1];
                int objCount = code.count(objs);

                if (this.proxy.GetType() == typeof(int))
                {
                    this.data = new float[0];
                    this.nodesPerStream = 0;
                    this.propsPerNode = instructions[3] != null ? code.count(instructions[3]) : instructions[2] != null ? code.count(instructions[2]) : 0;
                    this.propDataLength = timeline.length;
                    this.propsPerNodeList = new int[0];
                    this.state = "prebuff";
                    this.continuancePosValData0 = 1;
                    code.reversion = (int dataPos) => {
                        return dataPos - (this.propDataLength * (dataPos / this.propDataLength << 0)) + this.continuancePosValData0;
                    };
                }
                if (this.proxy.GetType() == typeof(int))
                {
                    //this.proxy = code.proxy();
                }
                int[] runtimePropsPerNodeList = this.propsPerNodeList;
                int runtimeLastPropsPerNode = runtimePropsPerNodeList.Length- 1;

                object[] initObj = (object[])objs[0];
                TLVector3 initProp = (TLVector3)initObj[0];
                //initProp.type = "position";

                // string dataType = (string)code.checkListGet(initProp.type, new object[] { "translation", "radian", "poly", "uniform", initProp.type });
                string dataType = (string)code.checkListGet(initProp.type, new
                                        string[]{
                                            "position=translation",
                                            "Vector4=translation",
                                            "Vector3=translation",
                                            "Vector2=translation",
                                            "rotation=radian",
                                            "Euler=radian",
                                            "translate=translation",
                                            "translation="+initProp.type,
                                            "radian="+initProp.type,
                                            "poly="+initProp.type,
                                            "other=uniform"
                                        });

                propIdKey = dataType != "radian" ? 806 : 801;

                this.nodesPerStream += code.count(instructions[1]);
                bool relative = (bool?)instructions[4] ?? _access.defaults.relative;
                // no percision
                float dataTypePrecision = (float?)instructions[5] ?? (float)code.checkListGet(dataType, new
                                        string[]{
                                            "translation=1000",
                                            "radian=10000",
                                            "uniform=1000"
                                        });
                float[] fff = new float[]{-1024F, -512F, 0F, 1F, -1F, 0F};
                Vector3 v33 = new Vector2(2,2);
                Math.Poly.multiplyScalarVector(fff, v33);
                for (int t = 0; t < timelineCount; t++) {
                    TIMELINE timeline = (TIMELINE)timelines[t];
                    for (int o = 0; o < objCount; o++) {
                        object[] obj = (object[])objs[0];
                        TLVector3 prop = (TLVector3)obj[0];
                        int buffKey = (int)obj[1];

                        
                        /*Math.Poly.generate(dataType, instructions[2][0][1], dataTypePrecision);

                        object props = dataType == "poly" ? Math.Poly.generate(dataType, instructions[2][0][1], dataTypePrecision)
                            : Math.Type.convertToPrecisionDataType(dataType, instructions[2].concat(), 1, 2, dataTypePrecision);
                        object pkeys = dataType == "poly"
                            ? Math.Poly.generateKeys(instructions[2][0][1], instructions[3]
                                                                        ? instructions[3][instructions[3].length - 1]
                                                                        : that.propIdKey)
                            : instructions[3]
                            ? instructions[3].concat()
                            : Math.Poly.generateKeys(instructions[2], instructions[3]
                                                                    ? instructions[3][instructions[3].length - 1]
                                                                    : that.propIdKey);*/

                        //string type = (instructionObject)objs[o].type;
                        //code.checkList(objs[0][0], new object[] { "translation", "radian", "poly", "uniform", objs[0][0].type, instructions[2][0][0] });
                    }
                }
            }
        }
    }
}

