using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour
{
    public static bool lockMovement;
    public static int zoomState;
    private bool firing;
    public GameObject zoomCam;
    public GameObject mainCam;
    public GameObject hunterCam;
    public GameObject bulletPrefab;
    public static bool isPaused;
    public GameObject firePos;

    CharacterMotor motor;

    void Start()
    {

        lockMovement = false;
        zoomState = 0;
        firing = false;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        motor = this.GetComponent<CharacterMotor>();
        StartCoroutine("StartThings");
    }

    private IEnumerator StartThings()
    {
        yield return new WaitForSeconds(.1f);
        StartCoroutine("InputCheck");
        StartCoroutine("DistanceReport");
    }

    private IEnumerator DistanceReport()
    {
        Vector3 pos1 = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 pos2;

        while (true)
        {
            pos2 = new Vector3(transform.position.x, 0, transform.position.z);
            PlayerData.ReportDistance(Vector3.Distance(pos1, pos2));
            pos1 = new Vector3(transform.position.x, 0, transform.position.z);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator InputCheck()
    {
        while (true)
        {
//			Debug.Log(isPaused + "  " + Time.time);
            if (HUD2.This().endState == 1)
            {
                // we reached the end of our current run. don't need this anymore!
                yield return false;
            }

            PauseCheck();

            if (!isPaused)
            {
                MoveCheck();

                if (!lockMovement)
                {
                    GunCheck();
                    DataGrabCheck();
                }
            }

            yield return 0;
        }
    }

    private void PauseCheck()
    {
        Cursor.visible = isPaused;
        if (Input.GetKeyDown(KeyCode.P))
            Pause();
    }

    public void Pause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        Time.timeScale = 1 - Time.timeScale;

        if (isPaused)
        {
            HUD2.This().Show();
        }
        else
        {
            HUD2.This().Hide();
        }
    }

    private void DataGrabCheck()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DataCheck.GrabbingData();
        }
    }

    private void GunCheck()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine("FireGun");

        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine("ScopeGun");
    }

    private IEnumerator FireGun()
    {
        // delay in seconds before we can shoot again
        float fireRate = .7f;

        if (!firing)
        {
            firing = true;
            GameObject.Instantiate(bulletPrefab, WhichObject().transform.position, mainCam.transform.rotation);
            PlayerData.ReportShot();

            if (zoomState != 1)
            {
                AnimControl.This().PlayAnim("fire");
                MuzzleFlash.Fire();
            }

            yield return new WaitForSeconds(fireRate);

            firing = false;
        }
    }

//	private Quaternion AddMuzzleSpray(Quaternion rot)
//	{
//		rot.to
//		rot += 
//	}

    private GameObject WhichObject()
    {
        if (zoomState != 1)
            return firePos;

        return mainCam;
    }

    private IEnumerator ScopeGun()
    {
        if (zoomState == 0)
        {
            zoomState = 1;
            AnimControl.This().PlayAnim("zoomIn");
            //float timeCatch = Time.time;

            while (AnimControl.This().anim.IsPlaying("zoomIn"))
            {
                yield return false;
            }

            //while (Time.time - timeCatch < .3f)
            //{
            //    yield return false;
            //}

            mainCam.GetComponent<Camera>().depth = -4;
            hunterCam.GetComponent<Camera>().depth = -3;

            HUD2.This().SetScope(true);
        }
        else
        {
            zoomState = 0;
            mainCam.GetComponent<Camera>().depth = 0;
            hunterCam.GetComponent<Camera>().depth = 1;
            AnimControl.This().PlayAnim("zoomOut");
            HUD2.This().SetScope(false);
        }
    }

    private void MoveCheck()
    {
        Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            AnimControl.This().AreWeMoving(true, Input.GetKey(KeyCode.LeftShift));

            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }
        else
            AnimControl.This().AreWeMoving(false, false);

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector * (zoomState == 1 ? .3f : 1f) * RunCheck();
        motor.inputJump = Input.GetButton("Jump");
    }

    private float RunCheck()
    {
        if (Input.GetKey(KeyCode.LeftShift) && zoomState == 0)
            return 2f;

        return 1f;
    }
}