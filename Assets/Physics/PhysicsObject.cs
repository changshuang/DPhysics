using FixedPointMath;

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

    public DCollider Collider {
        get {
            Vector2f translation = position - oldPosition;
            oldPosition = position;
            collider.Transform(translation);
            return collider;
        }
    }

    public Vector2f Velocity {
        get { return velocity; }
        set { velocity = value; }
    }

    public Vector2f Position {
        get { return position; }
    }

    public intf Mass {
        get { return this.mass; }
    }

    public intf InvMass {
        get { return this.invMass; }
    }

    public intf Restitution {
        get { return this.restitution; }
    }

    public void SetID(int identifier) {
        this.identifier = identifier;
    }

    public bool IsFixed() {
        return mass == 0;
    }

    public void Integrate(int frames) {
        position += velocity / frames;
        intf mu = friction / mass;
    }

    public int CompareTo(PhysicsObject other) {
        return identifier - other.identifier;
    }

    public override int GetHashCode() {
        return identifier;
    }

    public override bool Equals(object obj) {
        if (obj is PhysicsObject)
            return ((PhysicsObject)obj).identifier == identifier;
        return false;
    }

    public override string ToString() {
        return "P.O. ID: " + identifier + "[Collider: " + collider+"]";
    }
}
