using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI WavesText;

    public GameObject[] WaveActiveUI;
    public GameObject[] WaveInActiveUI;
    public GameObject GameOverUI;

    [Header("Income")]
    public GameObject BankRuptPopUp;
    public MoneyGraph moneyGraph;



    public void SetWaveUI(bool InWave)
    {
        foreach (var item in WaveActiveUI)
        {
            item.SetActive(InWave);
        }
        foreach (var item in WaveInActiveUI)
        {
            item.SetActive(!InWave);
        }
    }

    private void Awake()
    {

        WaveManager.Current.OnWaveChanged += OnWaveChanged;
        WaveManager.Current.OnWaveEnded += OnWaveEnded;
        WaveManager.Current.OnWaveStarted += OnWaveStarted;
        WaveManager.Current.OnGameOver += OnGameOver;

        moneyGraph.enabled = true;


    }


    private void OnGameOver(object sender, System.EventArgs e)
    {

        foreach (var item in WaveActiveUI)
        {
            item.SetActive(false);
        }
        foreach (var item in WaveInActiveUI)
        {
            item.SetActive(false);
        }
        int CalculatedScore = 0;

        if (!MoneySystem.current.HasGoneBankRupt)
        {
            if (!MoneySystem.current.hadAnEscape)
            {
                CalculatedScore = 3;
            }
            else
            {
                CalculatedScore = 2;
            }
        }
        else
        {
            CalculatedScore = 1;
        }

        GameOverUI.SetActive(true);
        GameOverUI.GetComponent<ScoreLabel>().ShowScoreDisplay(CalculatedScore);
        ScoreSystem.SetScore(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, CalculatedScore);
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        int NextIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        if(NextIndex<UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextIndex);
        }
        else
        {
            ReturnToMenu();
        }
        
    }

    private void OnDestroy()
    {
        moneyGraph.enabled = false;
        WaveManager.Current.OnWaveChanged -= OnWaveChanged;
        WaveManager.Current.OnWaveEnded -= OnWaveEnded;
        WaveManager.Current.OnWaveStarted -= OnWaveStarted;
        WaveManager.Current.OnGameOver -= OnGameOver;
        
    }

    private void OnWaveStarted(object sender, System.EventArgs e)
    {
        SetWaveUI(true);
    }

    private void OnWaveEnded(object sender, System.EventArgs e)
    {
        SetWaveUI(false);
    }

    private void OnWaveChanged(object sender, System.EventArgs e)
    {
        WavesText.text = WaveManager.Current.GetWavesInfo();
    }


    public void StartWaveClicked()
    {
        if (MoneySystem.IsCurrentlyBankRupt())
        {
            BankRuptPopUp.SetActive(true);
        }
        else
        {
            WaveManager.Current.StartNextWave();
        }
        
    }
}
