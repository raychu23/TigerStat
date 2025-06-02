using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    static ParticleSystem particle;

    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
    }

    public static void Fire()
    {
        particle.Emit(1);
    }
}