using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//used to display a levels current High Score;
public class ScoreLabel : MonoBehaviour
{
    public Image[] Stars;
    //must be the same as scene file name
    public string Level;
    public Sprite star_colored, star_blank;

    private void Awake()
    {
        if (Level != string.Empty)
        {
            ShowScoreDisplay();
        }
       
    }

    //sets the color of the stars to display the score 
    public void ShowScoreDisplay()
    {
        ShowScoreDisplay(ScoreSystem.GetScore(Level));
    }

    public void ShowScoreDisplay(int score)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i <= score - 1)
            {
                Stars[i].sprite = star_colored;
            }
            else
            {
                Stars[i].sprite = star_blank;
            }

        }
    }
}

public static class ScoreSystem
{
    //gets the score of the desired level, 0 1 2 or 3 stars
    public static int GetScore(string LevelName)
    {
        return PlayerPrefs.GetInt(LevelName + "_StarsScore",0);
    }

    //sets the score if it is higher than the previous, 0 1 2 or 3 stars
    public static void SetScore(string LevelName,int NumberOfStars)
    {
        if (NumberOfStars > GetScore(LevelName))
        {
            PlayerPrefs.SetInt(LevelName + "_StarsScore",NumberOfStars);
        }
    }
}
