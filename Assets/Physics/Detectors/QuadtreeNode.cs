using FixedMath;
using System;
using System.Collections.Generic;

/// <summary>
/// Generic class for quadtree nodes.
/// </summary>
/// <typeparam name="T">type inheriting from DBody</typeparam>
public class QuadtreeNode<T> where T : DBody {

    private int size;
    private Vector2F position;
    private List<T> objects;

    public QuadtreeNode<T> parent;
    public QuadtreeNode<T>[] children;

    public QuadtreeNode(int size, Vector2F position, QuadtreeNode<T> parent) {
        this.size = size;
        this.position = position;
        this.parent = parent;
        this.children = new QuadtreeNode<T>[4];
    }

    public int Size {
        get { return this.size; }
    }

    public Vector2F Position {
        get { return this.position; }
    }

    public Vector2F Center {
        get {
            return this.position + Vector2F.One * (size / 2);
        }
    }

    public List<T> Objects {
        get { return this.objects; }
    }

    public bool IsLeaf() {
        return children[0] == null;
    }

    public bool IsEmpty() {
        return (objects == null) ? true : objects.Count == 0;
    }

    public void AddObject(T value) {
        if (!this.IsLeaf()) {
            throw new InvalidOperationException("Cannot add object to non-leaf node!");
        }

        if (objects == null) {
            objects = new List<T>();
        }
        objects.Add(value);
    }

    public bool Intersects(DCollider collider) {
        DBoxCollider bbox = (collider.Type == ColliderType.Box) ? (DBoxCollider)collider : ((DCircleCollider)collider).BoundingBox;
        Vector2F min = position;
        Vector2F max = position + Vector2F.One * size;

        if (max.x < bbox.Min.x || min.x > bbox.Max.x) return false;
        if (max.y < bbox.Min.y || min.y > bbox.Max.y) return false;
        return true;
    }

    public bool Remove(T value) {
        return objects.Remove(value);
    }

    public void GetCollisions(HashSet<Manifold> collisionSet) {
        if (IsEmpty() || objects.Count < 2)
            return;

        for (int i = 0; i < objects.Count; i++) {
            for (int j = i + 1; j < objects.Count; j++) {
                Manifold intersection;
                if (objects[i].Collider.Intersects(objects[j].Collider, out intersection)) {
                    collisionSet.Add(intersection);
                }
            }
        }
    }
}
