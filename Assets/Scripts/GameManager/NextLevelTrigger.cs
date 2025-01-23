using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI için gerekli

public class NextLevelTrigger : MonoBehaviour
{
    public string targetSceneName; // Oynatýlacak sahnenin adý
    public CanvasGroup fadeCanvasGroup; // Ekran kararmasý için bir Canvas Group
    public float fadeDuration = 1f; // Kararma süresi

    private bool isFading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        // Ekran kararmasý
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        // Sahne yükle
        SceneManager.LoadScene(targetSceneName);
    }
}
