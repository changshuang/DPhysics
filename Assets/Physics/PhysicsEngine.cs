using UnityEngine;
using System.Collections.Generic;
using FixedPointMath;

/// <summary>
/// Class defining the physics engine core.
/// </summary>
public class PhysicsEngine : MonoBehaviour{

    public const int maxObjectCount = 4096;
    public const float correctionf = 0.4f;
    public const float slopf = 0.01f;
    public int sceneWidth;
    public int sceneHeight;
    public int cellSize;
    public bool draw;

    public static float alpha;
    private static PhysicsEngine instance;
    private static int bodyCount;

    private List<DBody> objects;
    private ICollisionDetector detector;
    private bool simulate;
    private intf correction;
    private intf slop;

    //TODO: remove this
    private GUIStyle style;

    /// <summary>
    /// Initializes the engine and sets the instance value.
    /// </summary>
    void Awake() {
        instance = this;
        bodyCount = 0;
        objects = new List<DBody>();
        detector = new HashGridDetector(cellSize, sceneWidth, sceneHeight);
        simulate = false;
        correction = intf.Create(correctionf);
        slop = intf.Create(slopf);
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
        if (bodyCount > maxObjectCount)
            return;

        obj.SetID(bodyCount);
        bodyCount++;
        this.objects.Add(obj);
        this.detector.Insert(obj);
    }

    /// <summary>
    /// Main physics loop, find collisions, resolve them and move the bodies.
    /// </summary>
    /// <param name="delta"> amount of time for this simulation step</param>
    public void UpdatePhysics(int frames) {
        if (!simulate)
            return;

        Profiler.BeginSample("Collision detection");
        HashSet<Manifold> broadPhaseCollisions = detector.FindPotentialCollisions();
        Profiler.EndSample();
        foreach (Manifold collision in broadPhaseCollisions) {
            if (!collision.IsTrigger()) {
                ResolveCollision(collision, frames);
            }
        }
    
        //for each physics object, apply forces
        foreach(DBody obj in objects) {
            if (!obj.IsFixed() && !obj.IsSleeping()) {

                Profiler.BeginSample("Remove");
                detector.Remove(obj);
                Profiler.EndSample();

                obj.Integrate(frames);

                Profiler.BeginSample("Insert");
                detector.Insert(obj);
                Profiler.EndSample();
            }
        }
    }

    /// <summary>
    /// Resolve the collision by calculating the resulting velocity, using the
    /// given normal and penetration, stored in the intersection.
    /// </summary>
    /// <param name="collision">Intersection instance containing all the collision data.</param>
    private void ResolveCollision(Manifold collision, int frames) {
        DBody a = collision.GetA();
        DBody b = collision.GetB();
        Vector2f rv = b.Velocity - a.Velocity;
        intf normalVel = Vector2f.Dot(rv, collision.Normal);

        if (normalVel > 0)
            return;

        intf e = FixedMath.Min(a.Restitution, b.Restitution);
        intf j = (-(1 + e) * normalVel) / (a.InvMass + b.InvMass);

        Vector2f impulse = collision.Normal * j;
        a.SetVelocity(-impulse * a.InvMass);
        b.SetVelocity(impulse * b.InvMass);
        CorrectPosition(collision);
    }

    private void CorrectPosition(Manifold collision) {
        DBody a = collision.GetA();
        DBody b = collision.GetB();
        intf totInvMass = a.InvMass + b.InvMass;
        intf penetration = FixedMath.Max(collision.Distance - slop, (intf)0);
        Vector2f corr = collision.Normal * (penetration / totInvMass) * correction;

        a.CorrectPosition(-corr);
        b.CorrectPosition(corr);
    }
}
