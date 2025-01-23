using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // mermi prefabi
    public Transform firePoint; // merminin fýrlatýlacaðý yer
    public float bulletSpeed = 10f; // mermi hýzý
    public bool hasWeapon = false;
    public int ammo = 50;

    private float cooldownDuration = 0.5f; // cooldown süresi
    private bool isCooldown = false; // cooldown durumu

    // Ses efekti için AudioSource
    public AudioSource shootAudioSource; // AudioSource bileþeni

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && hasWeapon) // silah varsa
        {
            if (!isCooldown) // cooldown deðilse
            {
                if (ammo > 0)
                {
                    Shoot();
                    ammo -= 1;
                    StartCoroutine(ShootCooldown()); // cooldown coroutine baþlat
                    Debug.Log("Kalan Mermi: " + ammo);
                }
                else
                {
                    Debug.Log("Mermi Bitti!");
                }
            }
        }
    }

    private IEnumerator ShootCooldown()
    {
        isCooldown = true; // cooldown durumunu true yap
        yield return new WaitForSeconds(cooldownDuration); // belirtilen süre boyunca bekle
        isCooldown = false; // cooldown durumunu false yap
    }

    void Shoot()
    {
        // Mermi oluþtur
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.right * bulletSpeed; // sað yöne ateþ et

        // Ses çalma
        if (shootAudioSource != null)
        {
            shootAudioSource.Play(); // Ateþ etme sesini çal
        }
    }
}
