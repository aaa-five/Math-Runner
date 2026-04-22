using UnityEngine;
using System.Collections;

public class ApproachingSign : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float stopZPosition = 4.5f;
    public float startZPosition = 50f;

    public MathQuestion mathQuestion;
    public AnswerSpawner spawner;
    public GameManager gameManager;

    private bool isMoving = true;
    private bool cycleRunning = false;
    private Renderer signRenderer;

    private LevelConfig levelConfig;
    private float finalMoveSpeed;

    void Start()
    {
        signRenderer = GetComponent<Renderer>();

        // Get current level settings
        levelConfig = FindFirstObjectByType<LevelConfig>();

        // Apply speed multiplier if available
        if (levelConfig != null)
            finalMoveSpeed = moveSpeed * levelConfig.speedMultiplier;
        else
            finalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.Translate(Vector3.back * finalMoveSpeed * Time.deltaTime);

        if (transform.position.z <= stopZPosition && !cycleRunning)
            StartCoroutine(SignCycle());
    }

    IEnumerator SignCycle()
    {
        // if the run is over (win/lose) and time is paused, don't keep cycling
        if (Time.timeScale == 0f) yield break;

        // prevents null issues if GameManager wasn't assigned
        if (gameManager == null) yield break;

        cycleRunning = true;
        isMoving = false;

        transform.position = new Vector3(transform.position.x, transform.position.y, stopZPosition);

        // PHASE 1: show question
        int answerToSpawn = 0;

        if (mathQuestion != null)
        {
            mathQuestion.gameObject.SetActive(true);
            mathQuestion.GenerateQuestion();

            // store answer now
            answerToSpawn = mathQuestion.CorrectAnswer;
            Debug.Log("Generated answer: " + answerToSpawn);
        }
        else
        {
            Debug.LogWarning("MathQuestion is NOT assigned on ApproachingSign.");
        }

        yield return new WaitForSeconds(3f);

        // PHASE 2: hide question + sign
        if (mathQuestion != null)
            mathQuestion.gameObject.SetActive(false);

        if (signRenderer != null)
            signRenderer.enabled = false;

        // Spawn during the gap
        if (spawner != null)
        {
            Debug.Log("Spawning answers during gap...");

            gameManager.NewQuestion(answerToSpawn);
            spawner.SpawnAnswers(answerToSpawn);
        }
        else
        {
            Debug.LogWarning("Spawner is NOT assigned on ApproachingSign.");
        }

        yield return new WaitForSeconds(5f);

        // if the game ended during the gap, don't restart moving
        if (Time.timeScale == 0f) yield break;

        // PHASE 3: reset
        transform.position = new Vector3(transform.position.x, transform.position.y, startZPosition);

        if (signRenderer != null)
            signRenderer.enabled = true;

        isMoving = true;
        cycleRunning = false;
    }
}