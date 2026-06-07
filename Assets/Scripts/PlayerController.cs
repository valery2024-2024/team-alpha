using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 7f;

    public GameObject gameOverText;

    [Header("Звук програшу")]
    public AudioClip gameOverSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    private int jumpCount = 0;
    private bool gameOverPlayed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        gameOverText.SetActive(false);
    }

    void Update()
    {
        // Автоматичний рух вперед
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // Подвійний стрибок
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Скидання лічильника стрибків
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }

        // Програш
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER");

            // Програти звук тільки один раз
            if (!gameOverPlayed)
            {
                if (audioSource != null && gameOverSound != null)
                {
                    audioSource.PlayOneShot(gameOverSound);
                }

                gameOverPlayed = true;
            }

            gameOverText.SetActive(true);

            // Невелика затримка щоб звук встиг програтись
            Invoke(nameof(StopGame), 1f);
        }
    }

    void StopGame()
    {
        Time.timeScale = 0f;
    }
}