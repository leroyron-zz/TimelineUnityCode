using UnityEngine;
using UnityEditor;
using System;
using TLExtensions;

public partial class CommonMonoBehaviour : MonoBehaviour
{
    static public int CallOnce = -1;
    static public int data = 0;
    bool Lock (int stage, string msg) {
        if (stage <= CallOnce) {
            return true;
        } else {
            Log("-------------------------------------------------------------"+msg);
            CallOnce = stage;
            return false;
        }
    }
    bool BiLock (int stage) {
        if (stage == CallOnce) {
            return true;
        } else {
            CallOnce = stage;
            return false;
        }
    }
    static Action[] cAwakes = new Action[10];
    static int cAwakesLength = 0;
    static public void AddAwake(Action action, int index) {
        cAwakes[index] = action;
    }
    void Awake()
    {
        if (Lock(0, "Awake "+cAwakes.Count())) return;
        gameInput.mouseUp = new bool[3];
        gameInput.mouseDown = new bool[3];
        gameInput.keyUp = new bool[3];
        gameInput.keyDown = new bool[3];
        PointerTransform = GameObject.Find("Cube").GetComponent<Transform>();
        material = Resources.Load("Materials/LineMaterial", typeof(Material)) as Material;
        m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
        m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
        for (int a = 0; a < cAwakes.Length; a++) {
            if (cAwakes[a]== null) break; cAwakes[a]();
        }
    }


    static Action[] cStarts = new Action[10];
    static int cStartsLength = 0;
    static public void AddStart(Action action, int index) {
        cStarts[index] = action;
    }
    void Start()
    {
        if (Lock(1, "Start "+cStarts.Count())) return;
        for (int s = 0; s < cStarts.Length; s++) {
            if (cStarts[s] == null) break; cStarts[s]();
        }
    }

    static Action<GameInput>[] cInputs = new Action<GameInput>[10];
    static int cInputsLength = 0;
    static public void AddInput(Action<GameInput> action, int index) {
        cInputs[index]= action;
    }
    void Inputs(GameInput gameInput)
    {
        for (int u = 0; u < cInputs.Length; u++) {
            if (cInputs[u] == null) break; cInputs[u](gameInput);
        }
    }

    public struct GameInput
    {
        static Vector3 _position = new Vector3();
        public Vector3 position { get { return _position; } set { _position = value; } }

        static Vector3 _rayPosition = new Vector3();
        public Vector3 rayPosition { get { return _rayPosition; } set { _rayPosition = value; } }
        public bool[] mouseUp;
        public bool[] mouseDown;
        public bool[] keyUp;
        public bool[] keyDown;
        public Input input;
    }
    public static GameInput gameInput = new GameInput();

    static Action[] cUpdates = new Action[10];
    static int cUpdatesLength = 0;
    static public void AddUpdate(Action action, int index) {
        cUpdates[index]= action;
    }
    bool alter = false;
    void Update()
    {
        if (BiLock(2)) return;
        alter = false;
        if (Input.GetMouseButtonDown(0))
        {
            Log("Pressed primary button.");
            gameInput.position = Input.mousePosition;
            RayInput(0, true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Log("Pressed secondary button.");
            gameInput.position = Input.mousePosition;
            RayInput(1, true);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Log("Pressed middle click.");
            gameInput.position = Input.mousePosition;
            RayInput(2, true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Log("Pressed primary button.");
            gameInput.position = Input.mousePosition;
            RayInput(0, false, 0, true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Log("Pressed secondary button.");
            gameInput.position = Input.mousePosition;
            RayInput(1, false, 1, true);
        }
        if (Input.GetMouseButtonUp(2))
        {
            Log("Pressed middle click.");
            gameInput.position = Input.mousePosition;
            RayInput(2, false, 2, true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Log("Esc Pressed.");
            RayInput(null, false, null, false, 0, true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Log("LShift Pressed.");
            RayInput(null, false, null, false, 1, true);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Log("Esc Pressed.");
            RayInput(null, false, null, false, 0, false, 0, true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Log("LShift Pressed.");
            RayInput(null, false, null, false, 1, false, 1, true);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            gameInput.position = touchDeltaPosition;
            // Move object across XY plane
            //PointerTransform.Translate(-touchDeltaPosition.x * 0.1f, -touchDeltaPosition.y * 0.1f, 0);

            Log("Touch called.");

            RayInput(0, false, 0, true);
        }

        if (!alter) RayMove();
        for (int u = 0; u < cUpdates.Length; u++) {
            if (cUpdates[u] == null) break; cUpdates[u]();
        }
    }

    internal Transform PointerTransform;
    float gridPlaneCenterZ = 0f;
    Vector3 m_DistanceFromCamera;
    internal Material material;
    Plane m_Plane;
    Ray ray;
    public void RayInput(int? downState = null, bool downTrue = false, int? upState = null, bool upTrue = false, int? keyDownState = null, bool keyDownTrue = false, int? keyUpState = null, bool keyUpTrue = false)
    {
        m_DistanceFromCamera.z = gridPlaneCenterZ;
        if (upState != null) gameInput.mouseUp[upState??0] = upTrue;
        if (downState != null) gameInput.mouseDown[downState??0] = downTrue;
        if (keyUpState != null) gameInput.keyUp[keyUpState??0] = keyUpTrue;
        if (keyDownState != null) gameInput.keyDown[keyDownState??0] = keyDownTrue;
        ray = Camera.main.ScreenPointToRay(gameInput.position);
        //Initialize the enter variable
        float enter = 0.0f;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            //Move your cube GameObject to the point where you clicked
            //if (PointerTransform) PointerTransform.position = hitPoint;
            gameInput.rayPosition = hitPoint;
        }
        alter = true;
        Inputs(gameInput);
        gameInput.mouseUp[upState??0] = false;
        gameInput.keyUp[keyUpState??0] = false;
    }

    public void RayMove()
    {
        gameInput.position = Input.mousePosition;
        m_DistanceFromCamera.z = gridPlaneCenterZ;
        ray = Camera.main.ScreenPointToRay(gameInput.position);
        //Initialize the enter variable
        float enter = 0.0f;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            //Move your cube GameObject to the point where you clicked
            //if (PointerTransform) PointerTransform.position = hitPoint;
            gameInput.rayPosition = hitPoint;
        }
        Inputs(gameInput);
    }
    
    static Action[] cOnPostRenders = new Action[10];
    static int cOnPostRendersLength = 0;
    static public void AddOnPostRender(Action action, int index) {
        cOnPostRenders[index]= action;
    }
    void OnPostRender()
    {
        if (BiLock(3)) return;
        for (int r = 0; r < cOnPostRenders.Length; r++) {
            if (cOnPostRenders[r]== null) break; cOnPostRenders[r]();
        }
    }

    /*
    static Action[] cOnDrawGizmoes = new Action[10];
    static int cOnDrawGizmoesLength = 0;
    static public void AddOnDrawGizmos(Action action, int index) {
        cOnDrawGizmoes[index]= action;
    }
    void OnDrawGizmos()
    {
        if (BiLock(4)) return;
        for (int g = 0; g < cOnDrawGizmoes.Length; g++) {
            if (cOnDrawGizmoes[g] == null) break; cOnDrawGizmoes[g]();
        }
    }
    
    static Action[] cOnSceneGUIs = new Action[10];
    static int cOnSceneGUIsLength = 0;
    static public void AddOnSceneGUI(Action action, int index) {
        cOnSceneGUIs[index]= action;
    }
    public void OnSceneCallGUI()
    {
        if (BiLock(5)) return;
        for (int g = 0; g < cOnSceneGUIs.Length; g++) {
            if (cOnSceneGUIs[g] == null) break; cOnSceneGUIs[g]();
        }
    }

    static Action[] cOnGUIs = new Action[10];
    static int cOnGUIsLength = 0;
    static public void AddOnGUI(Action action, int index) {
        cOnGUIs[index]= action;
    }
    void OnGUI()
    {
        if (BiLock(6)) return;
        for (int g = 0; g < cOnGUIs.Length; g++) {
            if (cOnGUIs[g] == null) break; cOnGUIs[g]();
        }
    }
    */

    public static void Log(object msg)
    {
        Debug.Log("Common: " + msg);
    }
}