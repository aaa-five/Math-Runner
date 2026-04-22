using UnityEngine;
using System.Collections.Generic;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public int roadsOnScreen = 3;
    public float roadLength = 50f;
    public float moveSpeed = 5f;

    private List<GameObject> activeRoads = new List<GameObject>();

    private LevelConfig levelConfig;
    private float finalMoveSpeed;

    void Start()
    {
        // Get current level settings
        levelConfig = FindFirstObjectByType<LevelConfig>();

        // Apply speed multiplier if available
        if (levelConfig != null)
            finalMoveSpeed = moveSpeed * levelConfig.speedMultiplier;
        else
            finalMoveSpeed = moveSpeed;

        if (roadPrefab == null)
        {
            Debug.LogError("Road Prefab is not assigned in RoadSpawner.");
            return;
        }

        Renderer rend = roadPrefab.GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Road Prefab does not contain a Renderer.");
            return;
        }

        roadLength = rend.bounds.size.z;

        for (int i = 0; i < roadsOnScreen; i++)
        {
            GameObject newRoad = Instantiate(
                roadPrefab,
                Vector3.forward * (i * roadLength),
                Quaternion.identity
            );

            activeRoads.Add(newRoad);
        }
    }

    void Update()
    {
        MoveRoads();
        RecycleRoad();
    }

    void MoveRoads()
    {
        foreach (GameObject road in activeRoads)
        {
            road.transform.position += Vector3.back * finalMoveSpeed * Time.deltaTime;
        }
    }

    void RecycleRoad()
    {
        if (activeRoads.Count == 0)
            return;

        GameObject firstRoad = activeRoads[0];

        if (firstRoad.transform.position.z < -roadLength)
        {
            GameObject lastRoad = activeRoads[activeRoads.Count - 1];

            firstRoad.transform.position =
                new Vector3(0, 0, lastRoad.transform.position.z + roadLength);

            activeRoads.RemoveAt(0);
            activeRoads.Add(firstRoad);
        }
    }
}