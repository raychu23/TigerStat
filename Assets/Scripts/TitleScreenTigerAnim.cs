using UnityEngine;
using System.Collections;

public class TitleScreenTigerAnim : MonoBehaviour {
    Animation anim;

	void Start () {
        anim = this.GetComponent<Animation>();
        anim["idleState"].speed = .5f;
    }
}
