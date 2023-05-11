using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TroopType
{
    basic,
    small,
    large
}
public class WaveManager : MonoBehaviour
{
    public static WaveManager Current;
    [System.Serializable]
    public class Wave //stores information about a wave
    {
        [SerializeField]
        public int MoneyIncomeOnCompletion = 100;
        [SerializeField]
        public BattleGroup[] BattleGroups;
    }
    public Transform RespawnPos;
    public GameObject AI_prefab;
    public int poolSize;
    GameObject[] AiPool;
    public int CurrentWave = -1;
    public Wave[] Waves;

    //wave events
    public event EventHandler OnWaveChanged;
    public event EventHandler OnWaveEnded;
    public event EventHandler OnWaveStarted;
    public event EventHandler OnGameOver;

    public string GetWavesInfo()
    {
        return "Wave "+(CurrentWave + 1).ToString() + " / " + Waves.Length;
    }

    bool SpawnAIFromPool(TroopType type)//spawn an AI based on its type
    {
        foreach (var item in AiPool)
        {
            if (item.activeSelf == false)
            {
                item.transform.position = RespawnPos.position;
                switch (type)
                {
                    case (TroopType.basic):
                        item.name = "basic";
                        item.GetComponent<AINavigation>().Setup(100, new Vector3(0.4f, 0.4f, 1),1);
                        break;
                    case (TroopType.small):
                        item.name = "small";
                        item.GetComponent<AINavigation>().Setup(50, new Vector3(0.3f, 0.35f, 1),1.6f);
                        break;
                    case (TroopType.large):
                        item.name = "large";
                        item.GetComponent<AINavigation>().Setup(250, new Vector3(0.6f, 0.6f, 1),0.5f);
                        break;
                    default:
                        item.name = "no case for troop type";
                        item.GetComponent<AINavigation>().Setup(100,new Vector3(0.4f, 0.4f, 1),1);
                        break;
                }
                
                

                return true;
            }
        }
        return false;
    }

    //are any troops alive
    bool HasTroopAlive()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (AiPool[i].activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        Current = this;
        AiPool = new GameObject[poolSize];//create AI pool
        for (int i = 0; i < poolSize; i++)
        {
            AiPool[i] = Instantiate(AI_prefab, RespawnPos.position, Quaternion.identity,null);
        }
       
    }

    private void Start()
    {
        //initalize waves
        OnWaveEnded?.Invoke(this, EventArgs.Empty);
        MoneySystem.current.SetIncome(Waves[CurrentWave + 1].MoneyIncomeOnCompletion);

    }
    bool waveActive = false;
    public void StartNextWave()//starts the next wave
    {
        CurrentWave++;
        OnWaveChanged?.Invoke(this, EventArgs.Empty);
        StartCoroutine(WaveUpdate());
    }

    IEnumerator WaveUpdate()//wave update sub routine
    {
        Debug.Log("Wave: " + CurrentWave + " Started");
        waveActive = true;
        
        OnWaveStarted?.Invoke(this, EventArgs.Empty);//invoke wave start event

        for (int x = 0; x < Waves[CurrentWave].BattleGroups.Length; x++)//iterate through waves battle groups
        {
            yield return new WaitForSecondsRealtime(Waves[CurrentWave].BattleGroups[x].StartDelay);//wait for battlegroup start delay

            for (int i = 0; i < Waves[CurrentWave].BattleGroups[x].Troops.Length; i++)
            {
                if (SpawnAIFromPool(Waves[CurrentWave].BattleGroups[x].Troops[i]) == false)//AI pool didnt have any ais free, should be increased
                {
                    Debug.LogWarning("Not Enough AI whithin pool");
                }
                yield return new WaitForSecondsRealtime(Waves[CurrentWave].BattleGroups[x].SpawnRate);//wait before spawning next troop
            }
        }

        yield return new WaitWhile(HasTroopAlive);//wait until all troops are dead

        waveActive = false;
        Debug.Log("Wave: " + CurrentWave + " Ended");
        if (CurrentWave + 1 == Waves.Length) // game over state reached
        {
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnWaveEnded?.Invoke(this, EventArgs.Empty); //wave over, notify event subscribers
            MoneySystem.current.SetIncome(Waves[CurrentWave + 1].MoneyIncomeOnCompletion);
        }
    }
}
