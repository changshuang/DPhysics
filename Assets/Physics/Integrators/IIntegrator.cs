using FixedMath;

public interface IIntegrator {
    void IntegrateForces(DBody body, Fix32 delta);
    void IntegrateVelocities(DBody body, Fix32 delta);
}
