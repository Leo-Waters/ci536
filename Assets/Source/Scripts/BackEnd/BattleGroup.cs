using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BattleGroup", order = 1)]
public class BattleGroup : ScriptableObject
{
    public float StartDelay = 5;
    public float SpawnRate = 1;
    public TroopType[] Troops;
}