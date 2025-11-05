using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour
{
    public GameObject instructionPanel;
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ✅ Load the Main Menu scene
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("scene.ui"); // Replace with your main menu scene name
    }

    // ✅ Load the Game/Play scene
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Replace with your game scene name
    }

    // ✅ Exit the game (works only in build)
    public void ExitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

    // ✅ Pause the game
    public void PauseGame()
    {

        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    // ✅ Resume the game
    public void ResumeGame()
    {

        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
    public void InstructionPanel()
    {
        instructionPanel.SetActive(true);
    }

    public void Instructionclose()
    {
        instructionPanel.SetActive(false);
    }

}