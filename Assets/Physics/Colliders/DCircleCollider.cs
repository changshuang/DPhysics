using FixedMath;

/// <summary>
/// Class representing a 2D circular collider, using fixed point math.
/// </summary>
public class DCircleCollider : DCollider {
    private Vector2F center;
    private Fix32 radius;
    private DBoxCollider boundingBox;

    /// <summary>
    /// Creates a new circle collider with the given position and radius.
    /// </summary>
    /// <param name="position">the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    public DCircleCollider(Vector2F position, Fix32 radius, bool isTrigger) : base(ColliderType.Circle, isTrigger){
        this.center = position;
        this.radius = radius;
        Vector2F min = center - Vector2F.One * radius;
        Vector2F max = center + Vector2F.One * radius;
        boundingBox = new DBoxCollider(min, max, isTrigger);
    }

    /// <summary>
    /// Returns this radius.
    /// </summary>
    public Fix32 Radius {
        get { return this.radius; }
    }

    /// <summary>
    /// Returns a bounding box containing the circle, used to quickly compare with other boxes.
    /// </summary>
    public DBoxCollider BoundingBox {
        get { return this.boundingBox; }
    }
    
    /// <summary>
    /// Gets the current center position.
    /// </summary>
    /// <returns>The center of the circle</returns>
    public override Vector2F GetPosition() {
        return this.center;
    }

    /// <summary>
    /// Returns the minimum bounding box containing the current object.
    /// </summary>
    /// <returns>a box collider containing the current object</returns>
    public override DBoxCollider GetContainer() {
        return this.boundingBox;
    }

    /// <summary>
    /// Transforms the position of the circle by the given amount, 
    /// together wuth the corresponding bounding box.
    /// </summary>
    /// <param name="translation">vector representing the translation.</param>
    public override void Transform(Vector2F translation) {
        this.center += translation;
        this.boundingBox.Transform(translation);
    }

    /// <summary>
    /// Checks whether the given box collider intersects with this circle, calculating
    /// the collision data in that case.
    /// </summary>
    /// <param name="other">box collider to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns>true if the colliders intersect, false otherwise.</returns>
    public override bool Intersects(DBoxCollider other, out Manifold intersection) {
        intersection = null;

        Vector2F halfExtents = other.GetExtents() / 2;
        Vector2F boxCenter = other.Min + halfExtents;

        Vector2F difference = center - boxCenter;
        Vector2F clamped = Vector2F.Clamp(difference, -halfExtents, halfExtents);
        Vector2F closest = boxCenter + clamped;
        difference = closest - center;

        if (difference.SqrtMagnitude > radius*radius)
            return false;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        Fix32 dist = difference.Magnitude;
        Fix32 penetration;
        Vector2F normal;
        if (dist > Fix32.Zero) {
            penetration = radius - dist;
            normal = difference / dist;
        }
        else {
            penetration = radius;
            normal = new Vector2F(1, 0);
        }
        intersection = new Manifold(this.Body, other.Body, normal, penetration);
        return true;
    }

    /// <summary>
    /// Checks whether the given circle intersects with this circle, calculating the 
    /// collision data in that case.
    /// </summary>
    /// <param name="other">the circle to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns>true if the colliders intersect, false otherwise.</returns>
    public override bool Intersects(DCircleCollider other, out Manifold intersection) {
        intersection = null;
        Fix32 rDistance = this.radius + other.radius;
        Fix32 sqrRadiusDistance = rDistance * rDistance;
        Vector2F centerDistance = other.center - this.center;
        if (centerDistance.SqrtMagnitude > sqrRadiusDistance)
            return false;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        Fix32 distance = centerDistance.Magnitude;
        Vector2F normal;
        Fix32 penetration;
        if (distance > Fix32.Zero) {
            penetration = rDistance - distance;
            normal = centerDistance / distance;
        }
        else {
            penetration = this.radius;
            normal = new Vector2F((Fix32)1, (Fix32)0);
        }
        intersection = new Manifold(this.Body, other.Body, normal, penetration);
        return true;
    }

    /// <summary>
    /// REturns a string containing the center and the radius of this collider.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return "Circle collider - center: " + center + ", radius: " + radius;
    }
}
