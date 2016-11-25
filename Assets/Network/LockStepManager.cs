using UnityEngine;
using System.Collections;
using FixedPointMath;

public class LockStepManager : MonoBehaviour {

    private PhysicsEngine physics;

    //game update frequency in ms
    private const int rate = 50;

    private float accumulator;
    private float fixedDelta;
    private intf fixedDeltaF;

    void Awake() {
        this.accumulator = 0;
        this.fixedDelta = rate / (float)1000; 
        this.fixedDeltaF = intf.Create(fixedDelta);
        print(fixedDelta);
    }

	void Start () {
        this.physics = PhysicsEngine.Instance;
	}
	
	// Updates the physics
	void Update () {
        float delta = Time.deltaTime;
        if (delta > 0.25f)
            delta = 0.25f;
        this.accumulator += delta;

        while (accumulator >= fixedDelta) {
            physics.Step(fixedDeltaF);
            accumulator -= fixedDelta;
        }
        PhysicsEngine.alpha = accumulator / fixedDelta;
	}
}
