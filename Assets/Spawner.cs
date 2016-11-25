using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject circle;
    public GameObject square;
    public Camera camera;
    public int instances;

	// Use this for initialization
	void Start () {
        int size = PhysicsEngine.Instance.sceneWidth;
        int range = size - 50;
        for (int i = 0; i < instances; i++) {
            Instantiate(circle, new Vector3(Random.value * range+25, 0, Random.value * range+25), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos.y = 0;
            Instantiate(circle, pos, Quaternion.identity);
        }

        if (Input.GetMouseButton(1)) {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            pos.y = 0;
            Instantiate(square, pos, Quaternion.identity);
        }
    }
}
