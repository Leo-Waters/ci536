using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytimePopUp : MonoBehaviour
{
    static PlaytimePopUp instance;
    public GameObject Popup;
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            //destroys duplicate pop up
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //makes the pop up persist throughout each scene
            DontDestroyOnLoad(gameObject);
            StartCoroutine(CountDown());
        }
        
    }
    IEnumerator CountDown()
    {
        //shows pop up after 1 hour
        yield return new WaitForSecondsRealtime(3600);
        
        Popup.SetActive(true);
        
    }
    //hides pop up
    public void hide()
    {
        Popup.SetActive(false);
        StartCoroutine(CountDown());
    }
}
