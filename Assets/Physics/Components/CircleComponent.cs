using UnityEngine;
using FixedMath;

/// <summary>
/// Monobehaviour component for circle colliders.
/// </summary>
public class CircleComponent : ColliderComponent {

    public Vector2 center;
    public float radius;

    /// <summary>
    /// Draws the collider as a sphere.
    /// </summary>
    void OnDrawGizmos() {
        if (radius <= 0 || !drawCollider)
            return;
        Gizmos.color = Color.cyan;
        Vector3 pos = new Vector3(center.x, 0, center.y) + transform.position;
        Gizmos.DrawWireSphere(pos, radius);
    }

    /// <summary>
    /// Generates the "real" collider transforming the current data into fixed point data.
    /// </summary>
    /// <returns>a deterministic circle collider</returns>
    public override DCollider RequireCollider() {
        Vector2 global = new Vector2(transform.position.x, transform.position.z);
        Vector2F ctr = (Vector2F)(global + center);
        return new DCircleCollider(ctr, (Fix32)radius, isTrigger);
    }
}
