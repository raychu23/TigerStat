using UnityEngine;
using System.Collections;

/// <summary>
/// Things I need!
/// States: idle, walking unalarmed, alarmed of hunter presence, attacking hunter, "dead"
/// walk: pick random direction, walk for a period of time.
///     stop when period of time is done.  pause for a period of time
///     continue in a new random direction
/// Alarmed to hunters presence
///     Hunter is within "warning range"
///     Look at hunter and "growl"
/// Attacking hunter
///     hunter has "shot and missed" near tiger while tiger was alarmed
///     makes direct route to hunter.
///     claws at hunter
/// "dead" mode
///     Happens when tiger is shot (1 shot.  doesnt matter where)
///     Plays death animation
///     Doesn't do anything else after this
/// </summary>
public class TigerAI : MonoBehaviour
{
    public bool easyMode;

    public enum tigerState { Idle, Walking, Alarmed, Attacking, DataPending, Dead }
    public tigerState curState;

    private float walkSpeed;
    private float runSpeed;
    private float warningRange;
    private float attackRange;
    private Vector2 walkTimeRange;
    private Vector2 idleTimeRange;
    float walkAngleRange;

    private float t1;
    private float t2;
    private float angle;

    private TerrainData tData;
    private TigerAnim anim;
    public int tigerNum, spawnNum;

    private TigerTag tigerTag;
    public GameObject tagObj;

    void Start()
    {
        // get terrain data (We do this in a coroutine in case the data isnt immediately available
        tData = transform.parent.GetComponent<TigerControl>().ourTerrainData;
        anim = gameObject.GetComponent<TigerAnim>();
        //StartCoroutine(WaitForTerrainData());

        idleTimeRange = new Vector2(2, 6);
        walkTimeRange = new Vector2(3, 14);
        walkAngleRange = 100;
        walkSpeed = 1.3f;
        runSpeed = 9f;

        // THESE NUMBERS NEED TESTING
        warningRange = 50f;
        attackRange = 20f;

        // start our tiger idle
        StartCoroutine("Idle");

        // dont start this coroutine if they are on "easy mode"
        if (!easyMode)
            StartCoroutine("DistanceCheck");
    }

    // not currently used
    private IEnumerator WaitForTerrainData()
    {
        while (transform.parent == null)
        {
            yield return false;
        }

        tData = transform.parent.GetComponent<TigerControl>().ourTerrainData;
    }

    private float timeIdle;

    // pick random amount of time within range.
    // once time is over, walk in a direction
    private IEnumerator Idle()
    {
        curState = tigerState.Idle;

        anim.Play("idle");
        timeIdle = Random.Range(idleTimeRange.x, idleTimeRange.y);

        yield return new WaitForSeconds(timeIdle);

        StartCoroutine("Walk");
    }

    private float direction, timeToWalk, timecatch;

    private IEnumerator Walk()
    {
        anim.Play("walk");
        curState = tigerState.Walking;

        // pick a direction and a duration to walk in taht direction
        // direction is within "walkAngleRange" degrees from current heading.  No 180 turnarounds
        direction = Random.Range(transform.localEulerAngles.y - walkAngleRange, transform.localEulerAngles.y + walkAngleRange);
        timeToWalk = Random.Range(walkTimeRange.x, walkTimeRange.y);
        timecatch = Time.time;

        while (Time.time - timecatch < timeToWalk)
        {
            RotateTo(direction, 2f);
            MoveTo(walkSpeed);
            yield return false;
        }

        StartCoroutine("Idle");
    }

    private void RotateTo(float dir, float turnSpeed)
    {
        this.transform.localEulerAngles = new Vector3(
            this.transform.localEulerAngles.x,
            Mathf.LerpAngle(transform.localEulerAngles.y, dir, Time.deltaTime * turnSpeed),
            this.transform.localEulerAngles.z);
    }

    private void MoveTo(float speed)
    {
        this.transform.localPosition = new Vector3(
            this.transform.localPosition.x + transform.forward.x * Time.deltaTime * speed,
            tData.GetInterpolatedHeight(Mathf.InverseLerp(0, 2000, transform.position.x), Mathf.InverseLerp(0, 2000, transform.position.z)),
            this.transform.localPosition.z + transform.forward.z * Time.deltaTime * speed);
    }

    private void ApplyDamage()
    {
        // if we are already dead, ignore
        // this probably doesnt happen ebcause we remove rigid bodies
        // i dont think it will detect a hit again
        if (curState == tigerState.Dead || curState == tigerState.DataPending)
            return;

        anim.Play("sleep"); // put tiger to "sleep"
        curState = tigerState.DataPending; // set tiger to have data to be collected
        //this.GetComponentInChildren<Billboard>().DataToCollect(); // turn on our instruction billboard
        this.GetComponentInChildren<TigerTag>().DataToCollect(); // turn on our instruction billboard
        PlayerData.ReportHit();

        foreach (BoxCollider foo in this.GetComponentsInChildren<BoxCollider>())
        {
            Destroy(foo.gameObject);
        }
        //Destroy(this.GetComponent<Rigidbody>()); // no more rigidbody since our tiger is out of the picture
        //GetComponent<BoxCollider>().enabled = false; // turn off our collider.  we dont need it anymore

        StopAllCoroutines();
    }

    private IEnumerator DistanceCheck()
    {
        // while hunter is outside of warning range, chill
		while (Vector3.Distance(transform.position, Camera.main.transform.position) > warningRange)
        {
            // adding 1 second between checks so as to help with performance
            yield return new WaitForSeconds(1);
        }

        // hunter is in warning range!  switch to alarm mode!
        StartCoroutine("Alarmed");
    }

    private IEnumerator Alarmed()
    {
        StopCoroutine("Walk");
        StopCoroutine("Idle");
        curState = tigerState.Alarmed;
        anim.Play("growl");

        float distance;

        // play animation for growling and face camera

        while (curState == tigerState.Alarmed)
        {
            RotateTo(GetAngle(), 6f);
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);

            // if hunter gets closer (within attackrange) switch to attack mode!
            if (distance < attackRange)
                StartCoroutine("Attack");

            // if hunter gets further away (outside of warning range), go back to idle
            if (distance > warningRange)
            {
                StartCoroutine("Idle");
                StartCoroutine("DistanceCheck");
            }

            yield return true;
        }
    }

    private float GetAngle()
    {
        t1 = transform.position.x - Camera.main.transform.position.x;
		t2 = transform.position.z - Camera.main.transform.position.z;

        angle = Mathf.Atan(t1 / t2) * Mathf.Rad2Deg;

        if (t2 > 0)
            return angle += 180;

        return angle;
    }

    private float dist;

    private IEnumerator Attack()
    {
        curState = tigerState.Attacking;
        //Attack!

		while (Vector3.Distance(this.transform.position, Camera.main.transform.position) < attackRange)
        {
            // look at player
            RotateTo(GetAngle(), 6f);

			dist = Vector3.Distance(this.transform.position, Camera.main.transform.position);

            // if we are too far away, run at the player
            if (dist > 3.5f)
            {
                MoveTo(runSpeed);
                anim.Play("run");
            }

            // if we are within a distance, "attack" player
            else
            {
                HUD2.This().BattleDamage();
                anim.Play("attack");
            }

            yield return false;
        }

        // we are outside of attack range, back to warning
        StartCoroutine("Alarmed");
    }
}