using UnityEngine;
using System.Collections;

public class TigerAnim : MonoBehaviour {

    Animation anim;
    void Awake()
    {
        anim = this.GetComponentInChildren<Animation>();
        anim.animatePhysics = false;
        anim.cullingType = AnimationCullingType.BasedOnRenderers;
    }

    public void Play(string name)
    {
        anim.CrossFade(name, .3f);
        //anim.Play(name,PlayMode.StopAll);
    }
}