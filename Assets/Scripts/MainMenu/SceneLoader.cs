using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // Yüklenecek sahnenin adý
    public Image fadeImage; // Fade efekti için kullanýlan Image
    public float fadeDuration = 1f; // Fade süresi
    public string saveFileName = "saveData.json"; // Kaydedilen dosyanýn adý (örnek: playerSave.json)

    // Bu metodu butonun OnClick() event'ine baðlayabilirsiniz
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Eski kaydý sil
            DeleteSaveFile();

            // Fade efekti ve sahne yükleme
            StartCoroutine(FadeOutAndLoadScene());
        }
        else
        {
            Debug.LogError("Scene name is not set!");
        }
    }

    // Fade Out iþlemi ve sahne yükleme
    private IEnumerator FadeOutAndLoadScene()
    {
        Time.timeScale = 1f;
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // Fade Out iþlemi
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Yeni sahneyi yükle
        SceneManager.LoadScene(sceneName);
    }

    // Eski kaydý silme fonksiyonu
    private void DeleteSaveFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        // Dosya varsa sil
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }
}
