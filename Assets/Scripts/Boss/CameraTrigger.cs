using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cinemachine kamera referansý
    [SerializeField] private float newOrthoSize = 5f; // Yeni kamera boyutu
    [SerializeField] private Vector3 newCameraOffset; // Yeni kamera offset deðeri
    [SerializeField] private float transitionDuration = 1f; // Geçiþ süresi

    [SerializeField] private GameObject healthBar; // Canvas üzerindeki can barý objesi
    [SerializeField] private TMP_Text messageText; // TextMeshPro objesi
    [SerializeField] private float messageDisplayDuration = 3f; // Mesajýn ekranda kalma süresi

    private float originalOrthoSize; // Eski kamera boyutu
    private Vector3 originalCameraOffset; // Eski kamera offset deðeri
    private CinemachineTransposer transposer; // Kameranýn Transposer bileþeni
    private Coroutine currentCoroutine; // Mevcut çalýþan Coroutine referansý
    private Coroutine messageCoroutine; // Mesaj için çalýþan Coroutine referansý

    [SerializeField] private AudioSource areaEnterSound; // Alana girildiðinde çalacak ses

    void Start()
    {
        // Baþlangýç deðerlerini kaydet
        if (virtualCamera != null)
        {
            originalOrthoSize = virtualCamera.m_Lens.OrthographicSize;

            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                originalCameraOffset = transposer.m_FollowOffset;
            }
        }

        // Can barýný baþlangýçta gizle
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }

        // Mesajý baþlangýçta gizle
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && virtualCamera != null)
        {
            // Mevcut Coroutine'i durdur ve yeni ayarlarý uygula
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ChangeCameraSettings(newOrthoSize, newCameraOffset));

            // Can barýný görünür yap
            if (healthBar != null)
            {
                healthBar.SetActive(true);
            }

            // Mesajý ekrana getir
            if (messageText != null)
            {
                if (messageCoroutine != null)
                {
                    StopCoroutine(messageCoroutine);
                }
                messageCoroutine = StartCoroutine(DisplayMessage());
            }

            // Alana girildiðinde ses çal
            if (areaEnterSound != null)
            {
                areaEnterSound.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && virtualCamera != null)
        {
            // Mevcut Coroutine'i durdur ve eski ayarlarý uygula
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ChangeCameraSettings(originalOrthoSize, originalCameraOffset));

            // Can barýný gizle
            if (healthBar != null)
            {
                healthBar.SetActive(false);
            }
        }
    }

    private IEnumerator ChangeCameraSettings(float targetOrthoSize, Vector3 targetOffset)
    {
        float elapsedTime = 0f;
        float startOrthoSize = virtualCamera.m_Lens.OrthographicSize;
        Vector3 startOffset = transposer.m_FollowOffset;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Kamera boyutunu ve offset'i kademeli olarak deðiþtir
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startOrthoSize, targetOrthoSize, elapsedTime / transitionDuration);
            transposer.m_FollowOffset = Vector3.Lerp(startOffset, targetOffset, elapsedTime / transitionDuration);

            yield return null;
        }

        // Nihai deðerleri ayarla
        virtualCamera.m_Lens.OrthographicSize = targetOrthoSize;
        transposer.m_FollowOffset = targetOffset;

        currentCoroutine = null; // Coroutine tamamlandýðýnda sýfýrla
    }

    private IEnumerator DisplayMessage()
    {
        // Mesajý aktif et
        messageText.gameObject.SetActive(true);

        // Yavaþça belirme
        Color originalColor = messageText.color;
        originalColor.a = 0f;
        messageText.color = originalColor;

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            originalColor.a = Mathf.Lerp(0f, 1f, elapsedTime);
            messageText.color = originalColor;
            yield return null;
        }

        // Mesaj ekranda beklesin
        yield return new WaitForSeconds(messageDisplayDuration);

        // Yavaþça kaybolma
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            originalColor.a = Mathf.Lerp(1f, 0f, elapsedTime);
            messageText.color = originalColor;
            yield return null;
        }

        // Mesajý gizle
        messageText.gameObject.SetActive(false);
    }
}
