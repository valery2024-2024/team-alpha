using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Текст монет
    public TextMeshProUGUI coinText;

    // Кількість монет
    private int coins = 0;

    void Start()
    {
        // Стартовий текст
        coinText.text = "Coins: 0";
    }

    // Додавання монети
    public void AddCoin()
    {
        // +1 монета
        coins++;

        // Оновлення тексту
        coinText.text = "Coins: " + coins;
    }
}