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
            public Transform transform;
            public TLVector3 position;
            public TLVector3 rotation;
            //public float w { get { return transform.rotation.w; } set { transform.rotation.w = value; } }
            // TO-do Optimize Rotation xyz combine and with w
            public float w = 0;
            public Vector4 xyz { 
                get { if (w == 0) return this.rotation.transform; else return this.position.transform; } 
                set { if (w == 0) this.transform.rotation = Quaternion.Euler(this.rotation.transform = value); else this.transform.position = this.position.transform = value; } 
            }
            public float x { 
                get { return rotation.x; } 
                set { transform.rotation = Quaternion.Euler(rotation.x = value, rotation.y, rotation.z); } 
            }
            public float y { 
                get { return rotation.y; } 
                set { transform.rotation = Quaternion.Euler(rotation.x, rotation.y = value, rotation.z); } 
            }
            public float z { 
                get { return rotation.z; } 
                set { transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z = value); } 
            }
            public TLTransform(Transform transform) {
                this.transform = transform;
                this.position = new TLVector3("position", transform.position);
                this.rotation = new TLVector3("rotation", transform.eulerAngles);
            }
        }
        public class TLRotation : TLType
        {
            public Vector3 transform;
            //public float w { get { return transform.w; } set { transform.w = value; } }
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public float z { get { return transform.z; } set { transform.z = value; } }
            public struct Exec
            {
                public ExecParams /*w, */x, y, z;
                public Exec(TLRotation This)
                {
                    //w = new ExecParams(This);
                    x = new ExecParams(This);
                    y = new ExecParams(This);
                    z = new ExecParams(This);
                }
            }
            public Exec timeline;
            public TLRotation(string type = "rotation", Vector3 transform = new Vector3()) {
                this.type = type;
                this.transform = transform;
                this.timeline = new Exec(this);
            }
            public TLRotation(string type = "rotation", float x = 0, float y = 0, float z = 0) {
                Init(x, y, z, type);
            }
            public TLRotation(float x = 0, float y = 0, float z = 0, string type = "rotation") {
                Init(x, y, z, type);
            }
            void Init(float x, float y, float z, string type) {
                this.type = type;
                this.transform = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new Exec(this);
            }
        }
        public class TLVector3 : TLType
        {
            public Vector3 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
            public float z { get { return transform.z; } set { transform.z = value; } }
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
            public TLVector3(string type = "position", Vector3 transform = new Vector3()) {
                this.type = type;
                this.transform = transform;
                this.timeline = new Exec(this);
            }
            public TLVector3(string type = "position", float x = 0, float y = 0, float z = 0) {
                Init(x, y, z, type);
            }
            public TLVector3(float x = 0, float y = 0, float z = 0, string type = "position") {
                Init(x, y, z, type);
            }
            void Init(float x, float y, float z, string type) {
                this.type = type;
                this.transform = new Vector3(this.x = x, this.y = y, this.z = z);
                this.timeline = new Exec(this);
            }
        }
        public class TLVector2 : TLType
        {
            public Vector2 transform;
            public float x { get { return transform.x; } set { transform.x = value; } }
            public float y { get { return transform.y; } set { transform.y = value; } }
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
                this.transform = new Vector2(this.x = x, this.y = y);
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
            public string type { get; set; }
            public float[] poly;
            public struct Exec
            {
                public ExecParams x, y, z, w, u, v, value, radius, rotation, alpha, scale;
            }
            public Exec timeline;
        }
        public class TLElement : TLType
        {
            public float? value, radius, rotation, alpha, scale;
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
