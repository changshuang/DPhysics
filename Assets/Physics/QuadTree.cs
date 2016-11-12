using UnityEngine;
using System.Collections.Generic;
using FixedPointMath;

public class QuadTree<T> where T : PhysicsObject {

    private static Vector2f[] OFFSET = new Vector2f[] {
        new Vector2f(0, 0),
        new Vector2f(0, 1),
        new Vector2f(1, 0),
        new Vector2f(1, 1)
    };

    private QuadtreeNode<T> root;
    private int minSize;

    public QuadTree(int minSize, int maxSize, Vector2f origin) {
        this.minSize = minSize;
        root = new QuadtreeNode<T>(maxSize, origin, null);
    }

    public int Size {
        get { return root.Size; }
    }

    public Vector2f Origin {
        get { return root.Position; }
    }

    public void Insert(T value) {
        if (value != null) {
            Insert(root, value);
        }
    }

    public void Remove(T value) {
        if (value != null) {
            Remove(root, value);
        }
    }

    public void FindCollisions(HashSet<Intersection> collisionSet) {
        FindCollisions(root, collisionSet);
    }

    private void Remove(QuadtreeNode<T> node, T value) {
        if (node == null)
            return;

        if (!node.Intersects(value.Collider))
            return;

        if (!node.IsLeaf()) {
            foreach (QuadtreeNode<T> child in node.children)
                Remove(child, value);
        }
        else {
            if (node.Remove(value)) {
                if (node.IsEmpty())
                    Collapse(node);
            }
        }
    }

    private void Insert(QuadtreeNode<T> node, T value) {
        if (node == null || !node.Intersects(value.Collider))
            return;

        if (node.Size <= minSize) {
            node.AddObject(value);
        }
        else {
            if (node.IsLeaf())
                Split(node);

            for (int i = 0; i < 4; i++) {
                Insert(node.children[i], value);
            }
        }
    }

    private void FindCollisions(QuadtreeNode<T> node, HashSet<Intersection> collisionSet) {
        if (node == null)
            return;

        if (node.IsLeaf()) {
            node.GetCollisions(collisionSet);
        }
        else {
            foreach (QuadtreeNode<T> child in node.children)
                FindCollisions(child, collisionSet);
        }
    }

    private void Collapse(QuadtreeNode<T> node) {
        if (node.IsLeaf()) {
            if (node.parent != null && node.IsEmpty())
                Collapse(node.parent);
        }
        else {
            bool empty = true;
            for (int i = 0; i < 4 && empty; i++) {
                if (!node.children[i].IsLeaf() || !node.children[i].IsEmpty())
                    empty = false;
            }
            if (empty) {
                node.children = new QuadtreeNode<T>[4];
                if (node.parent != null) {
                    Collapse(node.parent);
                }
            }
        }

    }

    private void Split(QuadtreeNode<T> node) {
        int childSize = node.Size / 2;
        for (int i = 0; i < 4; i++) {
            Vector2f childPosition = node.Position + OFFSET[i] * childSize;
            node.children[i] = new QuadtreeNode<T>(childSize, childPosition, node);
        }
    }

    public void Draw() {
        Draw(root);
    }

    private void Draw(QuadtreeNode<T> node) {
        if (node == null)
            return;

        Gizmos.color = (node.Objects != null) ? Color.red : Color.blue;

        Vector3 pos = node.Center.ToVector3();
        float size = node.Size;
        Gizmos.DrawWireCube(pos, new Vector3(size, 1, size));
        
        for (int i=0; i<4; i++) {
            Draw(node.children[i]);
        }
    }
}
