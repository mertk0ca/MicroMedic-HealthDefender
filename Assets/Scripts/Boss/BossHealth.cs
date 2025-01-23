using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 500;  // boss maksimum can
    public int currentHealth;  //boss anlik can

    [SerializeField] private Renderer bossRenderer; // boss rengini degistirmek icin renderer bilesenini referans alma
    [SerializeField] private Color damageColor = Color.red;  //boss hasar aldýgýnda degisecek renk
    [SerializeField] private float damageFlashDuration = 0.3f;  //boss ne kadar sure kirmizi renkte kalacak

    private Color originalColor;

    [SerializeField] private GameObject platformToDestroy;  // boss oldugunde yok edilecek platforum
    [SerializeField] private Image healthBar;  // boss can bari
    [SerializeField] private float healthBarLerpSpeed = 2f;  //  boss cani azaldiginda can barindaki degisimin hizi

    private Coroutine healthBarCoroutine;  //  can bari azalmasini yavas sekilde yapmak icin ko-rutin

    [SerializeField] private AudioSource audioSource;  // boss hasar aldiginda ses cýkarmasi icin audio referansý

    void Start()
    {
        currentHealth = maxHealth;  //baslangicta anlik can degerini maksimum olarak atama

        if (bossRenderer != null)
        {
            originalColor = bossRenderer.material.color;  // boss orijinal rengi
        }

        UpdateHealthBarInstant();
    }

    public void TakeDamage(int damageAmount)  // boss hasar alma fonksiyonu
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (bossRenderer != null)
        {
            StartCoroutine(DamageFlash());
        }

        // Hasar alýndýðýnda ses çalma
        if (audioSource != null)
        {
            audioSource.Play(); // boss hasar alma ses efekti
        }

        UpdateHealthBarSmooth();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBarInstant()  //baslangicta boss cani full oldugu icin animasyonsuz olarak bari fuller
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateHealthBarSmooth()  //boss hasar aldiginda LerpHealthBar fonksiyonuyla animasyonlu bir sekilde cani azaltýr
    {
        if (healthBar != null)
        {
            if (healthBarCoroutine != null)
            {
                StopCoroutine(healthBarCoroutine);
            }
            healthBarCoroutine = StartCoroutine(LerpHealthBar());
        }
    }

    private IEnumerator LerpHealthBar()  // boss can barini animasyonlu bir sekilde hareket ettirmek
    {
        float targetFillAmount = (float)currentHealth / maxHealth;
        while (!Mathf.Approximately(healthBar.fillAmount, targetFillAmount))
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, Time.deltaTime * healthBarLerpSpeed);
            yield return null;
        }
    }

    private IEnumerator DamageFlash()  //boss hasar aldiginda rengini belirli bir sure kirmizi yapmak
    {
        bossRenderer.material.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        bossRenderer.material.color = originalColor;
    }

    private void Die()
    {
        if (platformToDestroy != null)
        {
            Destroy(platformToDestroy);  //boss oldugunde platformu yok et
        }

        if (healthBar != null)
        {
            healthBar.transform.parent.gameObject.SetActive(false);  //boss oldugunde can barini gizle
        }

        Destroy(gameObject);  // boss oldugunde objeyi yok et
    }

    public int GetCurrentHealth()  //baska script dosyalarinda anlik can degerine referans vermeyi kolaylastirmak icin
    {
        return currentHealth;
    }

    public int GetMaxHealth()  //baska script dosyalarinda maksimum can degerine referans vermeyi kolaylastirmak icin
    {
        return maxHealth;
    }
}
