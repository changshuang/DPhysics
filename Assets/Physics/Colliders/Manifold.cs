using FixedPointMath;

/// <summary>
/// Class representing a collider manifold, with all the info about the collision.
/// </summary>
public class Manifold {

    private DBody objA;
    private DBody objB;
    private Vector2f normal;
    private intf distance;
    private int hash;
    private bool trigger;

    /// <summary>
    /// Creates a new collision manifold between the given objects.
    /// </summary>
    /// <param name="a">first rigid body</param>
    /// <param name="b">second rigid body</param>
    /// <param name="normal">collision normal</param>
    /// <param name="distance">penetration</param>
    public Manifold(DBody a, DBody b, Vector2f normal, intf distance)
    {
        this.objA = a;
        this.objB = b;
        this.normal = normal;
        this.distance = distance;
        this.trigger = false;
        GenerateHash();
    }

    /// <summary>
    /// Constructor used for trigger collisions (no collision data required)
    /// </summary>
    /// <param name="a">first object</param>
    /// <param name="b">second object</param>
    public Manifold(DBody a, DBody b) {
        this.objA = a;
        this.objB = b;
        this.trigger = true;
        GenerateHash();
    }

    /// <summary>
    /// Returns the collision normal
    /// </summary>
    public Vector2f Normal {
        get { return this.normal; }
    }

    /// <summary>
    /// Returns the penetration.
    /// </summary>
    public intf Distance {
        get { return this.distance; }
    }

    /// <summary>
    /// Returns the first rigid body.
    /// </summary>
    /// <returns>the first body</returns>
    public DBody GetA() {
        return this.objA;
    }

    /// <summary>
    /// Returns the second rigid body.
    /// </summary>
    /// <returns>the second body</returns>
    public DBody GetB() {
        return this.objB;
    }

    /// <summary>
    /// Checks whether the current manifold is a trigger or not.
    /// </summary>
    /// <returns>true if it's a trigger, false otherwise.</returns>
    public bool IsTrigger() {
        return this.trigger;
    }

    /// <summary>
    /// Resolve the collision by calculating the resulting velocity, using the
    /// given normal and penetration, stored in the intersection.
    /// </summary>
    /// <param name="collision">Intersection instance containing all the collision data.</param>
    public void ApplyImpulse() {

        //both objects have infinite mass, return
        if (objA.Mass + objB.Mass == 0) {
            objA.Velocity = Vector2f.Zero;
            objB.Velocity = Vector2f.Zero;
            return;
        }

        Vector2f rv = objB.Velocity - objA.Velocity;
        intf normalVel = Vector2f.Dot(rv, normal);

        if (normalVel > 0)
            return;

        intf e = FixedMath.Min(objA.Restitution, objB.Restitution);
        intf j = (-(1 + e) * normalVel) / (objA.InvMass + objB.InvMass);

        Vector2f impulse = normal * j;
        objA.Velocity -= impulse * objA.InvMass;
        objB.Velocity += impulse * objB.InvMass;
    }

    /// <summary>
    /// Corrects the position by a small percentage, with a small amount of tolerance.
    /// </summary>
    public void CorrectPosition() {
        intf totInvMass = objA.InvMass + objB.InvMass;
        intf penetration = FixedMath.Max(distance - PhysicsEngine.PENETRATION_SLOP, (intf)0);
        Vector2f corr = normal * (penetration / totInvMass) * PhysicsEngine.PENETRATION_CORRECTION;

        objA.Transform(-corr * objA.InvMass);
        objB.Transform(corr * objB.InvMass);
    }

    /// <summary>
    /// Returns the intersection hash code.
    /// </summary>
    /// <returns>integer representing a unique pair (excluding symmetrical couples)</returns>
    public override int GetHashCode() {
        return hash;
    }

    /// <summary>
    /// Checks whether the given intersection pair contains the same pair of objects.
    /// </summary>
    /// <param name="obj">the object to be compared-</param>
    /// <returns>true if they both contain the same physics objects, false otherwise</returns>
    public override bool Equals(object obj) {
        if (obj is Manifold) {
            Manifold other = (Manifold)obj;
            return ((objA.Equals(other.objA) && objB.Equals(other.objB)) ||
                    (objA.Equals(other.objB) && objB.Equals(other.objB)));
        }
        return false;
    }

    public override string ToString() {
        return "Collision <" + objA + ", " + objB + ">";
    }

    /// <summary>
    /// Combines the two identifiers using a variant of the Szudzik's function, removing the chance for duplicate
    /// symmetric pairs. The relatively small number of physics bodies inside the scene allows the use of integers.
    /// </summary>
    private void GenerateHash() {
        int hashA = objA.GetHashCode();
        int hashB = objB.GetHashCode();
        int a, b;
        if (hashA >= hashB) {
            a = hashA;
            b = hashB;
        }
        else {
            a = hashB;
            b = hashA;
        }
        hash = a * a + a + b;
    }
}
