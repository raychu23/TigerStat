// Script originated from TwiiK on the unity forums
// http://forum.unity3d.com/threads/20069-Third-person-shooter-Learning-project/page4
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed = 500.0f;
    public float life = 5;
    public GameObject impactEffect = null;
    public GameObject bulletHole;
    public int damage = 20;
    public float impactForce = 10;
	public float fallSpeed = 1f;

    private Vector3 velocity;
    private Vector3 newPos;
    private Vector3 oldPos;
    private bool hasHit = false;

    void Start()
    {
        newPos = transform.position;
        oldPos = newPos;
        velocity = speed * transform.forward;

        // schedule for destruction if bullet never hits anything
        Destroy(gameObject, life);
    }

    void Update()
    {
        if (hasHit)
            return;

		// update our velocity
		velocity += fallSpeed * Time.deltaTime * -transform.up;

        // assume we move all the way
        newPos += velocity * Time.deltaTime;

        // Check if we hit anything on the way
        Vector3 direction = newPos - oldPos;
        float distance = direction.magnitude;

        if (distance > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(oldPos, direction, out hit, distance))
            {
                //Debug.Log(Time.time);
                // adjust new position
                newPos = hit.point;

                // notify hit
                hasHit = true;

                //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                //Instantiate(impactEffect, hit.point, rotation);

                // if we have impact from bullets (probably wont)
                //if (hit.rigidbody)
                //    hit.rigidbody.AddForce(transform.forward * impactForce, ForceMode.Impulse);

                //// if we hit geometry we spawn a bullet hole (probably wont)
                //if (hit.transform.tag == "Geometry")
                //{
                //    GameObject instantiatedBulletHole = Instantiate(bulletHole, hit.point, rotation) as GameObject;
                //}

                hit.transform.SendMessageUpwards("ApplyDamage", SendMessageOptions.DontRequireReceiver);

                Destroy(gameObject, 1);
            }
        }

        oldPos = transform.position;
        transform.position = newPos;
    }
}