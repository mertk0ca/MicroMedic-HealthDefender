using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public Image weaponIcon;
    public Image AmmoIcon;
    public Image Mido;

    private playerInfo playerInfo;
    private PlayerShooting PlayerShooting;

    public Text AmmoText;

    private float targetHealth;

    void Start()
    {
        playerInfo = FindObjectOfType<playerInfo>();
        PlayerShooting = FindObjectOfType<PlayerShooting>();

        targetHealth = playerInfo.currentHealth;
    }

    void Update()
    {
        //hedef saglýk degeri degistiginde animasyonu baslat
        if (targetHealth != playerInfo.currentHealth)
        {
            targetHealth = playerInfo.currentHealth;
            StartCoroutine(UpdateHealthBar());
        }

        UpdateWeapon();
    }

    void UpdateWeapon()
    {
        if (PlayerShooting.hasWeapon)
        {
            AmmoIcon.enabled = true; //mermi iconu goster
            weaponIcon.enabled = true; //silah iconu goster
            Mido.enabled = false;

            AmmoText.text = Convert.ToString(PlayerShooting.ammo); //mermi miktarini texte yaz

            if (PlayerShooting.ammo == 0)
            {
                AmmoText.color = Color.red; //mermi bittiginde rengi kirmizi yap
            }
        }
        else
        {
            weaponIcon.enabled = false;
            AmmoIcon.enabled = false;
            Mido.enabled = true;
        }
    }

    IEnumerator UpdateHealthBar()
    {
        float currentHealth = healthBar.fillAmount * 100f; // mevcut saglik miktarini yuzdelik olarak al
        float duration = 0.3f; // animasyon suresi
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // zamani guncelle
            float newHealth = Mathf.Lerp(currentHealth, targetHealth, elapsedTime / duration); // gecis yap
            healthBar.fillAmount = newHealth / 100f; // saglik barini guncelle
            yield return null; // bir frame bekle
        }

        healthBar.fillAmount = targetHealth / 100f; // son degerle guncelle
    }
}
