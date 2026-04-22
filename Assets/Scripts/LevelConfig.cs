using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    [Header("Win / Lose Rules")]
    public int correctNeeded = 5;
    public int wrongAllowed = 3;

    [Header("Question Type")]
    public bool firstNumberOneDigit = true;
    public bool secondNumberOneDigit = true;

    [Header("Speed")]
    public float speedMultiplier = 1f;     // for objects & car
    public float roadSpeedMultiplier = 1f; // NEW: for road only
}