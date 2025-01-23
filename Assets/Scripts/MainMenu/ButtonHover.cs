using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // TextMeshPro bileþeni
    public VertexGradient normalGradient; // Varsayýlan gradient
    public VertexGradient hoverGradient;  // Hover sýrasýnda gradient
    public float fadeDuration = 0.5f;     // Geçiþ süresi
    public float normalCharacterSpacing = 0f; // Varsayýlan karakter boþluðu
    public float hoverCharacterSpacing = 10f; // Hover sýrasýnda karakter boþluðu
    public AudioSource hoverSound;       // Hover sesi için AudioSource
    public AudioSource clickSound;       // Týklama sesi için AudioSource

    private Coroutine currentGradientCoroutine;   // Aktif gradient Coroutine
    private Coroutine currentSpacingCoroutine;    // Aktif karakter boþluðu Coroutine

    private void Awake()
    {
        // Vertex gradient özelliðini aktif et
        buttonText.enableVertexGradient = true;
    }

    // Fare butonun üzerine geldiðinde çalýþýr
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartGradientTransition(hoverGradient);
        StartCharacterSpacingTransition(hoverCharacterSpacing);

        // Hover sesi çal
        if (hoverSound != null)
        {
            hoverSound.Play();
        }
    }

    // Fare butondan ayrýldýðýnda çalýþýr
    public void OnPointerExit(PointerEventData eventData)
    {
        StartGradientTransition(normalGradient);
        StartCharacterSpacingTransition(normalCharacterSpacing);
    }

    // Butona týklandýðýnda ses çalýnmasýný saðlayan fonksiyon
    public void OnButtonClick()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

    private void StartGradientTransition(VertexGradient targetGradient)
    {
        if (currentGradientCoroutine != null)
        {
            StopCoroutine(currentGradientCoroutine);
        }
        currentGradientCoroutine = StartCoroutine(GradientTransition(targetGradient));
    }

    private void StartCharacterSpacingTransition(float targetSpacing)
    {
        if (currentSpacingCoroutine != null)
        {
            StopCoroutine(currentSpacingCoroutine);
        }
        currentSpacingCoroutine = StartCoroutine(CharacterSpacingTransition(targetSpacing));
    }

    private IEnumerator GradientTransition(VertexGradient targetGradient)
    {
        VertexGradient initialGradient = buttonText.colorGradient; // Mevcut gradient
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            buttonText.colorGradient = new VertexGradient(
                Color.Lerp(initialGradient.topLeft, targetGradient.topLeft, elapsedTime / fadeDuration),
                Color.Lerp(initialGradient.topRight, targetGradient.topRight, elapsedTime / fadeDuration),
                Color.Lerp(initialGradient.bottomLeft, targetGradient.bottomLeft, elapsedTime / fadeDuration),
                Color.Lerp(initialGradient.bottomRight, targetGradient.bottomRight, elapsedTime / fadeDuration)
            );
            yield return null;
        }

        buttonText.colorGradient = targetGradient;
    }

    private IEnumerator CharacterSpacingTransition(float targetSpacing)
    {
        float initialSpacing = buttonText.characterSpacing; // Mevcut karakter boþluðu
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            buttonText.characterSpacing = Mathf.Lerp(initialSpacing, targetSpacing, elapsedTime / fadeDuration);
            yield return null;
        }

        buttonText.characterSpacing = targetSpacing;
    }
}
