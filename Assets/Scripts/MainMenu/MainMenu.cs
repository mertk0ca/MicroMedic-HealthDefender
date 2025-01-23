using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelPanel;

    public void OnContinueButtonPressed()
    {
        Time.timeScale = 1.0f;
        // sahneyi yukle
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitButtonPressed()
    {
        Time.timeScale = 1.0f;
        // oyundan cik
        Application.Quit();
    }

    public void OnLevelButtonPressed()
    {
        // paneli goster
        if (levelPanel != null)
        {
            levelPanel.SetActive(true);
        }
    }

    public void OnBackButtonPressed()
    {
        // paneli kapat
        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }
    }
}