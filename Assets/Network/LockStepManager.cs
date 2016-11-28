using UnityEngine;
using FixedMath;

public class LockStepManager : MonoBehaviour {

    private DWorld physics;

    //game update frequency in ms
    private const int rate = 50;

    private Fix32 accumulator;
    private Fix32 fixedDelta;

    void Awake() {
        this.accumulator = Fix32.Zero;
        this.fixedDelta = (Fix32)(rate / (float)1000); 
    }

	void Start () {
        this.physics = DWorld.Instance;
	}
	
	// Updates the physics
	void Update () {
        Fix32 delta = (Fix32)Time.deltaTime;
        if (delta > (Fix32)0.25f)
            delta = (Fix32)0.25f;
        this.accumulator += delta;

        while (accumulator >= fixedDelta) {
            physics.Step(fixedDelta);
            accumulator -= fixedDelta;
        }
        DWorld.alpha = (float)(accumulator / fixedDelta);
	}
}
