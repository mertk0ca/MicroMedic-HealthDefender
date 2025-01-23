using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class ParticleControllerCar : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;

    [Range(0, 10)]
    [SerializeField] int speedToReach;

    [Range(0, 0.2f)]
    [SerializeField] float formationPeriod;

    [SerializeField] Rigidbody2D playerRb;

    float counter;

    public ShakeData FallShakeData;

    bool isOnGround;

    private void Update()
    {
        counter += Time.deltaTime;

        // Oyuncu yerdeyse ve hareket ediyorsa partikülleri oynat
        if (isOnGround && Mathf.Abs(playerRb.velocity.x) > speedToReach)
        {
            if (counter > formationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            isOnGround = true;
            fallParticle.Play();
            //CameraShakerHandler.Shake(FallShakeData);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
}
