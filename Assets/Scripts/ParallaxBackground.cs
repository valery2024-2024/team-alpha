using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    // Камера
    private Transform cameraTransform;

    // Попередня позиція камери
    private Vector3 previousCameraPosition;

    [Header("Шари паралаксу")]
    public Transform[] layers;

    [Header("Швидкість")]
    public float[] parallaxScales;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        float deltaX = cameraTransform.position.x - previousCameraPosition.x;

        for (int i = 0; i < layers.Length; i++)
        {
            Vector3 pos = layers[i].position;

            pos.x += deltaX * parallaxScales[i];

            layers[i].position = pos;
        }

        previousCameraPosition = cameraTransform.position;
    }
}