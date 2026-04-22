using UnityEngine;

public class DebugReset : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelProgressManager.ResetAllProgress();
            Debug.Log("Progress Reset!");
        }
    }
}