using FixedPointMath;

public class EulerImplicit : IIntegrator {

    public EulerImplicit() { }

    public void Integrate(DBody body, intf delta) {
        body.Velocity += (PhysicsEngine.GRAVITY + body.Force * body.InvMass) * delta * 0.5f;
        body.Transform(body.Velocity * delta);
        body.ClearForces();
    }

}
