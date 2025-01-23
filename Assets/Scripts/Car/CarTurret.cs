using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

public class CarTurret : MonoBehaviour
{
    public Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    public GameObject liquidPrefab;
    public Transform firePoint;
    public Image CooldownIndicator;

    public float liquidSpeed = 10f;  //firlatilan sivinin hizi
    public float fireRate = 0.2f; //ates etme orani
    private float nextFireTime = 0f; // bir sonraki firlatma zamani

    public float maxFireTime = 1f; // maksimum ates edebilme suresi
    public float cooldownTime = 5f; // tekrar ates etmeden once bekleme suresi
    private bool isOnCooldown = false; // bekleme suresinde mi
    private bool isFiring = false; // ates ediyor mu

    public ShakeData FluidReleaseShakeData;

    public AudioSource fireSound;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CooldownIndicator.fillAmount = 1f; // baslangicta bar dolu
        CooldownIndicator.color = Color.green; // baslangicta yesil yap

        if (fireSound != null)
        {
            fireSound.loop = false; // donguye alma
        }
    }

    void Update()
    {
        RotateTurret();

        //cooldownda ise barin rengini kirmizi ve beyaz arasýnda ping pong efektiyle degistir
        if (isOnCooldown)
        {
            float t = Mathf.PingPong(Time.time * 2, 1f);
            CooldownIndicator.color = Color.Lerp(Color.red, Color.white, t);
            return;
        }

        //cooldownda degilse barin rengini yesil ve beyaz arasýnda ping pong efektiyle degistir
        else
        {
            float t = Mathf.PingPong(Time.time / 2, 1f);
            CooldownIndicator.color = Color.Lerp(Color.white, Color.green, t);
        }

        // sol tiklandiginda 1 saniye boyunca ates et
        if (Input.GetMouseButtonDown(0) && !isFiring)
        {
            StartCoroutine(FireForDuration());

            if (fireSound != null)
            {
                fireSound.Play(); // ates etme sesini cal
            }
        }
    }

    //arabanin ustundeki turreti fare pozisyonuna gore hareket ettir
    private void RotateTurret()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = mousePosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipY = true; //eger fare arabanin arkasina gecerse turrei o yone dondur
        }
        else
        {
            spriteRenderer.flipY = false; // normal pozisyon
        }
    }

    //sivi atesleme
    private void FireLiquid()
    {
        GameObject liquid = Instantiate(liquidPrefab, firePoint.position, firePoint.rotation); //liquid prefabini fire rate'e gore olustur
        CameraShakerHandler.Shake(FluidReleaseShakeData);

        float randomScale = Random.Range(0.02f, 0.06f);  //liquid prefablarinin boyutunu rastegele olustur
        liquid.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        Rigidbody2D rb = liquid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * liquidSpeed; // firlatma yonu ve hizi
        }

    }

    //bir saniye boyunca ates et
    private IEnumerator FireForDuration()
    {
        isFiring = true;

        float elapsedTime = 0f;
        while (elapsedTime < maxFireTime)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                FireLiquid();
            }

            // bari yavasca azalt
            CooldownIndicator.fillAmount = 1f - (elapsedTime / maxFireTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartCooldown());
        isFiring = false;
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;

        float elapsedTime = 0f;
        while (elapsedTime < cooldownTime)
        {
            // gostergeyi yavasca doldur
            CooldownIndicator.fillAmount = elapsedTime / cooldownTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CooldownIndicator.fillAmount = 1f; // Göstergeyi tam dolu yap
        isOnCooldown = false;
    }
}
