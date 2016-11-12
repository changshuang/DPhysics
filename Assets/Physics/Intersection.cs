using UnityEngine;
using System.Collections;
using FixedPointMath;

public class Intersection {

    private PhysicsObject objA;
    private PhysicsObject objB;
    private Vector2f normal;
    private intf distance;
    private int hash;
    private bool trigger;

    public Intersection(PhysicsObject a, PhysicsObject b, Vector2f normal, intf distance)
    {
        this.objA = a;
        this.objB = b;
        this.normal = normal;
        this.distance = distance;
        this.trigger = false;
        GenerateHash();
    }

    public Intersection(PhysicsObject a, PhysicsObject b) {
        this.objA = a;
        this.objB = b;
        this.trigger = true;
        GenerateHash();
    }

    public Vector2f Normal {
        get { return this.normal; }
    }

    public intf Distance {
        get { return this.distance; }
    }

    public PhysicsObject GetA() {
        return this.objA;
    }

    public PhysicsObject GetB() {
        return this.objB;
    }

    public bool IsTrigger() {
        return this.trigger;
    }

    /// <summary>
    /// Combine the two identifiers using a variant to the Szudzik's function, removing the chance for duplicate
    /// symmetric pairs. The relatively small number of physics bodies inside the scene allows the use of integers.
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
        if (obj is Intersection) {
            Intersection other = (Intersection)obj;
            return ((objA.Equals(other.objA) && objB.Equals(other.objB)) ||
                    (objA.Equals(other.objB) && objB.Equals(other.objB)));
        }
        return false;
    }

    public override string ToString() {
        return "Collision <" + objA + ", " + objB + ">";
    }

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
