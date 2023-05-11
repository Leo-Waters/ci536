using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//leo waters 16/03/2023
//target sensor
//detects and stores reference to AI's

[RequireComponent(typeof(CircleCollider2D))]
public class TargetSensor : MonoBehaviour
{
    public CircleCollider2D RangeTrigger;
    List<GameObject> Targets = new List<GameObject>();

    //set the range of the sensor
    public void SetRange(float range)
    {
        RangeTrigger.radius = range;
    }

    //does the sensor have targets in range
    public bool HasTarget()
    {
        return (Targets.Count != 0)&& Targets[0]!=null;
    }
    //gets the first target
    public GameObject GetTarget()
    {
        return Targets[0];
    }
    //removes a target from  storage used if it has been killed
    public void RemoveTarget(GameObject Target)
    {
        if (Targets.Contains(Target))
        {
            Targets.Remove(Target);
        }
       
    }

    //adds detected ai
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "AI")
        {
            Targets.Add(collision.gameObject);
        }
    }
    //removes ai that is no longer in range
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AI")
        {
            Targets.Remove(collision.gameObject);
        }
    }
}
