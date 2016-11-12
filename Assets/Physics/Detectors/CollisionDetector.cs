using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for collision detectors.
/// </summary>
public interface CollisionDetector {

    /// <summary>
    /// Adds a new object to the physics simulation.
    /// </summary>
    /// <param name="obj">the object</param>
    void Insert(PhysicsObject obj);

    /// <summary>
    /// Removes an object from the simulation.
    /// </summary>
    /// <param name="obj">the object</param>
    void Remove(PhysicsObject obj);

    /// <summary>
    /// Draws the current structure using gizmos.
    /// </summary>
    void Draw();

    /// <summary>
    /// Gets all the collisions inside the structure.
    /// </summary>
    /// <returns>hash set containing the collision data</returns>
    HashSet<Intersection> FindPotentialCollisions();
}