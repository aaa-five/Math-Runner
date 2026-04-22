using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Make offset relative to car rotation
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Match camera rotation to car rotation
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target.rotation,
            smoothSpeed * Time.deltaTime
        );
    }
}
