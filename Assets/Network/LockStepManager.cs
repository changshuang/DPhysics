using UnityEngine;
using System.Collections;
using FixedPointMath;

public class LockStepManager : MonoBehaviour {

    private PhysicsEngine physics;

    //game update frequency in ms
    private const int rate = 50;

    private int accumulator;
    private int gameFrameSec;

    void Awake() {
        this.accumulator = 0;
        this.gameFrameSec = 1000/rate;
        print(gameFrameSec);
    }

	void Start () {
        this.physics = PhysicsEngine.Instance;
	}
	
	// Updates the physics
	void Update () {
        int delta = (int)(Time.deltaTime * 1000);
        if (delta > 250)
            delta = 250;
        this.accumulator += delta;

        while (accumulator >= rate) {
            physics.UpdatePhysics(gameFrameSec);
            accumulator -= rate;
        }
       PhysicsEngine.alpha = accumulator / (float)rate;
	}
}
