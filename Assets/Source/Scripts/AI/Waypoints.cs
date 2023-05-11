using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance;
    public Transform[] points;
    private void Awake()
    {
        Instance = this;//set singleton
        points = GetComponentsInChildren<Transform>();//get all waypoints from children
    }

    public bool IsAtEnd(int index)//is this index the last waypoint
    {
        return (index+1 == points.Length);
    }
    public Vector2 GetWaypointPosition(int index)//gets the position of a waypoint
    {
        return points[index+1].position;
    }
    public Vector2 GetRandom()//gets a random waypoint, used for Prisoner court yard in menu
    {
        return points[Random.Range(0,points.Length)].position;
    }
}
