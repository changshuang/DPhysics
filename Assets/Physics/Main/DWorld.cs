using UnityEngine;
using System.Collections.Generic;
using FixedMath;

/// <summary>
/// Class defining the physics engine core.
/// </summary>
public class DWorld : MonoBehaviour{

    //physics constants
    public const int MAX_BODIES = 4096;
    public const int ITERATIONS = 1;
    public const bool IMP_ACCUM = false;
    public static readonly Fix32 EPSILON = (Fix32)0.0001;
    public static readonly Fix32 SQR_EPSILON = EPSILON * EPSILON;
    public static readonly Fix32 PENETRATION_CORRECTION = (Fix32)0.4;
    public static readonly Fix32 PENETRATION_SLOP = (Fix32)0.01;
    public static readonly Vector2F GRAVITY = new Vector2F(0, 0);
    
    public int sceneWidth;
    public int sceneHeight;
    public int cellSize;
    public bool draw;

    public static float alpha;
    private static DWorld instance;
    private static int bodyCount;

    private List<DBody> bodies;
    private HashSet<Manifold> contacts;
    private ICollisionDetector detector;
    private IIntegrator integrator;
    private bool simulate;

    //TODO: remove this
    private GUIStyle style;

    /// <summary>
    /// Initializes the engine and sets the instance value.
    /// </summary>
    void Awake() {
        instance = this;
        bodyCount = 0;
        bodies = new List<DBody>();
        detector = new HashGridDetector(cellSize, sceneWidth, sceneHeight);
        integrator = new EulerImplicit();
        contacts = new HashSet<Manifold>();
        simulate = false;
    }

    /// <summary>
    /// Starts the simulation (temporary).
    /// </summary>
    void Start() {
        simulate = true;
        style = new GUIStyle();
        style.normal.textColor = Color.white;
    }

    /// <summary>
    /// Draws info about the simulation as a GUI.
    /// </summary>
    void OnGUI() {
        Rect label = new Rect(100, 100, 300, 20);
        GUIStyle style = new GUIStyle();
        GUI.Label(label, "body count: " + bodyCount, style);
    }

    /// <summary>
    /// Draw the detection structure.
    /// </summary>
    void OnDrawGizmos() {
        Vector3 center = new Vector3(sceneWidth / 2, 0, sceneHeight / 2);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, new Vector3(sceneWidth, 1, sceneHeight));
        if (detector == null || !draw)
            return;
        detector.Draw();
    }

    /// <summary>
    /// Returns the singleton instance for the engine.
    /// </summary>
    public static DWorld Instance {
        get { return instance; }
    }

    /// <summary>
    /// Adds a physics object to the physics environment.
    /// </summary>
    /// <param name="obj">the new object.</param>
    public void AddObject(DBody obj) {
        if (bodyCount > MAX_BODIES)
            return;

        obj.SetID(bodyCount);
        bodyCount++;
        this.bodies.Add(obj);
        detector.Insert(obj);
    }

    /// <summary>
    /// Main physics loop, find collisions, resolve them and move the bodies.
    /// </summary>
    /// <param name="delta"> amount of time for this simulation step</param>
    public void Step(Fix32 delta) {
        if (!simulate)
            return;

        contacts.Clear();
        Fix32 invDelta = (delta > Fix32.Zero) ? (Fix32)1 / delta : Fix32.Zero;

        //integrate forces
        Profiler.BeginSample("Integrate forces");
        foreach (DBody body in bodies) {
            if (body.IsFixed())
                continue;

            integrator.IntegrateForces(body, delta);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Find collisions");
        detector.FindCollisions(contacts);
        Profiler.EndSample();
     
        //init collision manifolds
        foreach (Manifold contact in contacts) {
            contact.Init(invDelta);
        }

        //resolve collisions
        for (uint i = 0; i < ITERATIONS; i++) {
            foreach (Manifold contact in contacts) {
                contact.ApplyImpulse();
            }
        }

        //integrate velocities
        foreach (DBody body in bodies) {
            if (body.IsFixed())
                continue;

            Profiler.BeginSample("Remove-Integrate-Insert");
            detector.Remove(body);
            integrator.IntegrateVelocities(body, delta);
            detector.Insert(body);
            Profiler.EndSample();
        }
    }
}
