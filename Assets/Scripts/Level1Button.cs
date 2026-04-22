using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class Level1Button : MonoBehaviour
{
    // Method to load the game scene by name
    public void LoadGameScene()
    {
        // Replace with your exact game scene name
        SceneManager.LoadScene("Math Runner 1.0 Game Assets"); 
    }
    
    // Alternative method: Load next scene by index
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}