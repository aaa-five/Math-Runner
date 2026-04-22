using TMPro;
using UnityEngine;

public class MathQuestion : MonoBehaviour
{
    public TMP_Text questionText;

    int correctAnswer;

    public int CorrectAnswer => correctAnswer;

    public void GenerateQuestion()
    {
        LevelConfig config = FindFirstObjectByType<LevelConfig>();

        int a;
        int b;

        // Default ranges
        int minA = 1;
        int maxA = 10;
        int minB = 1;
        int maxB = 10;

        // Use LevelConfig for digit control
        if (config != null)
        {
            minA = config.firstNumberOneDigit ? 1 : 10;
            maxA = config.firstNumberOneDigit ? 10 : 100;

            minB = config.secondNumberOneDigit ? 1 : 10;
            maxB = config.secondNumberOneDigit ? 10 : 100;
        }

        int levelIndex = LevelSelectionMenuManager.selectedLevelIndex;

        switch (LevelSelectionMenuManager.selectedType)
        {
            case LevelProgressManager.LevelType.Addition:
                a = Random.Range(minA, maxA);
                b = Random.Range(minB, maxB);

                correctAnswer = a + b;
                questionText.text = a + " + " + b + " = ?";
                break;

            case LevelProgressManager.LevelType.Subtraction:
                a = Random.Range(minA, maxA);
                b = Random.Range(minB, maxB);

                if (a < b)
                {
                    int temp = a;
                    a = b;
                    b = temp;
                }

                correctAnswer = a - b;
                questionText.text = a + " - " + b + " = ?";
                break;

            case LevelProgressManager.LevelType.Multiplication:
                if (levelIndex == 2) // Level 3
                {
                    a = Random.Range(10, 13); // 10-12
                    b = Random.Range(10, 13);
                }
                else if (levelIndex == 3) // Level 4
                {
                    a = Random.Range(10, 15); // 10-14
                    b = Random.Range(10, 15);
                }
                else
                {
                    // Level 1-2
                    a = Random.Range(1, 10);
                    b = Random.Range(1, 10);
                }

                correctAnswer = a * b;
                questionText.text = a + " × " + b + " = ?";
                break;

            case LevelProgressManager.LevelType.Division:
                int divisor;
                int quotient;
                int dividend;

                if (levelIndex == 2) // Level 3
                {
                    do
                    {
                        divisor = Random.Range(10, 13);   // 10-12
                        quotient = Random.Range(1, 10);   // answer can be 1 digit
                        dividend = divisor * quotient;
                    }
                    while (dividend < 10 || dividend > 70);
                }
                else if (levelIndex == 3) // Level 4
                {
                    do
                    {
                        divisor = Random.Range(10, 15);   // 10-14
                        quotient = Random.Range(1, 10);   // answer can be 1 digit
                        dividend = divisor * quotient;
                    }
                    while (dividend < 10 || dividend > 99);
                }
                else
                {
                    // Level 1-2
                    do
                    {
                        divisor = Random.Range(2, 10);
                        quotient = Random.Range(1, 10);
                        dividend = divisor * quotient;
                    }
                    while (dividend < 1 || dividend > 99);
                }

                correctAnswer = quotient;
                questionText.text = dividend + " ÷ " + divisor + " = ?";
                break;
        }
    }
}