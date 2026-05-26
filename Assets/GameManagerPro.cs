using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerPro : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text bestText;
    public TMP_Text messageText;

    [Header("Scoring")]
    public float scorePerSecond = 1f;

    private float score = 0f;
    private float best = 0f;

    public bool IsGameOver { get; private set; } = false;
    private bool isPaused = false;

    private const string BEST_KEY = "BEST_SCORE";

    void Start()
    {
        best = PlayerPrefs.GetFloat(BEST_KEY, 0f);

        if (messageText != null)
            messageText.text = "";

        Time.timeScale = 1f;

        UpdateUI();
    }

    void Update()
    {
        // ❗ ЖОРСТКА перевірка
        if (IsGameOver == true)
            return;

        if (isPaused == true)
            return;

        score += scorePerSecond * Time.deltaTime;

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetFloat(BEST_KEY, best);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    // -------------------
    // GAME OVER
    // -------------------
    public void GameOver(string reason)
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("GAME OVER TRIGGERED: " + reason, this);

        if (messageText != null)
            messageText.text = "GAME OVER";
    }

    // -------------------
    // RESTART
    // -------------------
    public void RestartButton()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // -------------------
    // PAUSE
    // -------------------
    public void TogglePauseButton()
    {
        if (IsGameOver) return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (messageText != null)
            messageText.text = isPaused ? "PAUSED" : "";
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score);

        if (bestText != null)
            bestText.text = "Best: " + Mathf.FloorToInt(best);
    }
}
