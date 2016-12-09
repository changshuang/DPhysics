using FixedMath;

/// <summary>
/// Class representing a collider manifold, with all the info about the collision.
/// </summary>
public class Manifold {

    private DBody bodyA;
    private DBody bodyB;
    private Vector2F normal;
    private Fix32 penetration;
    private Fix32 restitution;
    private Fix32 totalInvMass;
    private Fix32 bias;

    private int hash;
    private bool trigger;

    /// <summary>
    /// Creates a new collision manifold between the given objects.
    /// </summary>
    /// <param name="a">first rigid body</param>
    /// <param name="b">second rigid body</param>
    /// <param name="normal">collision normal</param>
    /// <param name="distance">penetration</param>
    public Manifold(DBody a, DBody b, Vector2F normal, Fix32 penetration)
    {
        this.bodyA = a;
        this.bodyB = b;
        this.normal = normal;
        this.penetration = penetration;
        this.trigger = false;
        GenerateHash();
    }

    /// <summary>
    /// Constructor used for trigger collisions (no collision data required)
    /// </summary>
    /// <param name="a">first object</param>
    /// <param name="b">second object</param>
    public Manifold(DBody a, DBody b) {
        this.bodyA = a;
        this.bodyB = b;
        this.trigger = true;
        GenerateHash();
    }

    /// <summary>
    /// Returns the collision normal
    /// </summary>
    public Vector2F Normal {
        get { return this.normal; }
    }

    /// <summary>
    /// Returns the penetration.
    /// </summary>
    public Fix32 Distance {
        get { return this.penetration; }
    }

    /// <summary>
    /// Returns the first rigid body.
    /// </summary>
    /// <returns>the first body</returns>
    public DBody GetA() {
        return this.bodyA;
    }

    /// <summary>
    /// Returns the second rigid body.
    /// </summary>
    /// <returns>the second body</returns>
    public DBody GetB() {
        return this.bodyB;
    }

    /// <summary>
    /// Checks whether the current manifold is a trigger or not.
    /// </summary>
    /// <returns>true if it's a trigger, false otherwise.</returns>
    public bool IsTrigger() {
        return this.trigger;
    }

    public void Init(Fix32 invDelta) {
        totalInvMass = bodyA.InvMass + bodyB.InvMass;
        bias = -DWorld.PENETRATION_CORRECTION * invDelta * Fix32.Min((Fix32)0, -penetration + DWorld.PENETRATION_SLOP);
        restitution = Fix32.Min(bodyA.Restitution, bodyB.Restitution);
    }

    /// <summary>
    /// Resolve the collision by calculating the resulting velocity, using the
    /// given normal and penetration, stored in the intersection.
    /// </summary>
    /// <param name="collision">Intersection instance containing all the collision data.</param>
    public void ApplyImpulse() {
        Vector2F dv = bodyB.Velocity - bodyA.Velocity;
        Fix32 vn = Vector2F.Dot(dv, normal);

        Fix32 imp = ((-((Fix32)1 +restitution) * vn + bias)) / totalInvMass;
        imp = Fix32.Max(imp, (Fix32)0);

        Vector2F impulse = normal * imp;
        bodyA.Velocity -= impulse * bodyA.InvMass;
        bodyB.Velocity += impulse * bodyB.InvMass;
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
            return ((bodyA.Equals(other.bodyA) && bodyB.Equals(other.bodyB)) ||
                    (bodyA.Equals(other.bodyB) && bodyB.Equals(other.bodyB)));
        }
        return false;
    }

    public override string ToString() {
        return "Collision <" + bodyA + ", " + bodyB + ">";
    }

    /// <summary>
    /// Combines the two identifiers using a variant of the Szudzik's function, removing the chance for duplicate
    /// symmetric pairs. The relatively small number of physics bodies inside the scene allows the use of integers.
    /// </summary>
    private void GenerateHash() {
        int hashA = bodyA.GetHashCode();
        int hashB = bodyB.GetHashCode();
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
