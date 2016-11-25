using FixedPointMath;
using UnityEngine;

/// <summary>
/// Class representing a rigid body inside the physics simulation
/// </summary>
public class DBody {

    private int identifier;

    private DCollider collider;

    private Vector2f position;
    private Vector2f prevPosition;
    private Vector2f velocity;
    private Vector2f force;

    private intf mass;
    private intf invMass;
    private intf restitution;
    private intf friction;
    private bool sleeping;

    /// <summary>
    /// Creates a new rigid body with the given parameters.
    /// </summary>
    /// <param name="collider">the collider for this object</param>
    /// <param name="position">the current position in the space</param>
    /// <param name="mass">the object's mass</param>
    /// <param name="restitution">the "bounciness"</param>
    /// <param name="friction">amount of friction</param>
    public DBody(DCollider collider, Vector2f position, intf mass, intf restitution, intf friction) {
        this.collider = collider;
        this.position = position;
        this.prevPosition = position;
        this.mass = mass;
        this.invMass = (mass > 0) ? ((intf)1 / mass) : (intf)0;
        this.restitution = restitution;
        this.friction = friction;
        this.collider.Body = this;
        this.sleeping = false;
    }

    /// <summary>
    /// Returns the current collider.
    /// </summary>
    public DCollider Collider {
        get {
            return collider;
        }
    }

    /// <summary>
    /// Returns or sets the current velocity.
    /// </summary>
    public Vector2f Velocity {
        get { return velocity; }
        set { velocity = value; }
    }

    /// <summary>
    /// Returns the current position.
    /// </summary>
    public Vector2f Position {
        get { return position; }
    }

    /// <summary>
    /// Generates a Vector3 using linear interpolation from the previous position to the current one, 
    /// in order to generate a smooth transition in the rendering of the physics steps.
    /// </summary>
    /// <returns>Vector3 interpolated</returns>
    public Vector3 InterpolatedPosition() {
        Vector3 previous = prevPosition.ToVector3();
        Vector3 current = position.ToVector3();
        float a = PhysicsEngine.alpha;
        return (current * a + previous * (1f - a));
    }

    /// <summary>
    /// Moves the body and updates the position of the collider.
    /// </summary>
    /// <param name="translation">amount of movement to apply</param>
    public void Transform(Vector2f translation) {
        Vector2f difference = position - prevPosition;
        prevPosition = position;
        collider.Transform(difference);
        position += translation;
    }

    /// <summary>
    /// Returns the mass.
    /// </summary>
    public intf Mass {
        get { return this.mass; }
    }

    /// <summary>
    /// Returns the inverse mass, calculated as 1/mass.
    /// </summary>
    public intf InvMass {
        get { return this.invMass; }
    }

    /// <summary>
    /// Returns the current restitution.
    /// </summary>
    public intf Restitution {
        get { return this.restitution; }
    }

    /// <summary>
    /// Returns the current applied force on this body.
    /// </summary>
    public Vector2f Force {
        get { return this.force; }
    }

    /// <summary>
    /// Applies a force to the object. The given amount is added to the total amount.
    /// </summary>
    /// <param name="force">vector representing a force</param>
    public void AddForce(Vector2f force) {
        this.force += force;
    }

    /// <summary>
    /// Resets the forces applied to this body.
    /// </summary>
    public void ClearForces() {
        this.force = Vector2f.Zero;
    }

    /// <summary>
    /// Sets the unique identifier for this object.
    /// </summary>
    /// <param name="identifier">the id</param>
    public void SetID(int identifier) {
        this.identifier = identifier;
    }

    /// <summary>
    /// Checks whether the object is fixed or not (mass = 0).
    /// </summary>
    /// <returns>true if the mass is i0, false otherwise.</returns>
    public bool IsFixed() {
        return mass == 0;
    }

    /// <summary>
    /// Checks whether this object is active or not.
    /// </summary>
    /// <returns>the value of sleeping</returns>
    public bool IsSleeping() {
        return this.sleeping;
    }

    /// <summary>
    /// Compares the current object to the given one, using the ids.
    /// </summary>
    /// <param name="other">generic object.</param>
    /// <returns> the result of the calculation this.ID - other.ID</returns>
    public int CompareTo(DBody other) {
        return identifier - other.identifier;
    }

    /// <summary>
    /// Returns the hash of the object (the identifier).
    /// </summary>
    /// <returns>the identifier</returns>
    public override int GetHashCode() {
        return identifier;
    }

    /// <summary>
    /// Checks whether the goven object is a rigid body, comparing
    /// the identifiers in that case.
    /// </summary>
    /// <param name="obj">the generic object</param>
    /// <returns>true if the object is a rigid body and the identifiers match, false otherwise</returns>
    public override bool Equals(object obj) {
        if (obj is DBody)
            return ((DBody)obj).identifier == identifier;
        return false;
    }

    /// <summary>
    /// Returns a string containing the ID and the collider data.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return "P.O. ID: " + identifier + "[Collider: " + collider+"]";
    }
}
