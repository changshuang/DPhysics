using UnityEngine;
using System.Collections;
using FixedPointMath;
using System;

public class DPlaneCollider : DCollider {

    private Vector2f normal;
    private intf distance;

    public DPlaneCollider(Vector2f normal, intf distance, bool isTrigger) : base(ColliderType.Plane,isTrigger) {
        this.normal = normal;
        this.distance = distance;
    }
    public override DBoxCollider GetContainer() {
        throw new NotImplementedException();
    }

    public override Vector2f GetPosition() {
        throw new NotImplementedException();
    }

    public override bool Intersects(DBoxCollider other, out Intersection intersection) {
        throw new NotImplementedException();
    }

    public override bool Intersects(DCircleCollider other, out Intersection intersection) {
        throw new NotImplementedException();
    }

    public override void Transform(Vector2f translation) {
        throw new NotImplementedException();
    }
}
