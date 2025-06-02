using UnityEngine;
using System.Collections;

public class TargetControl : MonoBehaviour
{
    private ParticleSystem particles;

    void Start()
    {
        particles = this.GetComponentInChildren<ParticleSystem>();
    }


    void ApplyDamage()
    {
        particles.enableEmission = false;
        TutorialArrowControl.TargetHit();
        TutorialAppearControl.TargetHit();
    }
}
