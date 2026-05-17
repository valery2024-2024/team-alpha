using UnityEngine;

public class Coin : MonoBehaviour
{
    // CoinManager
    public CoinManager coinManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Якщо торкнувся Player - гравець
        if (collision.CompareTag("Player"))
        {
            // Додає монету
            coinManager.AddCoin();

            // Ховає монету
            gameObject.SetActive(false);
        }
    }
}