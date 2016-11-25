using UnityEngine;
using FixedPointMath;

/// <summary>
/// Monobehaviour component used to define a new physics object.
/// </summary>
public class DBodyComponent : MonoBehaviour {

    public float speed;

    public float mass;
    public float restitution;
    public float friction;

    private ColliderComponent colliderComponent;
    private DBody physicsObject;

    //TODO: remove this temporary code
    void Start() {
        this.colliderComponent = GetComponent<ColliderComponent>();
        physicsObject = new DBody(
            colliderComponent.RequireCollider(),
            new Vector2f(transform.position),
            intf.Create(mass),
            intf.Create(restitution),
            intf.Create(friction)
            );
        float x = Random.Range(-.5f, .5f);
        float z = Random.Range(-.5f, .5f);
        
        Vector3 dir = (Random.value > .5f) ? new Vector3(x, 0, z).normalized : Vector3.zero;
        physicsObject.Velocity = new Vector2f(dir) * speed;
        PhysicsEngine.Instance.AddObject(physicsObject);
    }
	
	/// <summary>
    /// Updates the position of the gameObject by using the physics object data.
    /// </summary>
	void Update () {
        if (!physicsObject.IsSleeping() && !physicsObject.IsFixed())
            this.transform.position = physicsObject.InterpolatedPosition();
	}

    void OnDrawGizmos() {
        /*if (physicsObject == null)
            return;
        Gizmos.color = (physicsObject.IsSleeping()) ? Color.green : Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one * 2);*/
    }
}
