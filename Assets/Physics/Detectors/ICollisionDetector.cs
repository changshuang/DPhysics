using System.Collections.Generic;

/// <summary>
/// Interface for collision detectors.
/// </summary>
public interface ICollisionDetector {

    /// <summary>
    /// Adds a new object to the physics simulation.
    /// </summary>
    /// <param name="obj">the object</param>
    void Insert(DBody obj);

    /// <summary>
    /// Removes an object from the simulation.
    /// </summary>
    /// <param name="obj">the object</param>
    void Remove(DBody obj);

    /// <summary>
    /// Inserts every item sotred inside the given list of bodies inside the grid.
    /// </summary>
    /// <param name="bodies">list of physics bodies</param>
    void Build(List<DBody> bodies);

    /// <summary>
    /// Completely resets the current detector.
    /// </summary>
    void Clear();

    /// <summary>
    /// Draws the current structure using gizmos.
    /// </summary>
    void Draw();

    /// <summary>
    /// Gets all the collisions inside the structure.
    /// </summary>
    /// <returns>hash set containing the collision data</returns>
    void FindCollisions(HashSet<Manifold> contacts);
}