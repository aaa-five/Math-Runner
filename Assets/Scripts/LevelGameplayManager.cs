using UnityEngine;

public class LevelGameplayManager : MonoBehaviour
{
    [System.Serializable]
    public class CategoryLevels
    {
        public LevelProgressManager.LevelType levelType;
        public GameObject[] levels;
    }

    public CategoryLevels[] categoryLevels;

    void Start()
    {
        DisableAllLevels();

        LevelProgressManager.LevelType currentType = LevelSelectionMenuManager.selectedType;
        int levelIndex = LevelSelectionMenuManager.selectedLevelIndex;

        foreach (CategoryLevels category in categoryLevels)
        {
            if (category.levelType == currentType)
            {
                ActivateLevel(category.levels, levelIndex);
                break;
            }
        }
    }

    void ActivateLevel(GameObject[] levelArray, int index)
    {
        if (levelArray == null || levelArray.Length == 0)
            return;

        if (index >= 0 && index < levelArray.Length)
            levelArray[index].SetActive(true);
    }

    void DisableAllLevels()
    {
        foreach (CategoryLevels category in categoryLevels)
        {
            if (category.levels == null)
                continue;

            foreach (GameObject lvl in category.levels)
            {
                if (lvl != null)
                    lvl.SetActive(false);
            }
        }
    }

    public void CompleteLevel()
    {
        LevelProgressManager.LevelType currentType = LevelSelectionMenuManager.selectedType;
        int currentLevelIndex = LevelSelectionMenuManager.selectedLevelIndex;

        LevelProgressManager.UnlockNextLevel(currentType, currentLevelIndex);

        GameObject[] levels = GetLevelsForType(currentType);
        if (levels != null && currentLevelIndex >= levels.Length - 1)
        {
            LevelProgressManager.LevelType? next = LevelProgressManager.GetNextCategory(currentType);
            if (next.HasValue)
            {
                LevelProgressManager.UnlockCategory(next.Value);
            }
        }
    }

    GameObject[] GetLevelsForType(LevelProgressManager.LevelType type)
    {
        foreach (CategoryLevels category in categoryLevels)
        {
            if (category.levelType == type)
                return category.levels;
        }

        return null;
    }

    public int GetCurrentCategoryLevelCount()
    {
        GameObject[] levels = GetLevelsForType(LevelSelectionMenuManager.selectedType);
        return levels != null ? levels.Length : 0;
    }
}