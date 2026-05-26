using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;                
    public Transform spawnParent;           
    public Collider2D groundCollider;       

    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Ahead")]
    public float spawnAheadDistance = 18f;

    [Header("Spacing")]
    public float minSpacing = 6f;           
    public float maxSpacing = 11f;          

    [Header("Chance")]
    [Range(0f, 1f)] public float spawnChance = 0.85f;

    [Header("STACK / CLUSTER")]
    [Tooltip("Шанс, що буде кластер (2-3 однакові підряд)")]
    [Range(0f, 1f)] public float clusterChance = 0.35f;

    [Tooltip("Мін кількість у кластері")]
    public int clusterMinCount = 2;

    [Tooltip("Макс кількість у кластері")]
    public int clusterMaxCount = 3;

    [Tooltip("Відстань між блоками в кластері")]
    public float clusterSpacing = 2.2f;

    [Header("HEIGHT SPAWN")]
    [Tooltip("Шанс, що перешкода буде вище землі")]
    [Range(0f, 1f)] public float elevatedChance = 0.25f;

    [Tooltip("Мін висота над землею")]
    public float elevatedMinHeight = 1.0f;

    [Tooltip("Макс висота над землею")]
    public float elevatedMaxHeight = 2.5f;

    [Header("Cleanup")]
    public float destroyBehindDistance = 25f;

    private float nextSpawnX;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        nextSpawnX = player.position.x + spawnAheadDistance;
    }

    void Update()
    {
        if (player == null || obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        float triggerX = player.position.x + spawnAheadDistance;

        if (triggerX >= nextSpawnX)
        {
            SpawnGroupAt(nextSpawnX);

            // наступний "слот" спавну (базовий спейсинг)
            float spacing = Random.Range(minSpacing, maxSpacing);
            nextSpawnX += spacing;
        }

        CleanupOld();
    }

    void SpawnGroupAt(float x)
    {
        if (Random.value > spawnChance) return;

        // вибираємо 1 префаб
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // вирішуємо: одиночний чи кластер
        bool makeCluster = (Random.value < clusterChance);

        int count = 1;
        if (makeCluster)
        {
            count = Random.Range(clusterMinCount, clusterMaxCount + 1);
        }

        // спавнимо 1..N
        for (int i = 0; i < count; i++)
        {
            float spawnX = x + i * clusterSpacing;

            float y = GetGroundYAtX(spawnX);

            // шанс підняти вище
            if (Random.value < elevatedChance)
            {
                y += Random.Range(elevatedMinHeight, elevatedMaxHeight);
            }

            Vector3 pos = new Vector3(spawnX, y, 0f);

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, spawnParent);

            // на всякий випадок
            obj.tag = "Obstacle";
        }
    }

    float GetGroundYAtX(float x)
    {
        if (groundCollider != null)
        {
            return groundCollider.bounds.max.y;
        }

        return 0f;
    }

    void CleanupOld()
    {
        if (spawnParent == null) return;

        for (int i = spawnParent.childCount - 1; i >= 0; i--)
        {
            Transform child = spawnParent.GetChild(i);
            if (child.position.x < player.position.x - destroyBehindDistance)
            {
                Destroy(child.gameObject);
            }
        }
    }
}