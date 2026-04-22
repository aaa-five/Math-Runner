using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 10f;

    private float finalSpeed;
    private LevelConfig levelConfig;

    void Start()
    {
        // Get current level settings
        levelConfig = FindFirstObjectByType<LevelConfig>();

        // Apply speed multiplier if available
        if (levelConfig != null)
        {
            finalSpeed = speed * levelConfig.speedMultiplier;
        }
        else
        {
            finalSpeed = speed;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.back * finalSpeed * Time.deltaTime);
    }
}