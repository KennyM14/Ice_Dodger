using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tapText;
    [SerializeField] private GameObject titleText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject GOPanel;

    private CanvasGroup tapTextCanvasGroup;

    void Awake()
    {
        tapTextCanvasGroup = tapText.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        pauseButton.gameObject.SetActive(false);
    }

    public void Initialize(int highScore)
    {
        StartBlink();
        highScoreText.text = "High Score: " + highScore;
        scoreText.gameObject.SetActive(false);
    }

    public void HideTapAndTitle()
    {
        StopBlink();
        tapText.SetActive(false);
        titleText.SetActive(false);
    }

    public void ShowScore()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
        scoreText.transform.DOKill();
        scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1);
    }

    public void UpdateHighScore(int score)
    {
        highScoreText.text = "High Score: " + score;
    }

    private void StartBlink()
    {
        tapTextCanvasGroup.DOFade(0f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void StopBlink()
    {
        tapTextCanvasGroup.DOKill();
        tapTextCanvasGroup.alpha = 1f;
    }

    public void showPauseButton()
    {
        pauseButton.gameObject.SetActive(true);
    }

    public void HidePauseButton()
    {
        pauseButton.gameObject.SetActive(false);
    }

    public void ShowPausePanel()
    {
        Debug.Log("ShowPausePanel fue llamado");
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
        pauseButton.gameObject.SetActive(false);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
        pauseButton.gameObject.SetActive(true);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        GameSession.isRetry = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        GOPanel.SetActive(true);

        CanvasGroup canvasGroup = GOPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, 0.5f).SetUpdate(true).SetEase(Ease.OutQuad);
        }
    }
}
