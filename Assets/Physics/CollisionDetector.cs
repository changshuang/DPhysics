using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface CollisionDetector {
    void Insert(PhysicsObject obj);

    void Remove(PhysicsObject obj);

    void Draw();

    HashSet<Intersection> FindPotentialCollisions();
}