using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelProgressManager.LevelType menuType;
    public LevelObject[] levelObjects;

    public static int selectedLevelIndex;
    public static LevelProgressManager.LevelType selectedType;

    void OnEnable()
    {
        RefreshMenu();
    }

    public void RefreshMenu()
    {
        bool categoryUnlocked = LevelProgressManager.IsCategoryUnlocked(menuType);

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (levelObjects[i] == null || levelObjects[i].levelButton == null)
                continue;

            bool unlocked = categoryUnlocked && LevelProgressManager.IsLevelUnlocked(menuType, i);
            levelObjects[i].levelButton.interactable = unlocked;
        }
    }

    public void OnClickLevel(int levelIndex)
    {
        if (!LevelProgressManager.IsCategoryUnlocked(menuType))
            return;

        if (!LevelProgressManager.IsLevelUnlocked(menuType, levelIndex))
            return;

        selectedType = menuType;
        selectedLevelIndex = levelIndex;

        SceneManager.LoadScene("Math Runner 1.0 Game Assets");
    }
}