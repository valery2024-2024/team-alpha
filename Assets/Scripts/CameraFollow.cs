using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - player.position;

        Debug.Log($"[CameraFollow]: Camera offset ({cameraOffset})");
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x, 0, -10) + cameraOffset;
    }
}