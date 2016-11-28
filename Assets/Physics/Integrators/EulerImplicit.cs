using FixedMath;

public class EulerImplicit : IIntegrator {

    public EulerImplicit() { }

    public void IntegrateForces(DBody body, Fix32 delta) {
        body.Velocity += (DWorld.GRAVITY + body.Force * body.InvMass) * delta * (Fix32.One - body.Friction);
    }

    public void IntegrateVelocities(DBody body, Fix32 delta) {
        body.Transform(body.Velocity * delta);
        body.ClearForces();
    }
}
