using UnityEngine;
using System.Collections.Generic;
using FixedPointMath;

/// <summary>
/// Class defining the physics engine core.
/// </summary>
public class PhysicsEngine : MonoBehaviour{

    //physics constants
    public const int MAX_BODIES = 4096;
    public const int ITERATIONS = 1;
    public static readonly intf EPSILON = intf.Create(0.0001);
    public static readonly intf SQR_EPSILON = EPSILON * EPSILON;
    public static readonly intf PENETRATION_CORRECTION = intf.Create(0.4);
    public static readonly intf PENETRATION_SLOP = intf.Create(0.04);
    public static readonly Vector2f GRAVITY = new Vector2f(0, 0);
    
    public int sceneWidth;
    public int sceneHeight;
    public int cellSize;
    public bool draw;

    public static float alpha;
    private static PhysicsEngine instance;
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
    /// Draw the quadtree.
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
    public static PhysicsEngine Instance {
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
    }

    /// <summary>
    /// Main physics loop, find collisions, resolve them and move the bodies.
    /// </summary>
    /// <param name="delta"> amount of time for this simulation step</param>
    public void Step(intf delta) {
        if (!simulate)
            return;
        contacts.Clear();

        //apply forces and integrate
        foreach (DBody body in bodies) {
            if (!body.IsFixed() && !body.IsSleeping()) {
                integrator.Integrate(body, delta);
            }
        }

        //find collisions
        detector.Build(bodies);
        detector.FindPotentialCollisions(contacts);

        //resolve collisions
        for (uint i = 0; i < ITERATIONS; i++) {
            foreach (Manifold contact in contacts) {
                contact.ApplyImpulse();
            }
        }

        //TODO fix positional correction (jittering and not properly working)
        /*correct positions
        foreach (Manifold contact in contacts) {
            contact.CorrectPosition();
        }*/
    }
}
