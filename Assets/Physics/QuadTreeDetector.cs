using UnityEngine;
using System.Collections.Generic;
using FixedPointMath;

public class QuadTreeDetector : CollisionDetector{

    private QuadTree<PhysicsObject> tree;
    private int extension;

    public QuadTreeDetector(int maxSize, int minSize) {
        Debug.Log("size:" + maxSize + " leaf size: " + minSize);
        tree = new QuadTree<PhysicsObject>(minSize, maxSize, Vector2f.Zero);
    }

    public void Insert(PhysicsObject obj) {
        tree.Insert(obj);
    }

    public void Remove(PhysicsObject obj) {
        tree.Remove(obj);
    }

    public HashSet<Intersection> FindPotentialCollisions() {
        HashSet<Intersection> collisionSet = new HashSet<Intersection>();
        tree.FindCollisions(collisionSet);
        return collisionSet;
    }

    public void Draw() {
        tree.Draw();
    }
}
