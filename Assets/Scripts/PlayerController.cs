using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 7f;

    public GameObject gameOverText;

    private Rigidbody2D rb;
    private int jumpCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameOverText.SetActive(false);
    }

    void Update()
    {
        // авто біг
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // стрибок
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER");

            gameOverText.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}