using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using FixedPointMath;

/// <summary>
/// Collision detector using spatial hash grids.
/// </summary>
public class HashGridDetector : ICollisionDetector {

    private int cellSize;
    private Cell[,] cells;
    private int rows;
    private int cols;

    /// <summary>
    /// Creates a new detector with the given cell size, height and width.
    /// </summary>
    /// <param name="cellSize">size of a single cell</param>
    /// <param name="sceneWidth">total scene width</param>
    /// <param name="sceneHeight">total scene height</param>
    public HashGridDetector(int cellSize, int sceneWidth, int sceneHeight) {
        this.cellSize = cellSize;
        this.rows = sceneWidth / cellSize;
        this.cols = sceneHeight / cellSize;
        cells = new Cell[rows, cols];
    }

    /// <summary>
    /// Inserts a new object into the detector.
    /// </summary>
    /// <param name="obj">the object</param>
    public void Insert(DBody obj) {
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

    /// <summary>
    /// Removes the given object from the grid.
    /// </summary>
    /// <param name="obj">the rigid body</param>
    public void Remove(DBody obj) {
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

    /// <summary>
    /// Returns a set of collisions, iterating through each active cell.
    /// </summary>
    /// <returns>set of collisions</returns>
    public HashSet<Manifold> FindPotentialCollisions() {
        HashSet<Manifold> collisionSet = new HashSet<Manifold>();
        foreach (Cell c in cells) {
            if (c != null && c.Active) {
                c.FindCollisions(collisionSet);
            }
        }
        return collisionSet;
    }

    /// <summary>
    /// Draws the grid using gizmos. Blue indicates a free cell, red an occupied one.
    /// </summary>
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

    /// <summary>
    /// Returns the coordinates of the cell containing the given point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private Coord Hash(Vector2f point) {
        int x = (int)(point.x / cellSize);
        int y = (int)(point.y / cellSize);
        return new Coord(x, y);
    }

    /// <summary>
    /// Checks whether the given coordinate lies inside the grid bounds.
    /// </summary>
    /// <param name="coord">the coordinates to check</param>
    /// <returns></returns>
    private bool IsInsideBounds(Coord coord) {
        return coord.x >= 0 && coord.x < rows &&
                coord.y >= 0 && coord.y < cols;
    }
    
    /// <summary>
    /// Adds a new object to the bucket of the cell in the given coordinates.
    /// </summary>
    /// <param name="obj">object to insert</param>
    /// <param name="x">line of the cell</param>
    /// <param name="y">column of the cell</param>
    private void Insert(DBody obj, int x, int y) {
        if (cells[x, y] == null)
            cells[x, y] = new Cell();
        cells[x, y].Insert(obj);
    }

    /// <summary>
    /// REmoves the object from the bucket of the cell in the given coordinates.
    /// </summary>
    /// <param name="obj">object to remove</param>
    /// <param name="x">line of the cell</param>
    /// <param name="y">column of the cell</param>
    private void Remove(DBody obj, int x, int y) {
        if (cells[x, y] == null)
            return;
        cells[x, y].Remove(obj);
    }
}

/// <summary>
/// Structure for interger coordinates.
/// </summary>
public struct Coord {
    public int x;
    public int y;

    public Coord(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

/// <summary>
/// Class defining a single grid cell.
/// </summary>
public class Cell {

    private List<DBody> bucket;

    /// <summary>
    /// Creates a new cell, initializing the bucket list.
    /// </summary>
    public Cell() {
        this.bucket = new List<DBody>();
    }

    /// <summary>
    /// Checks whether the current cell contains objects or not.
    /// </summary>
    public bool Active {
        get { return bucket.Count > 0; }
    }

    /// <summary>
    /// Inserts a new object into the bucket list.
    /// </summary>
    /// <param name="obj">the object</param>
    public void Insert(DBody obj) {
        this.bucket.Add(obj);
    }

    /// <summary>
    /// Removes the given object from the bucket list.
    /// </summary>
    /// <param name="obj">the object</param>
    public bool Remove(DBody obj) {
        return this.bucket.Remove(obj);
    }

    /// <summary>
    /// Creates a set of all the collisions found inside the cell.
    /// </summary>
    /// <param name="intersections">hash set of intersections</param>
    public void FindCollisions(HashSet<Manifold> intersections) {
        if (!Active || bucket.Count < 2)
            return;

        for (int i = 0; i < bucket.Count; i++) {
            for (int j = i+1; j < bucket.Count; j++) {
                Manifold intersection;
                if (bucket[i].Collider.Intersects(bucket[j].Collider, out intersection)) {
                    intersections.Add(intersection);
                }
            }
        }
    }
}
