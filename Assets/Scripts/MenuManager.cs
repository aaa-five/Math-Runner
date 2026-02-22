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

using UnityEngine;
using System.Collections.Generic;

public class EndlessRoadManager : MonoBehaviour
{
    // List to hold the road sections currently in the scene
    public List<GameObject> activeRoadSections; 
    public GameObject[] roadPrefabs; // Array of different road prefabs (for variety)
    public float roadSpeed;
    public float sectionLength = 100f; // Length of a single road prefab
    public Transform playerTransform; // Reference to the player car's transform

    void Start()
    {
        // Instantiate initial sections or use the ones already in the scene
        // (You can use object pooling for better performance as your game grows)
    }

    void Update()
    {
        // Move the road sections towards the player
        foreach (var road in activeRoadSections)
        {
            // Move the road using transform.Translate or by modifying transform.position
            // Ensure you use Time.deltaTime for consistent speed across different frame rates
            road.transform.Translate(Vector3.back * roadSpeed * Time.deltaTime);
        }

        // Check if the first section is behind the player and needs to be moved to the end
        if (activeRoadSections.Count > 0 && activeRoadSections[0].transform.position.z < playerTransform.position.z - sectionLength)
        {
            MoveRoadSection();
        }
    }

    void MoveRoadSection()
    {
        // Get the first (oldest) section
        GameObject movedSection = activeRoadSections[0];
        activeRoadSections.RemoveAt(0);

        // Calculate the new position: the end of the last section currently in front
        // The position is calculated by adding the sectionLength to the last section's Z position
        float newZPosition = activeRoadSections[activeRoadSections.Count - 1].transform.position.z + sectionLength;
        movedSection.transform.position = new Vector3(0, 0, newZPosition);

        // Add the moved section to the end of the list
        activeRoadSections.Add(movedSection);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class Level1Button : MonoBehaviour
{
    // Method to load the game scene by name
    public void LoadGameScene()
    {
        // Replace with your exact game scene name
        SceneManager.LoadScene("Math Runner 1.0 Game Assets"); 
    }
    
    // Alternative method: Load next scene by index
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
