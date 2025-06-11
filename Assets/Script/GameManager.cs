using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool GameStarted { get; private set; } = false;
    public bool TutorialActive { get; private set; } = false;
    [SerializeField] private Player player;
    [SerializeField] private GameObject tutoPanel;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SpawnManager spawnManager;
    private int score = 0;
    private int highScore = 0;  
    private float timeElapsed = 0f;
    private float difficultyInterval = 10f; 


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (GameSession.isRetry)
        {
            // Configuración rápida si es un reinicio
            GameStarted = true;
            TutorialActive = false;

            uiManager.HideTapAndTitle();
            uiManager.ShowScore();
            uiManager.UpdateScore(0);
            uiManager.UpdateHighScore(highScore);
            uiManager.showPauseButton();

            player.enabled = true;
            spawnManager.StartSpawning();

            GameSession.isRetry = false;
        }
        else
        {
            uiManager.Initialize(highScore);
            player.enabled = false;
        }
    }

    void Update()
    {
        if (GameStarted)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= difficultyInterval)
            {
                spawnManager.IncreaseDifficulty();
                timeElapsed = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!GameStarted && !TutorialActive)
            {
                StartGame();
            }
            else if (TutorialActive)
            {
                EndTutorial();
            }
        }
    }

    private void StartGame()
    {
        GameStarted = true;
        uiManager.HideTapAndTitle();
        uiManager.ShowScore();
        ShowTutorial();
        TutorialActive = true;
    }

    private void EndTutorial()
    {
        TutorialActive = false;
        player.enabled = true;
        HideTutorial();
        spawnManager.StartSpawning();
        uiManager.showPauseButton(); 
    }

    public void AddScore()
    {
        score++;
        uiManager.UpdateScore(score);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            uiManager.UpdateHighScore(highScore);
        }
    }
    
    public void ShowTutorial()
    {
        tutoPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        tutoPanel.SetActive(false);
    }
}
