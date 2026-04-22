using UnityEngine;

public static class LevelProgressManager
{
    public enum LevelType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public static string GetLevelKey(LevelType type)
    {
        return type.ToString() + "_UnlockedIndex";
    }

    public static string GetCategoryKey(LevelType type)
    {
        return type.ToString() + "_CategoryUnlocked";
    }

    public static int GetUnlockedLevelIndex(LevelType type)
    {
        return PlayerPrefs.GetInt(GetLevelKey(type), 0);
    }

    public static void SetUnlockedLevelIndex(LevelType type, int index)
    {
        PlayerPrefs.SetInt(GetLevelKey(type), index);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(LevelType type, int levelIndex)
    {
        return levelIndex <= GetUnlockedLevelIndex(type);
    }

    public static void UnlockNextLevel(LevelType type, int completedLevelIndex)
    {
        int currentUnlocked = GetUnlockedLevelIndex(type);

        if (completedLevelIndex == currentUnlocked)
        {
            SetUnlockedLevelIndex(type, currentUnlocked + 1);
        }
        Debug.Log("Unlocked index now: " + GetUnlockedLevelIndex(type));
    }

    public static bool IsCategoryUnlocked(LevelType type)
    {
        if (type == LevelType.Addition)
            return true;

        return PlayerPrefs.GetInt(GetCategoryKey(type), 0) == 1;
    }

    public static void UnlockCategory(LevelType type)
    {
        PlayerPrefs.SetInt(GetCategoryKey(type), 1);
        PlayerPrefs.Save();
    }

    public static LevelType? GetNextCategory(LevelType current)
    {
        switch (current)
        {
            case LevelType.Addition:
                return LevelType.Subtraction;
            case LevelType.Subtraction:
                return LevelType.Multiplication;
            case LevelType.Multiplication:
                return LevelType.Division;
            default:
                return null;
        }
    }

    public static void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}