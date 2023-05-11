using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//leo waters
//16 03 2023
//moves a projectile towards a target
public class Projectile : MonoBehaviour
{
    Transform Target;
    public float Speed = 5;
    public void Setup(GameObject Target_Obj)
    {
        Target = Target_Obj.transform;
    }
    public void FixedUpdate()
    {
        if (Target)
        {
            if(!Target.gameObject.activeInHierarchy|| Vector2.Distance(transform.position, Target.position) < 0.2)
            {
                gameObject.SetActive(false);
                return;
            }
            Vector2 move = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
            //move towards next waypoint
            transform.position = new Vector3(move.x, move.y, -1);
        }
    }
}
