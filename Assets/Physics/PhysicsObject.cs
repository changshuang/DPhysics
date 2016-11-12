using FixedPointMath;

/// <summary>
/// Class representing a rigid body inside the physics simulation
/// </summary>
public class PhysicsObject {

    private DCollider collider;
    private Vector2f position;
    private Vector2f oldPosition;
    private Vector2f velocity;
    private intf mass;
    private intf invMass;
    private intf restitution;
    private intf friction;
    private int identifier;

    /// <summary>
    /// Creates a new rigid body with the given parameters.
    /// </summary>
    /// <param name="collider">the collider for this object</param>
    /// <param name="position">the current position in the space</param>
    /// <param name="mass">the object's mass</param>
    /// <param name="restitution">the "bounciness"</param>
    /// <param name="friction">amount of friction</param>
    public PhysicsObject(DCollider collider, Vector2f position, intf mass, intf restitution, intf friction) {
        this.collider = collider;
        this.position = position;
        this.oldPosition = position;
        this.mass = mass;
        this.invMass = (mass > 0) ? ((intf)1 / mass) : (intf)0;
        this.restitution = restitution;
        this.friction = friction;
        this.collider.Body = this;
    }

    /// <summary>
    /// Returns the current collider, transofrming the position to the current one.
    /// </summary>
    public DCollider Collider {
        get {
            Vector2f translation = position - oldPosition;
            oldPosition = position;
            collider.Transform(translation);
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
    /// Calculates the new position, using the current velocity.
    /// </summary>
    /// <param name="frames">number of frames for the current timestep</param>
    public void Integrate(int frames) {
        position += velocity / frames;
    }

    /// <summary>
    /// Compares the current object to the given one, using the ids.
    /// </summary>
    /// <param name="other">generic object.</param>
    /// <returns> the result of the calculation this.ID - other.ID</returns>
    public int CompareTo(PhysicsObject other) {
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
        if (obj is PhysicsObject)
            return ((PhysicsObject)obj).identifier == identifier;
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
