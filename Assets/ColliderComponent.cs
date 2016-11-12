using UnityEngine;
using System.Collections;

public abstract class ColliderComponent : MonoBehaviour {

    public bool drawCollider = true;
    public bool isTrigger = false;

    public abstract DCollider RequireCollider();
}
