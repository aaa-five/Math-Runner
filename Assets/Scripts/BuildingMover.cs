using UnityEngine;

public class BuildingMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float resetZ = 60f;
    public float destroyZ = -30f;

    void Update()
    {
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;

        if (transform.position.z < destroyZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetZ);
        }
    }
}
