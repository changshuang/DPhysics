using FixedPointMath;

/// <summary>
/// 2D collider representing a rectangle, using fixed point math. It is defined by the minimum and
/// maximum edges, located respectively on the bottom left and top right.
/// </summary>
public class DBoxCollider : DCollider {

    private Vector2f min;
    private Vector2f max;

    /// <summary>
    /// Creates a new box collider with the given dimensions.
    /// </summary>
    /// <param name="min">te minimum edge</param>
    /// <param name="max">the maximum edge</param>
    public DBoxCollider(Vector2f min, Vector2f max, bool isTrigger) : base(ColliderType.Box, isTrigger) {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Returns the minimum edge.
    /// </summary>
    public Vector2f Min {
        get { return this.min; }
    }

    /// <summary>
    /// Returns the maximum edge.
    /// </summary>
    public Vector2f Max {
        get { return this.max; }
    }

    /// <summary>
    /// Gets the collider's position, intended as the bottom left edge.
    /// </summary>
    /// <returns>The minimum edge</returns>
    public override Vector2f GetPosition() {
        return this.min;
    }

    /// <summary>
    /// Returns the current object as the bounding box.
    /// </summary>
    /// <returns>The current bounding box.</returns>
    public override DBoxCollider GetContainer() {
        return this;
    }

    /// <summary>
    /// Gets a vector representing the extension of this collider on the x and y axis.
    /// </summary>
    /// <returns>The extents of the collider</returns>
    public Vector2f GetExtents() {
        return new Vector2f(max.x - min.x, max.y - min.y);
    }

    /// <summary>
    /// Transforms the collider's position by the given amount.
    /// </summary>
    /// <param name="translation">Vector indicating the translation</param>
    public override void Transform(Vector2f translation) {
        this.min += translation;
        this.max += translation;
    }

    /// <summary>
    /// Checks whether the given colliders intersect, generating an Intersection instance
    /// with the collision data in that case.
    /// Since this function represents a "box vs circle" collision, the symmetric function is called
    /// instead. See DCircleCollider for more info.
    /// </summary>
    /// <param name="other">the second collider</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns></returns>
    public override bool Intersects(DCircleCollider other, out Manifold intersection) {
        return other.Intersects(this, out intersection);
    }

    /// <summary>
    /// Checks whether the two boxes collide, generating an intersection containing the collision data.
    /// This function can only compare bounding boxes againsts other bounding boxes.
    /// </summary>
    /// <param name="other">bounding box to check</param>
    /// <param name="intersection">the collision data, <code>null</code> if no collision has been detected.</param>
    /// <returns></returns>
    public override bool Intersects(DBoxCollider other, out Manifold intersection) {
        intersection = null;

        //check if one of them is a trigger
        if (this.IsTrigger || other.IsTrigger) {
            intersection = new Manifold(this.Body, other.Body);
            return true;
        }

        // Vector from A to B
        Vector2f distance = other.Body.Position - Body.Position;

        // Calculate half extents along x axis for each object
        intf xEntentA = (max.x - min.x) / 2;
        intf xExtentB = (other.max.x - other.min.x) / 2;

        // Calculate overlap on x axis
        intf offsetX = xEntentA + xExtentB - FixedMath.Abs(distance.x);

        // SAT test on x axis
        if (offsetX > 0) {
            // Calculate half extents along x axis for each object
            intf yExtentA = (max.y - min.y) / 2;
            intf yExtentB = (other.max.y - other.min.y) / 2;

            // Calculate overlap on y axis
            intf offsetY = yExtentA + yExtentB - FixedMath.Abs(distance.y);

            // SAT test on y axis
            if (offsetY > 0) {
                Vector2f n;
                // Find out which axis is axis of least penetration
                if (offsetX < offsetY) {
                    // Point towards B knowing that n points from A to B
                    if (distance.x < 0)
                        n = new Vector2f(-1, 0);
                    else
                        n = new Vector2f(1, 0);
                    intersection = new Manifold(Body, other.Body, n, offsetX);
                    return true;
                }
                else {
                    // Point toward B knowing that n points from A to B
                    if (distance.y < 0)
                        n = new Vector2f(0, -1);
                    else
                        n = new Vector2f(0, 1);
                    intersection = new Manifold(Body, other.Body, n, offsetY);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Simple function used to check whether the given point lies inside the collider's
    /// boundaries.
    /// </summary>
    /// <param name="point">Vector representing a point</param>
    /// <returns>true if the point is inside the collider, false otherwise.</returns>
    public bool Contains(Vector2f point) {
        return (min.x <= point.x && point.x <= max.x &&
                min.y <= point.y && point.x <= max.y);
    }

    /// <summary>
    /// Returns a string containing the min and max vectors.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return "Box collider - min: " + min + ", max: " + max;
    }
}
