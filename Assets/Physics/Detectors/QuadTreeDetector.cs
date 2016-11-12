using UnityEngine;
using System.Collections.Generic;
using FixedPointMath;

/// <summary>
/// Collision detector using a quadtree for spatial subdivision.
/// </summary>
public class QuadTreeDetector : CollisionDetector{

    private QuadTree<PhysicsObject> tree;
    private int extension;

    /// <summary>
    /// Creates a new detector, initializing the tree root.
    /// </summary>
    /// <param name="maxSize">maximum tree extent (square)</param>
    /// <param name="minSize">minimum node size</param>
    public QuadTreeDetector(int maxSize, int minSize) {
        Debug.Log("size:" + maxSize + " leaf size: " + minSize);
        tree = new QuadTree<PhysicsObject>(minSize, maxSize, Vector2f.Zero);
    }

    /// <summary>
    /// Inserts the given object inside the tree.
    /// </summary>
    /// <param name="obj">the object</param>
    public void Insert(PhysicsObject obj) {
        tree.Insert(obj);
    }

    /// <summary>
    /// Removes the given object from the tree.
    /// </summary>
    /// <param name="obj">the object</param>
    public void Remove(PhysicsObject obj) {
        tree.Remove(obj);
    }

    /// <summary>
    /// Gets a set of all the collisions in the structure.
    /// </summary>
    /// <returns>hash set of intersections</returns>
    public HashSet<Intersection> FindPotentialCollisions() {
        HashSet<Intersection> collisionSet = new HashSet<Intersection>();
        tree.FindCollisions(collisionSet);
        return collisionSet;
    }

    /// <summary>
    /// Draws the current tree structure.
    /// </summary>
    public void Draw() {
        tree.Draw();
    }
}
