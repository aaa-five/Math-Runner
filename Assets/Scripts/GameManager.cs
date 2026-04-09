using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Flash Overlay")]
    public Image screenFlash;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip failSound;

    [Header("Score Rules")]
    public int maxWrong = 3;
    public int maxCorrect = 5;

    [Header("Tutorial Settings")]
    public GameObject tutorialTextObject; // Assign your "Swipe" text here
    public int tutorialTarget = 1;        // Set to 1 for now, change to 5 later
    private int tutorialProgress = 0;

    [Header("Crash Settings")]
    public GameObject playerCar;
    public float crashDuration = 1.5f;
    public float crashSpinSpeed = 500f;
    public float crashTiltAmount = 25f;
    public MonoBehaviour[] carControlScripts;   // drag movement scripts here
    public ParticleSystem crashExplosion;       // optional explosion effect
    public Transform explosionSpawnPoint;       // where the explosion appears

    [Header("UI (Lights + Counters)")]
    public GameObject topLeftUI;
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI wrongText;
    public Image correctLight;
    public Image wrongLight;

    public Color lightOff = new Color(1f, 1f, 1f, 0.25f);
    public Color lightOn = new Color(1f, 1f, 1f, 1f);

    public TextMeshProUGUI resultText;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Button[] backButtons;
    public Button[] nextButtons;
    public int correctAnswer;

    private int lives;
    private int correctCount;
    private int wrongCount;

    private Coroutine correctLightRoutine;
    private Coroutine wrongLightRoutine;

    private bool hasAnswered = false;
    private bool runEnded = false;
    private Coroutine flashRoutine;
    private Coroutine resultTextRoutine;
    private LevelConfig currentLevelConfig;

    void Start()
    {
        // Check if this is Level 1. If not, turn off tutorial text immediately.
        if (LevelSelectionMenuManager.selectedLevelIndex != 0 && tutorialTextObject != null)
        {
            tutorialTextObject.SetActive(false);
        }

        StartRun();
    }

    public void StartRun()
    {
        currentLevelConfig = FindFirstObjectByType<LevelConfig>();

        if (currentLevelConfig != null)
        {
            maxCorrect = currentLevelConfig.correctNeeded;
            maxWrong = currentLevelConfig.wrongAllowed;
        }

        runEnded = false;
        lives = maxWrong;
        correctCount = 0;
        wrongCount = 0;
        tutorialProgress = 0; // Reset tutorial progress on restart

        if (correctLight) correctLight.color = lightOff;
        if (wrongLight) wrongLight.color = lightOff;

        if (topLeftUI != null)
            topLeftUI.SetActive(true);

        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);

        if (resultText)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        if (screenFlash) screenFlash.color = new Color(0, 0, 0, 0);

        foreach (Button btn in nextButtons)
        {
            if (btn != null)
                btn.gameObject.SetActive(true);
        }

        foreach (MonoBehaviour script in carControlScripts)
        {
            if (script != null)
                script.enabled = true;
        }

        Time.timeScale = 1f;
        UpdateUI();
    }

    public void NewQuestion(int newCorrectAnswer)
    {
        if (runEnded) return;

        correctAnswer = newCorrectAnswer;
        hasAnswered = false;

        if (screenFlash) screenFlash.color = new Color(0, 0, 0, 0);

        if (resultText)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }
    }

    public void CheckAnswer(int playerAnswer)
    {
        if (runEnded) return;
        if (hasAnswered) return;
        hasAnswered = true;

        bool isCorrect = (playerAnswer == correctAnswer);

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashColor(isCorrect ? Color.green : Color.red));

        if (resultTextRoutine != null)
            StopCoroutine(resultTextRoutine);
        resultTextRoutine = StartCoroutine(ShowResultText(isCorrect ? "Correct!" : "Wrong!"));

        if (isCorrect)
        {
            correctCount++;

            // --- TUTORIAL LOGIC ---
            if (tutorialTextObject != null && tutorialTextObject.activeSelf)
            {
                tutorialProgress++;
                if (tutorialProgress >= tutorialTarget)
                {
                    tutorialTextObject.SetActive(false);
                }
            }

            if (correctLightRoutine != null)
                StopCoroutine(correctLightRoutine);

            correctLightRoutine = StartCoroutine(FlashLight(correctLight));

            UpdateUI();

            if (correctCount >= maxCorrect)
            {
                EndRun(win: true);
            }
        }
        else
        {
            lives--;
            wrongCount++;
            UpdateUI();

            if (wrongLightRoutine != null)
                StopCoroutine(wrongLightRoutine);

            wrongLightRoutine = StartCoroutine(FlashLight(wrongLight));

            if (lives <= 0)
            {
                StartCoroutine(CrashSequence());
            }
        }
    }

    private IEnumerator CrashSequence()
    {
        runEnded = true;

        if (resultTextRoutine != null)
        {
            StopCoroutine(resultTextRoutine);
            resultTextRoutine = null;
        }

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        if (playerCar == null)
        {
            EndRun(false);
            yield break;
        }

        foreach (MonoBehaviour script in carControlScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        if (crashExplosion != null)
        {
            Vector3 spawnPos = playerCar.transform.position;
            Quaternion spawnRot = Quaternion.identity;

            if (explosionSpawnPoint != null)
            {
                spawnPos = explosionSpawnPoint.position;
                spawnRot = explosionSpawnPoint.rotation;
            }

            ParticleSystem explosionInstance = Instantiate(crashExplosion, spawnPos, spawnRot);
            explosionInstance.Play();
            Destroy(explosionInstance.gameObject, 2f);
        }

        float timer = 0f;
        while (timer < crashDuration)
        {
            timer += Time.deltaTime;
            float spinY = crashSpinSpeed * Time.deltaTime;
            playerCar.transform.Rotate(0f, spinY, 0f, Space.World);
            Vector3 rot = playerCar.transform.eulerAngles;
            float wobble = Mathf.Sin(timer * 20f) * 5f;
            playerCar.transform.rotation = Quaternion.Euler(0f, rot.y, wobble);
            yield return null;
        }
        EndRun(false);
    }

    private void EndRun(bool win)
    {
        runEnded = true;

        if (resultTextRoutine != null)
        {
            StopCoroutine(resultTextRoutine);
            resultTextRoutine = null;
        }

        if (resultText != null)
        {
            resultText.text = "";
            resultText.gameObject.SetActive(false);
        }

        if (topLeftUI != null)
            topLeftUI.SetActive(false);

        if (win)
        {
            LevelGameplayManager levelManager = FindFirstObjectByType<LevelGameplayManager>();
            if (levelManager != null)
            {
                levelManager.CompleteLevel();
            }
        }

        if (winScreen) winScreen.SetActive(win);
        if (loseScreen) loseScreen.SetActive(!win);

        UpdateResultButtons(win);
        StartCoroutine(PlayEndSound(win));
    }

    private IEnumerator PlayEndSound(bool win)
    {
        if (audioSource != null)
        {
            if (win && winSound != null)
                audioSource.PlayOneShot(winSound);
            else if (!win && failSound != null)
                audioSource.PlayOneShot(failSound);
        }
        yield return new WaitForSecondsRealtime(0.5f);
    }

    public void ReturnToLevelSelect()
    {
        Time.timeScale = 1f;
        Manager.returnToLevelSelect = true;
        SceneManager.LoadScene("Math Runner 1.0");
    }

    public void RedoLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Math Runner 1.0 Game Assets");
    }

    public void NextLevel()
    {
        LevelGameplayManager levelManager = FindFirstObjectByType<LevelGameplayManager>();
        if (levelManager == null) return;

        int totalLevels = levelManager.GetCurrentCategoryLevelCount();
        if (LevelSelectionMenuManager.selectedLevelIndex >= totalLevels - 1) return;

        Time.timeScale = 1f;
        LevelSelectionMenuManager.selectedLevelIndex++;
        SceneManager.LoadScene("Math Runner 1.0 Game Assets");
    }

    private void UpdateResultButtons(bool win)
    {
        bool canGoBack = LevelSelectionMenuManager.selectedLevelIndex > 0;
        foreach (Button btn in backButtons)
        {
            if (btn != null) btn.interactable = canGoBack;
        }

        LevelGameplayManager levelManager = FindFirstObjectByType<LevelGameplayManager>();
        int totalLevels = 0;
        if (levelManager != null)
            totalLevels = levelManager.GetCurrentCategoryLevelCount();

        bool canGoNext = win && LevelSelectionMenuManager.selectedLevelIndex < totalLevels - 1;
        foreach (Button btn in nextButtons)
        {
            if (btn != null) btn.gameObject.SetActive(canGoNext);
        }
    }

    private void UpdateUI()
    {
        if (correctText)
            correctText.text = correctCount + " / " + maxCorrect;

        if (wrongText)
            wrongText.text = wrongCount + " / " + maxWrong;
    }

    IEnumerator ShowResultText(string message)
    {
        if (resultText == null) yield break;
        resultText.text = message;
        resultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        resultText.gameObject.SetActive(false);
        resultTextRoutine = null;
    }

    IEnumerator FlashColor(Color color)
    {
        float duration = 0.5f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, timer / duration);
            if (screenFlash)
                screenFlash.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        if (screenFlash) screenFlash.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FlashLight(Image light)
    {
        if (light == null) yield break;
        light.color = lightOn;
        yield return new WaitForSeconds(0.4f);
        light.color = lightOff;
    }
}