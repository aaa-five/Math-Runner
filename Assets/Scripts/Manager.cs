using UnityEngine;

public class Manager : MonoBehaviour
{
    public static bool returnToLevelSelect = false;

    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject settingsMenu;

    public GameObject levelSelectMenu;                 // Red = Addition
    public GameObject levelSelectMenuSubtraction;      // Blue
    public GameObject levelSelectMenuMultiplication;   // Green
    public GameObject levelSelectMenuDivision;         // Purple

    void Start()
    {
        if (returnToLevelSelect)
        {
            returnToLevelSelect = false;
            OpenCurrentLevelSelect();
        }
        else
        {
            BackToMain();
        }
    }

    public void OpenLevels()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
        settingsMenu.SetActive(false);
        CloseAllLevelSelectMenus();
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(true);
        CloseAllLevelSelectMenus();
    }

    public void BackToMain()
    {
        mainMenu.SetActive(true);
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        CloseAllLevelSelectMenus();
    }

    public void BackToLevels()
    {
        mainMenu.SetActive(false);
        levelsMenu.SetActive(true);
        settingsMenu.SetActive(false);
        CloseAllLevelSelectMenus();
    }

    public void OpenRed()
    {
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        CloseAllLevelSelectMenus();
        levelSelectMenu.SetActive(true);
    }

    public void OpenBlue()
    {
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        CloseAllLevelSelectMenus();
        levelSelectMenuSubtraction.SetActive(true);
    }

    public void OpenGreen()
    {
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        CloseAllLevelSelectMenus();
        levelSelectMenuMultiplication.SetActive(true);
    }

    public void OpenPurple()
    {
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        CloseAllLevelSelectMenus();
        levelSelectMenuDivision.SetActive(true);
    }

public void OpenCurrentLevelSelect()
{
    mainMenu.SetActive(false);
    levelsMenu.SetActive(false);
    settingsMenu.SetActive(false);
    CloseAllLevelSelectMenus();

    GameObject activeMenu = null;

    switch (LevelSelectionMenuManager.selectedType)
    {
        case LevelProgressManager.LevelType.Addition:
            activeMenu = levelSelectMenu;
            break;

        case LevelProgressManager.LevelType.Subtraction:
            activeMenu = levelSelectMenuSubtraction;
            break;

        case LevelProgressManager.LevelType.Multiplication:
            activeMenu = levelSelectMenuMultiplication;
            break;

        case LevelProgressManager.LevelType.Division:
            activeMenu = levelSelectMenuDivision;
            break;
    }

    if (activeMenu != null)
    {
        activeMenu.SetActive(true);

        // ✅ FORCE REFRESH
        LevelSelectionMenuManager menu = activeMenu.GetComponent<LevelSelectionMenuManager>();
        if (menu != null)
        {
            menu.RefreshMenu();
        }
    }
}

    public void QuitGame()
    {
        Application.Quit();
    }

    void CloseAllLevelSelectMenus()
    {
        levelSelectMenu.SetActive(false);
        levelSelectMenuSubtraction.SetActive(false);
        levelSelectMenuMultiplication.SetActive(false);
        levelSelectMenuDivision.SetActive(false);
    }
}
