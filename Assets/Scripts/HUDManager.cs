using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    // Текст очок
    public TextMeshProUGUI coinText;

    // Текст життів
    public TextMeshProUGUI livesText;

    // Текст рівня
    public TextMeshProUGUI levelText;

    // Текст таймера
    public TextMeshProUGUI timerText;

    // Кількість життів
    public int lives = 3;

    // Номер рівня
    public int level = 1;

    // Час рівня
    public float timer = 10f;

    void Update()
    {
        // Таймер зменшується
        timer -= Time.deltaTime;
        // Якщо час закінчився
        if (timer <= 0)
        {
            timer = 0;

            // Викликає перемогу
            FindObjectOfType<PauseManager>().WinGame();
        }

        // Оновлення тексту таймера
        timerText.text = "Time: " + Mathf.Round(timer);

        // Оновлення життів
        livesText.text = "Lives: " + lives;

        // Оновлення рівня
        levelText.text = "Level: " + level;
    }
}