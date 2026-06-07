using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Звук збору монети")]
    public AudioClip coinSound;

    private AudioSource audioSource;

    private void Start()
    {
        // Беремо AudioSource з Player
        audioSource = FindFirstObjectByType<PlayerController>()
            .GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinManager coinManager = FindFirstObjectByType<CoinManager>();

            if (coinManager != null)
            {
                coinManager.AddCoin();
            }

            // Відтворення звуку
            if (coinSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinSound);
            }

            gameObject.SetActive(false);
        }
    }
}