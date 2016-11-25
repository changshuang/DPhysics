using UnityEngine;
using System.Collections;

public interface IIntegrator {
    void Integrate(DBody body, int frames);
}
