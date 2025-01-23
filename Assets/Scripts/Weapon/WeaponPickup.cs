using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    // AudioSource referansý
    public AudioSource pickupAudioSource; // Ses efekti için AudioSource

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerShooting playerShooting = collision.GetComponent<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.GetComponent<PlayerShooting>().hasWeapon = true;

                // Silah alýndýðýnda ses çal
                if (pickupAudioSource != null)
                {
                    pickupAudioSource.Play(); // Silah alma sesini çal
                }
            }
            Destroy(gameObject);
        }
    }
}
