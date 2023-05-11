using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void update()
    {
        Application.Quit();
        Debug.Log("QUIT");       
    }
}
