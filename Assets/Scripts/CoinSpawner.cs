using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Префаб монети")]
    public GameObject coinPrefab;

    [Header("Гравець")]
    public Transform player;

    [Header("Налаштування появи")]
    public float spawnDistance = 12f;
    public float spawnInterval = 2f;
    public float minY = 1.2f;
    public float maxY = 2.2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    void SpawnCoin()
    {
        float xPosition = player.position.x + spawnDistance;
        float yPosition = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(xPosition, yPosition, 0f);

        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}