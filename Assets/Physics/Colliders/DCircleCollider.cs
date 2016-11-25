using FixedPointMath;

/// <summary>
/// Class representing a 2D circular collider, using fixed point math.
/// </summary>
public class DCircleCollider : DCollider {
    private Vector2f center;
    private intf radius;
    private DBoxCollider boundingBox;

    /// <summary>
    /// Creates a new circle collider with the given position and radius.
    /// </summary>
    /// <param name="position">the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    public DCircleCollider(Vector2f position, intf radius, bool isTrigger) : base(ColliderType.Circle, isTrigger){
        this.center = position;
        this.radius = radius;
        Vector2f min = center - Vector2f.One * radius;
        Vector2f max = center + Vector2f.One * radius;
        boundingBox = new DBoxCollider(min, max, isTrigger);
    }

    /// <summary>
    /// Returns this radius.
    /// </summary>
    public intf Radius {
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
    public override Vector2f GetPosition() {
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
    public override void Transform(Vector2f translation) {
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

        Vector2f halfExtents = other.GetExtents() / 2;
        Vector2f boxCenter = other.Min + halfExtents;

        Vector2f difference = center - boxCenter;
        Vector2f clamped = Vector2f.Clamp(difference, -halfExtents, halfExtents);
        Vector2f closest = boxCenter + clamped;
        difference = closest - center;

        if (difference.SqrtMagnitude > radius*radius)
            return false;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        intf dist = difference.Magnitude;
        intf penetration;
        Vector2f normal;
        if (dist > 0) {
            penetration = radius - dist;
            normal = difference / dist;
        }
        else {
            penetration = radius;
            normal = new Vector2f(1, 0);
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
        intf rDistance = this.radius + other.radius;
        intf sqrRadiusDistance = rDistance * rDistance;
        Vector2f centerDistance = other.center - this.center;
        if (centerDistance.SqrtMagnitude > sqrRadiusDistance)
            return false;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        intf distance = centerDistance.Magnitude;
        Vector2f normal;
        intf penetration;
        if (distance != 0) {
            penetration = rDistance - distance;
            normal = centerDistance / distance;
        }
        else {
            penetration = this.radius;
            normal = new Vector2f((intf)1, (intf)0);
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
