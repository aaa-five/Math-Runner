using UnityEngine;

public class AnswerTrigger : MonoBehaviour
{
    public int answerValue;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private bool used = false;

private void OnTriggerEnter(Collider other)
{
    if (used) return;
    if (!other.CompareTag("Player")) return;

    used = true;

    gameManager.CheckAnswer(answerValue);
    Destroy(gameObject);
}
}