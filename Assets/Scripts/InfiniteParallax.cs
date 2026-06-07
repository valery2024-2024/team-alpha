using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    [Header("Камера")]
    public Transform cameraTransform;

    [Header("Сила паралаксу")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    [Header("Ширина одного фрагмента фону")]
    public float backgroundWidth = 20f;

    private Vector3 startPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        startPosition = transform.position;
    }

    void LateUpdate()
    {
        float distance = cameraTransform.position.x * parallaxEffect;
        float temp = cameraTransform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(
            startPosition.x + distance,
            startPosition.y,
            startPosition.z
        );

        if (temp > startPosition.x + backgroundWidth)
        {
            startPosition.x += backgroundWidth;
        }
        else if (temp < startPosition.x - backgroundWidth)
        {
            startPosition.x -= backgroundWidth;
        }
    }
}