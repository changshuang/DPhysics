using UnityEngine;
using System.Collections;
using FixedPointMath;

public class PhysicsComponent : MonoBehaviour {

    public float speed;

    public float mass;
    public float restitution;
    public float friction;

    private ColliderComponent colliderComponent;
    private PhysicsObject physicsObject;

    void Start() {
        this.colliderComponent = GetComponent<ColliderComponent>();
        physicsObject = new PhysicsObject(
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
	
	// Update is called once per frame
	void Update () {
        this.transform.position = physicsObject.Position.ToVector3();
	}
}
