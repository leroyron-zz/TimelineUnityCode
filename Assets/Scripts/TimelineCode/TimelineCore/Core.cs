using System;
using UnityEngine;
using TLExtensions;

public partial class Timeline
{
    public partial class Core
    {
        public class TLGameObject
        {
            public GameObject gameObject;

            public TLTransform transform;
            public TLType[] nodes;
            public TLGameObject(string gameObjectName) {
                gameObject = GameObject.Find(gameObjectName);
                transform = new TLTransform(gameObject.transform);
            }
        }

        public class TLTransform
        {
            //public Transform transform;
            public TLVector3 position;
            public TLVector3 rotation;
            public TLTransform(Transform transform) {
                //this.transform = transform;
                this.position = new TLVector3("position", transform, transform.position);
                this.rotation = new TLVector3("rotation", transform, transform.eulerAngles);
            }
        }
                
        public class TLVector3 : TLType
        {
            public Transform transform;
            public Vector3 vector;
            private Action DoTransform;  
            public override Vector3 xyz { 
                get { return vector; } 
                set { DoTransform(); } 
            }
            public override float x { 
                get { 
                        return vector.x; 
                    } 
                set { 
                        vector.x = value;
                        xyz = vector;
                    } 
            }
            public override float y { 
                get { 
                        return vector.y; 
                    } 
                set { 
                        vector.y = value;
                        xyz = vector;
                    } 
            }
            public override float z { 
                get { 
                        return vector.z; 
                    } 
                set { 
                        vector.z = value;
                        xyz = vector;
                    } 
            }
            public struct Exec
            {
                public ExecParams x, y, z;
                public Exec(TLVector3 This)
                {
                    x = new ExecParams(This);
                    y = new ExecParams(This);
                    z = new ExecParams(This);
                }
            }
            public Exec timeline;
            public TLVector3(string type = "position", Transform transform = null, Vector3 vector = new Vector3()) {
                this.type = type;
                this.transform = transform;
                this.vector = vector;
                this.timeline = new Exec(this);
                if (this.type == "rotation") 
                    DoTransform = () => {
                        this.transform.rotation = Quaternion.Euler(this.vector);
                    }; 
                else 
                    DoTransform = () => {
                        this.transform.position = this.vector;
                    };
            }
            public TLVector3(string type = "position", float x = 0, float y = 0, float z = 0) {
                Init(x, y, z, type);
            }
            public TLVector3(float x = 0, float y = 0, float z = 0, string type = "position") {
                Init(x, y, z, type);
            }
            void Init(float x, float y, float z, string type) {
                this.type = type;
                this.vector = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new Exec(this);
                if (this.type == "rotation") 
                    DoTransform = () => {
                        this.transform.rotation = Quaternion.Euler(this.vector);
                    }; 
                else 
                    DoTransform = () => {
                        this.transform.position = this.vector;
                    };
            }
        }
        public class TLVector2 : TLType
        {
            public Vector2 vector;
            public override float x { get { return vector.x; } set { vector.x = value; } }
            public override float y { get { return vector.y; } set { vector.y = value; } }
            public struct Exec
            {
                public ExecParams x, y;
                public Exec(TLVector2 This)
                {
                    x = new ExecParams(This);
                    y = new ExecParams(This);
                }
            }
            public Exec timeline;
            public TLVector2(string type = "position", float x = 0, float y = 0) {
                Init(x, y, type);
            }
            public TLVector2(float x = 0, float y = 0, string type = "position") {
                Init(x, y, type);
            }
            void Init(float x, float y, string type) {
                this.type = type;
                this.vector = new Vector2(this.x = x, this.y = y);
                this.timeline = new Exec(this);
            }
        }
        public class TLPoly : TLType
        {
            public ExecParams[] timeline;
            public TLPoly(float[] poly = null) {
                this.poly = Init(poly);
            }
            private float[] Init(float[] poly) {
                this.type = "poly";
                this.timeline = new ExecParams[poly != null ? poly.Length : 0];
                for (int p = 0; p < timeline.Length; p++) {
                    this.timeline[p] = new ExecParams(this);
                }
                return poly;
            }
        }
        const float _radianMax =  Mathf.PI * 2;
        public class TLVectors
        {
            public bool isRadian { get; set; }
            public float x, y, z, w, u, v;
            public TLVectors(object param) {
                this.x = Convert.ToSingle(param.GetMember("x"));
                this.y = Convert.ToSingle(param.GetMember("y"));
                this.z = Convert.ToSingle(param.GetMember("z"));
                this.w = Convert.ToSingle(param.GetMember("w"));
                this.u = Convert.ToSingle(param.GetMember("u"));
                this.v = Convert.ToSingle(param.GetMember("v"));
                this.isRadian = this.x < _radianMax
                &&
                this.y < _radianMax
                &&
                this.z < _radianMax
                &&
                this.w < _radianMax;
            }
        }
        public class TLType
        {
            public virtual string type { get; set; }
            public float[] poly;
            public virtual float x {get; set;}
            public virtual float y{get; set;}
            public virtual float z{get; set;}
            public virtual Vector3 xyz{get; set;}
            public virtual float w{get; set;}
            public virtual float u{get; set;}
            public virtual float v{get; set;}
            public virtual float value{get; set;}
            public virtual float radius{get; set;}
            public virtual float rotation{get; set;}
            public virtual float alpha{get; set;}
            public virtual float scale{get; set;}
            public struct Exec
            {
                public int binding;
                public string conversion; 
                public int position; 
                public bool relative;
                public ExecParams x, y, z, w, u, v, value, radius, rotation, alpha, scale;
            }
            public Exec timeline;
        }
        public class TLElement : TLType
        {
            public float value, radius, rotation, alpha, scale;
            public struct Exec
            {
                public ExecParams value, radius, rotation, alpha, scale;
            }
            public Exec timeline;
            public TLElement(string type = null, string[] names = null, float[] values = null)
            {
                this.type = "uniform";
                string name;
                float value;
                for (int f = 0; f < names.Length; f++) {
                    name = (string)names[f];
                    value = (float)values[f];
                    switch (name)
                    {
                        case "value":
                            this.value = value;
                            this.timeline.value = new ExecParams(this);
                            break;
                        case "radius":
                            this.radius = value;
                            this.timeline.radius = new ExecParams(this);
                            break;
                        case "rotation":
                            this.rotation = value;
                            this.timeline.rotation = new ExecParams(this);
                            break;
                        case "alpha":
                            this.alpha = value;
                            this.timeline.alpha = new ExecParams(this);
                            break;
                        case "scale":
                            this.scale = value;
                            this.timeline.scale = new ExecParams(this);
                            break;
                        default :
                            break;
                    }
                }
            }
        }
    }
}
