using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerInfo : MonoBehaviour
{
    public Animator animator;

    private PlayerController playerController;

    public float maxHealth = 100;
    public float currentHealth;

    public float knockbackForce = 5f;
    public float invincibilityDuration = 1f;
    private bool isInvincible = false;
    private bool isDead = false; // Yasýyor mu

    public Color damageColor = Color.red;
    private Color originalColor;
    public float colorChangeDuration = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;

    public CanvasGroup gameOverCanvasGroup;
    public Button restartButton;

    public ShakeData PlayerDamageShakeData;

    public AudioSource damageSound;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        Time.timeScale = 1f;//zamaný devam ettir

        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        originalColor = spriteRenderer.color;

        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; //oyuncu olduyse hasar almasin

        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(20, collision.transform);
        }

        if (collision.gameObject.CompareTag("Boss") && !isInvincible)
        {
            TakeDamage(100, collision.transform);
        }
    }

    void TakeDamage(float damage, Transform enemyTransform)
    {
        currentHealth -= damage;
        Debug.Log("Current Health: " + currentHealth);

        // Hasar sesi çalma
        if (damageSound != null)
        {
            damageSound.Play();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Vector2 knockbackDirection = (transform.position - enemyTransform.position);
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            StartCoroutine(InvincibilityCoroutine());
            StartCoroutine(ChangeColorCoroutine());

            CameraShakerHandler.Shake(PlayerDamageShakeData);
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    IEnumerator ChangeColorCoroutine()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorChangeDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (isDead) return; 

        isDead = true;
        Debug.Log("Player Died!");
        animator.SetTrigger("Die");

        ResetSavedData();

        if (gameOverCanvasGroup != null)
        {
            StartCoroutine(FadeInGameOver());
        }
    }

    void ResetSavedData()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            // Varsayýlan deðerlerle kaydý sýfýrla
            Vector3 defaultPosition = Vector3.zero; // Baþlangýç pozisyonuna sýfýrla
            float defaultHealth = maxHealth; // Maksimum saðlýða sýfýrla
            bool defaultHasWeapon = false; // Silahý sýfýrla

            saveManager.SavePlayerPosition(defaultPosition, defaultHealth, defaultHasWeapon);
            Debug.Log("Kaydedilen veriler sýfýrlandý.");
        }
    }

    IEnumerator FadeInGameOver()
    {
        float fadeDuration = 2f;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
        gameOverCanvasGroup.blocksRaycasts = true;
        gameOverCanvasGroup.interactable = true;

        // Zamaný yavaþlat
        yield return StartCoroutine(SlowDownTime());
    }

    IEnumerator SlowDownTime()
    {
        float slowDuration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < slowDuration)
        {
            timeElapsed += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(1f, 0f, timeElapsed / slowDuration);
            yield return null;
        }

        Time.timeScale = 0f; // Zamaný tamamen durdur
    }
}
