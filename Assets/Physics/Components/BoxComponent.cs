using UnityEngine;
using FixedMath;

/// <summary>
/// Monobehaviour component for a box collider.
/// </summary>
public class BoxComponent : ColliderComponent {

    public Vector2 min;
    public Vector2 max;

    /// <summary>
    /// Draws the collider.
    /// </summary>
    void OnDrawGizmos() {
        if (min == max || !drawCollider)
            return;
        Vector3 position = transform.position;

        Vector3 aa = new Vector3(min.x, 0, min.y) + position;
        Vector3 ab = new Vector3(min.x, 0, max.y) + position;
        Vector3 ba = new Vector3(max.x, 0, min.y) + position;
        Vector3 bb = new Vector3(max.x, 0, max.y) + position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(aa, ab);
        Gizmos.DrawLine(ab, bb);
        Gizmos.DrawLine(bb, ba);
        Gizmos.DrawLine(ba, aa);
    }

    /// <summary>
    /// Generates the "real" collider transforming the current data into fixed point data.
    /// </summary>
    /// <returns>a deterministic collider</returns>
    public override DCollider RequireCollider() {
        Vector2 global = new Vector2(transform.position.x, transform.position.z);
        Vector2F fmin = (Vector2F)(min + global);
        Vector2F fmax = (Vector2F)(max + global);
        return new DBoxCollider(fmin, fmax, isTrigger);
    }
}
