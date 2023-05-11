using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject exitButton;
    public string howToPlayvideo = "www.howtoplay.com";
    private void Awake()
    {

#if UNITY_WEBGL
        if (exitButton)
        {
            exitButton.SetActive(false);
        }
#endif
    }
    public void LoadLevel(int Index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Index);
    }

    public void HowToPlay()
    {
        Application.OpenURL(howToPlayvideo);
    }
}
