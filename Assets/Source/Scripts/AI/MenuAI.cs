using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAI : MonoBehaviour
{
    Vector2 CurrentWaypointPos;

    private Animator anim;
    public Vector3 dir;
    public float speed = 1;
    public float WaypointSwitch = 5;
    public float WaypointWait = 5;
    void Start()
    {
        gameObject.SetActive(true);
        anim = GetComponent<Animator>();
        int rand = Random.Range(0, 2);

        anim.SetBool("Change", rand == 1);

        StartCoroutine(GetNextWaypoint());

    }

    IEnumerator GetNextWaypoint()
    {
        CurrentWaypointPos = Waypoints.Instance.GetRandom();
        yield return new WaitForSeconds(WaypointSwitch);

        if (HasReachedWaypoint())
        {
            yield return new WaitForSeconds(WaypointWait);
            CurrentWaypointPos = Waypoints.Instance.GetRandom();
        }
        StartCoroutine(GetNextWaypoint());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    bool HasReachedWaypoint()
    {
        if (Vector2.Distance(transform.position, CurrentWaypointPos) < 1)//is near to target
        {
            return true;//waypoint has been reached
        }
        else
        {
            return false;//waypoint has not been reached
        }

    }
    private void Update()
    {
        if (HasReachedWaypoint())
        {
            anim.SetBool("isRunning", false);

        }
        else
        {
            anim.SetBool("isRunning", true);
            Vector2 move = Vector2.MoveTowards(transform.position, CurrentWaypointPos, speed * Time.deltaTime);
            //move towards next waypoint
            Vector3 move3 = move;
            Vector3 direction = transform.position - move3;
            transform.position = new Vector3(move.x, move.y, -1);


            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
            anim.SetFloat("speed", direction.sqrMagnitude);
        }

    }
}
