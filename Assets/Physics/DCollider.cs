using FixedPointMath;

public enum ColliderType {
    Box,
    Circle
}

public abstract class DCollider {

    private readonly ColliderType type;
    private PhysicsObject body;
    private bool isTrigger;

    public DCollider(ColliderType type, bool isTrigger) {
        this.type = type;
        this.isTrigger = isTrigger;
    }

    public DCollider(ColliderType type, PhysicsObject body, bool isTrigger) {
        this.type = type;
        this.body = body;
        this.isTrigger = isTrigger;
    }

    public ColliderType Type {
        get { return type; }
    }

    public PhysicsObject Body {
        get { return body; }
        set { this.body = value; }
    }

    public bool IsTrigger {
        get { return isTrigger; }
        set { this.isTrigger = value; }
    }

    public bool Intersects(DCollider other, out Intersection intersection) {
        switch (other.Type) {
            case ColliderType.Box:
                return Intersects((DBoxCollider)other, out intersection);
            default:
                return Intersects((DCircleCollider)other, out intersection);
        }
    }

    public abstract Vector2f GetPosition();

    public abstract DBoxCollider GetContainer();

    public abstract void Transform(Vector2f translation);

    public abstract bool Intersects(DCircleCollider other, out Intersection intersection);

    public abstract bool Intersects(DBoxCollider other, out Intersection intersection);
}
