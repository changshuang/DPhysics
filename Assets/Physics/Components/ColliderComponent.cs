using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract class defining a generic collider component.
/// </summary>
public abstract class ColliderComponent : MonoBehaviour {

    public bool drawCollider = true;
    public bool isTrigger = false;

    public abstract DCollider RequireCollider();
}
