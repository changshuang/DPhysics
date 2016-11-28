using FixedMath;

/// <summary>
/// Class representing a generic collider. Every new collider must inherit from this class.
/// </summary>
public abstract class DCollider {

    private readonly ColliderType type;
    private DBody body;
    private bool isTrigger;

    /// <summary>
    /// Creates a new collider of the given type.
    /// </summary>
    /// <param name="type">collider type</param>
    /// <param name="isTrigger">true if it is a trigger, false otherwise</param>
    public DCollider(ColliderType type, bool isTrigger) {
        this.type = type;
        this.isTrigger = isTrigger;
    }

    /// <summary>
    /// Creates a new collider of the given type, linking it to a specified physics object.
    /// </summary>
    /// <param name="type">type of the collider</param>
    /// <param name="body">rigid body</param>
    /// <param name="isTrigger">true if this is a trigger, false otherwise</param>
    public DCollider(ColliderType type, DBody body, bool isTrigger) {
        this.type = type;
        this.body = body;
        this.isTrigger = isTrigger;
    }

    /// <summary>
    /// Returns the type of this collider.
    /// </summary>
    public ColliderType Type {
        get { return type; }
    }

    /// <summary>
    /// Returns the body of this collider.
    /// </summary>
    public DBody Body {
        get { return body; }
        set { this.body = value; }
    }

    /// <summary>
    /// Returns true if this is a trigger.
    /// </summary>
    public bool IsTrigger {
        get { return isTrigger; }
        set { this.isTrigger = value; }
    }

    /// <summary>
    /// Checks whether the current collider intersects with the given one.
    /// This function subdivides the call of the specific functions, based on the type of the colliders.
    /// </summary>
    /// <param name="other">other collider.</param>
    /// <param name="intersection">result of the collision, if any.</param>
    /// <returns></returns>
    public bool Intersects(DCollider other, out Manifold intersection) {
        switch (other.Type) {
            case ColliderType.Box:
                return Intersects((DBoxCollider)other, out intersection);
            default:
                return Intersects((DCircleCollider)other, out intersection);
        }
    }


    /// <summary>
    /// Abstract function to get the position of the collider.
    /// </summary>
    /// <returns>The position of the collider.</returns>
    public abstract Vector2F GetPosition();

    /// <summary>
    /// Gets the minimum bounding box for this collider.
    /// if this is already a bounding box, it returns itself.
    /// </summary>
    /// <returns>The minimum bounding box.</returns>
    public abstract DBoxCollider GetContainer();

    /// <summary>
    /// Transforms the current position using the given amount.
    /// </summary>
    /// <param name="translation">the translation.</param>
    public abstract void Transform(Vector2F translation);

    /// <summary>
    /// Abstract function for intersections with circles.
    /// </summary>
    /// <param name="other">circle collider</param>
    /// <param name="intersection">collision data</param>
    /// <returns></returns>
    public abstract bool Intersects(DCircleCollider other, out Manifold intersection);

    /// <summary>
    /// Abstract function for intersections with boxes.
    /// </summary>
    /// <param name="other">box collider.</param>
    /// <param name="intersection">collision data.</param>
    /// <returns></returns>
    public abstract bool Intersects(DBoxCollider other, out Manifold intersection);
}
