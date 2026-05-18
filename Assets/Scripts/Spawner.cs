using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Object Pool
    public ObjectPool pool;

    // Spawn point
    public Transform spawnPoint;

    // Час появи
    public float spawnTime = 2f;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", 1f, spawnTime);
    }

    void SpawnObstacle()
    {
        // Бере obstacle - перешкоду з pool
        GameObject obj = pool.GetObject();

        // Якщо obstacle - перешкода є
        if (obj != null)
        {
            // Показує obstacle - перешкоду
            obj.SetActive(true);

            // Позиція obstacle - перешкоди
            obj.transform.position = spawnPoint.position;
        }
    }
}