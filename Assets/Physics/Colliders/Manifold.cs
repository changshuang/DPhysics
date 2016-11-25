﻿using FixedPointMath;

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
