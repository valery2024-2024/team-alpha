using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(3f, 1.5f, -10f);

    [HideInInspector] public bool frozen = false;

    void LateUpdate()
    {
        if (target == null) return;
        if (frozen) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            offset.y,
            offset.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    // СНАП — одразу ставимо камеру на правильну позицію (без Lerp)
    public void SnapToTarget()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + offset.x,
            offset.y,
            offset.z
        );
    }
}
