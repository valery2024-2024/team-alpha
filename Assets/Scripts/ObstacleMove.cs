using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    // Швидкість
    public float speed = 5f;

    void Update()
    {
        // Рух obstacle - перешкоди
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Якщо obstacle - перешкода вийшла за межі
        if (transform.position.x < -15f)
        {
            // Ховає obstacle - перешкоду
            gameObject.SetActive(false);
        }
    }
}