using UnityEngine;
using TMPro;

public class AnswerSpawner : MonoBehaviour
{
    public GameObject numberPrefab;   // TextMeshPro prefab
    public Transform car;

    public float leftLaneX = -3.5f;
    public float rightLaneX = 3.5f;
    public float spawnDistanceZ = 30f;

    [Header("Text Size")]
    public float normalFontSize = 6f;
    public float tripleDigitFontSize = 4.5f;

    public void SpawnAnswers(int correctAnswer)
    {
        if (numberPrefab == null || car == null)
        {
            Debug.LogError("Missing references.");
            return;
        }

        bool correctOnLeft = Random.value > 0.5f;

        int wrongAnswer = correctAnswer + Random.Range(1, 6);
        if (wrongAnswer == correctAnswer)
            wrongAnswer += 1;

        Vector3 leftPos = new Vector3(leftLaneX, car.position.y + 1f, car.position.z + spawnDistanceZ);
        Vector3 rightPos = new Vector3(rightLaneX, car.position.y + 1f, car.position.z + spawnDistanceZ);

        GameObject leftObj = Instantiate(numberPrefab, leftPos, Quaternion.identity);
        GameObject rightObj = Instantiate(numberPrefab, rightPos, Quaternion.identity);

        TextMeshPro leftText = leftObj.GetComponent<TextMeshPro>();
        TextMeshPro rightText = rightObj.GetComponent<TextMeshPro>();

        AnswerTrigger leftTrigger = leftObj.GetComponent<AnswerTrigger>();
        AnswerTrigger rightTrigger = rightObj.GetComponent<AnswerTrigger>();

        if (leftTrigger == null || rightTrigger == null)
        {
            Debug.LogError("AnswerTrigger is missing on the prefab!");
            return;
        }

        if (correctOnLeft)
        {
            leftText.text = correctAnswer.ToString();
            rightText.text = wrongAnswer.ToString();

            leftTrigger.answerValue = correctAnswer;
            rightTrigger.answerValue = wrongAnswer;

            AdjustTextSize(leftText, correctAnswer);
            AdjustTextSize(rightText, wrongAnswer);
        }
        else
        {
            leftText.text = wrongAnswer.ToString();
            rightText.text = correctAnswer.ToString();

            leftTrigger.answerValue = wrongAnswer;
            rightTrigger.answerValue = correctAnswer;

            AdjustTextSize(leftText, wrongAnswer);
            AdjustTextSize(rightText, correctAnswer);
        }
    }

    void AdjustTextSize(TextMeshPro textMesh, int number)
    {
        if (textMesh == null)
            return;

        if (Mathf.Abs(number) >= 100)
            textMesh.fontSize = tripleDigitFontSize; // smaller for 3 digits
        else
            textMesh.fontSize = normalFontSize;      // normal for 1-2 digits
    }
}
