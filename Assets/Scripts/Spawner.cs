using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform spawnPoint;

    public float spawnTime = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);
            timer = 0f;
        }
    }
}