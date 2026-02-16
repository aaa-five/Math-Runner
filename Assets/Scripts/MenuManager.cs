using UnityEngine;
 
public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }
 
    // Update is called once per frame
    void Update()
    {
       
    }
}

using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sidewaysSpeed = 12f; // Increased for better feel
    
    private float horizontalInput;

    void Update()
    {
        // Keep input in Update for responsiveness
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // 1. Define the direction of movement (Normalized)
        // We calculate the direction first, then apply speed and time later.
        Vector3 direction = new Vector3(horizontalInput * sidewaysSpeed, 0, speed);

        // 2. Apply movement
        // We multiply by Time.fixedDeltaTime HERE to ensure frame-rate independence
        transform.Translate(direction * Time.fixedDeltaTime, Space.Self);
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target;     // Drag your Car here
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Position relative to car
    [SerializeField] private float smoothSpeed = 0.125f;            // Higher = snappier

    void LateUpdate()
    {
        // LateUpdate runs after movement to prevent stuttering
        Vector3 desiredPosition = target.position + offset;
        
        // Use Lerp (Linear Interpolation) for that smooth "elastic" feel
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;

        // Always look at the car
        transform.LookAt(target);
    }
}
