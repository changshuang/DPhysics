using UnityEngine;
using FixedPointMath;

public class CircleComponent : ColliderComponent {

    public Vector2 center;
    public float radius;

    void OnDrawGizmos() {
        if (radius <= 0 || !drawCollider)
            return;
        Gizmos.color = Color.cyan;
        Vector3 pos = new Vector3(center.x, 0, center.y) + transform.position;
        Gizmos.DrawWireSphere(pos, radius);
    }

    public override DCollider RequireCollider() {
        Vector2 global = new Vector2(transform.position.x, transform.position.z);
        Vector2f ctr = (Vector2f)(global + center);
        return new DCircleCollider(ctr, intf.Create(radius), isTrigger);
    }
}
