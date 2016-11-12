using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using FixedPointMath;

public class HashGridDetector : CollisionDetector {

    private int cellSize;
    private Cell[,] cells;
    private int sceneWidth;
    private int sceneHeight;
    private int rows;
    private int cols;

    public HashGridDetector(int cellSize, int sceneWidth, int sceneHeight) {
        this.cellSize = cellSize;
        this.sceneWidth = sceneWidth;
        this.sceneHeight = sceneHeight;
        this.rows = sceneWidth / cellSize;
        this.cols = sceneHeight / cellSize;
        cells = new Cell[rows, cols];
    }

    public void Insert(PhysicsObject obj) {
        DBoxCollider box = obj.Collider.GetContainer();
        Coord min = Hash(box.Min);
        Coord max = Hash(box.Max);
        if (!IsInsideBounds(min) || !IsInsideBounds(max))
            return;

        for (int i = min.x; i <= max.x; i++) {
            for (int j = min.y; j <= max.y; j++) {
                Insert(obj, i, j);
            }
        }
    }

    public void Remove(PhysicsObject obj) {
        DBoxCollider box = obj.Collider.GetContainer();
        Coord min = Hash(box.Min);
        Coord max = Hash(box.Max);
        if (!IsInsideBounds(min) || !IsInsideBounds(max))
            return;

        for (int i = min.x; i <= max.x; i++) {
            for (int j = min.y; j <= max.y; j++) {
                Remove(obj, i, j);
            }
        }
    }

    public HashSet<Intersection> FindPotentialCollisions() {
        HashSet<Intersection> collisionSet = new HashSet<Intersection>();
        foreach (Cell c in cells) {
            if (c != null && c.Active) {
                c.FindCollisions(collisionSet);
            }
        }
        return collisionSet;
    }

    public void Draw() {
        if (cells == null)
            return;

        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < cols; y++) {
                Gizmos.color = (cells[x,y] == null || !cells[x,y].Active) ? Color.blue : Color.red;
                Vector3 center = new Vector3(x*cellSize + cellSize/2, 0.5f, y*cellSize + cellSize/2);
                Gizmos.DrawWireCube(center, Vector3.one * cellSize);
            }
        }
    }

    private Coord Hash(Vector2f point) {
        int x = (int)(point.x / cellSize);
        int y = (int)(point.y / cellSize);
        return new Coord(x, y);
    }

    private bool IsInsideBounds(Coord coord) {
        return coord.x >= 0 && coord.x < rows &&
                coord.y >= 0 && coord.y < cols;
    }
    
    private void Insert(PhysicsObject obj, int x, int y) {
        if (cells[x, y] == null)
            cells[x, y] = new Cell();
        cells[x, y].Insert(obj);
    }

    private void Remove(PhysicsObject obj, int x, int y) {
        if (cells[x, y] == null)
            return;
        cells[x, y].Remove(obj);
    }
}

public struct Coord {
    public int x;
    public int y;

    public Coord(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public class Cell {

    private List<PhysicsObject> bucket;

    public Cell() {
        this.bucket = new List<PhysicsObject>();
    }

    public bool Active {
        get { return bucket.Count > 0; }
    }

    public void Insert(PhysicsObject obj) {
        this.bucket.Add(obj);
    }

    public bool Remove(PhysicsObject obj) {
        return this.bucket.Remove(obj);
    }

    public void FindCollisions(HashSet<Intersection> intersections) {
        if (!Active || bucket.Count < 2)
            return;

        for (int i = 0; i < bucket.Count; i++) {
            for (int j = i+1; j < bucket.Count; j++) {
                Intersection intersection;
                if (bucket[i].Collider.Intersects(bucket[j].Collider, out intersection)) {
                    intersections.Add(intersection);
                }
            }
        }
    }
}
