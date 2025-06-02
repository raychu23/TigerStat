using UnityEngine;
using System.Collections;

public class AnimControl : MonoBehaviour
{
    public Animation anim;
    private static AnimControl obj;

    void Start()
    {
        obj = this;
        anim = this.GetComponent<Animation>();
    }

    public static AnimControl This()
    {
        if (obj == null)
            Debug.Log("BOOOOOOOOOOOOM");
        return obj;
    }

    public void PlayAnim(string animName)
    {
        if (!ShouldWeAnimate(animName))
            return;

        switch (animName)
        {
            case "fire":
                anim.Play(animName);
                break;
            case "notes":
                anim.CrossFade(animName);
                break;
            default:
                anim.CrossFade(animName);
                break;
        }
    }

    public void AreWeMoving(bool moving, bool sprinting)
    {
        // dont update movement if we are zoomed in
        if (InputControl.zoomState == 1 || anim.IsPlaying("fire") || anim.IsPlaying("zoomOut"))
            return;

        if (moving)
        {
            PlayAnim("run");

            if (sprinting)
                anim.GetComponent<Animation>()["run"].speed = 2f;
            else
                anim.GetComponent<Animation>()["run"].speed = 1f;
        }

        else
        {
            PlayAnim("idle");
        }
    }

    private bool ShouldWeAnimate(string animName)
    {
        // no animating if we are taking notes
        if (anim.IsPlaying("notes"))
            return false;

        if (animName == "run")
            if (anim.IsPlaying("run"))
                return false;

        if (animName == "idle")
            if (anim.IsPlaying("idle"))
                return false;

        return true;
    }
}