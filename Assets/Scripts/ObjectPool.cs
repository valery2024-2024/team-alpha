using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Prefab obstacle
    public GameObject obstaclePrefab;

    // Скільки obstacle - перешкод створити
    public int poolSize = 5;

    // Список obstacle - перешкод
    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Створює obstacle - перешкод наперед
        for (int i = 0; i < poolSize; i++)
        {
            // Створення obstacle - перешкод
            GameObject obj = Instantiate(obstaclePrefab);

            // Ховаємо obstacle - перешкоди
            obj.SetActive(false);

            // Додає у список
            pool.Add(obj);
        }
    }

    // Метод отримання obstacle - перешкод
    public GameObject GetObject()
    {
        // Шукає вільний obstacle - перешкоду
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                // Повертає obstacle - перешкоду
                return obj;
            }
        }

        return null;
    }
}