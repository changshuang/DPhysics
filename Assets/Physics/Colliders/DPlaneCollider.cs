using FixedMath;
using System;

public class DPlaneCollider : DCollider {

    private Vector2F normal;
    private Fix32 distance;

    public DPlaneCollider(Vector2F normal, Fix32 distance, bool isTrigger) : base(ColliderType.Plane,isTrigger) {
        this.normal = normal;
        this.distance = distance;
    }
    public override DBoxCollider GetContainer() {
        throw new NotImplementedException();
    }

    public override Vector2F GetPosition() {
        throw new NotImplementedException();
    }

    public override bool Intersects(DBoxCollider other, out Manifold intersection) {
        throw new NotImplementedException();
    }

    public override bool Intersects(DCircleCollider other, out Manifold intersection) {
        throw new NotImplementedException();
    }

    public override void Transform(Vector2F translation) {
        throw new NotImplementedException();
    }
}
