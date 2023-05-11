using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINavigation : MonoBehaviour
{
    int currentWaypoint = -1;
    Vector2 CurrentWaypointPos;

    public float Speed=1;
    public float WaypointChangeDist = 1;
    public int TroopEscapeCost = 10;

    public float Health = 100;

    public bool shouldRotate;
    private Animator anim;
    public Vector3 dir;

    bool Stuned = false;
    public void Stun(float stunTime)//stun the AI
    {
        if (!Stuned)
        {
            StartCoroutine(StunCoolDown(stunTime));
        }
    }
    IEnumerator StunCoolDown(float stunTime)//stun timer
    {
        Stuned = true;
        yield return new WaitForSeconds(stunTime);
        Stuned = false;
    }
    private void OnDisable()//reset the AI ready for its next respawn
    {
        StopAllCoroutines();
        Stuned = false;
    }

    float stunModifier()//speed modifier for if the AI is stunned
    {
        if (Stuned)
        {
            return 0.5f;
        }
        else
        {
            return 1;
        }
    }
    public void Setup(float hp,Vector3 scale,float _speed)//setup the AI troop
    {
        Speed = _speed;
        transform.localScale = scale;
        Health = hp;
        gameObject.SetActive(true);
        anim = GetComponent<Animator>();
        int rand = Random.Range(0, 2);//random skin colour

        anim.SetBool("Change", rand == 1);

        anim.SetBool("isRunning", true);
        //set navigation starting point
        currentWaypoint = 0;
        CurrentWaypointPos = Waypoints.Instance.GetWaypointPosition(currentWaypoint);
        
    }

    void NextWaypoint()
    {
        currentWaypoint++;
        if (Waypoints.Instance.IsAtEnd(currentWaypoint)){//has the ai reached the end, if so remove income rom player and set ai to inactive
            //Debug.Log("Reached End");
            MoneySystem.current.DeductIncome(TroopEscapeCost);
            gameObject.name = "Inactive AI";
            gameObject.SetActive(false);
        }
        else
        {
            //set waypoint
            CurrentWaypointPos = Waypoints.Instance.GetWaypointPosition(currentWaypoint);
        }
    }
    bool HasReachedWaypoint()
    {
        if(Vector2.Distance(transform.position, CurrentWaypointPos)<WaypointChangeDist)//is near to target
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
        if (Health <= 0)
        {
            gameObject.name = "Dead AI";
            gameObject.SetActive(false);
        }
        
        if (HasReachedWaypoint())//has reached target
        {
            NextWaypoint();//get next waypoint
            return;
        }
        Vector2 move = Vector2.MoveTowards(transform.position, CurrentWaypointPos, (Speed*stunModifier()) * Time.deltaTime);
        //move towards next waypoint
        Vector3 move3 = move;
        Vector3 direction = transform.position - move3;
        transform.position = new Vector3(move.x, move.y, -1);

        //set the directions and speed for the animator to animate the sprite
        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
        anim.SetFloat("speed", direction.sqrMagnitude);
    }
}
