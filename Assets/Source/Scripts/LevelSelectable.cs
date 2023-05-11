using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectable : MonoBehaviour
{
    public Button button;
    public string RequiredLevel = "Level1";
    void Start()
    {
        button.interactable = ScoreSystem.GetScore(RequiredLevel) >= 1;
    }
}
