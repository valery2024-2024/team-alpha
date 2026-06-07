using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Звук збору монети")]
    public AudioClip coinSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Якщо монету торкнувся Player
        if (collision.CompareTag("Player"))
        {
            // Додаємо монету до лічильника
            CoinManager coinManager = FindFirstObjectByType<CoinManager>();

            if (coinManager != null)
            {
                coinManager.AddCoin();
            }

            // Програємо звук монети
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
            }

            // Ховаємо монету після збору
            gameObject.SetActive(false);
        }
    }
}