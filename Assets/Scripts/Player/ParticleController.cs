using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;
    [SerializeField] ParticleSystem fallParticle;

    [Range(0, 10)]
    [SerializeField] int speedToReach;

    [Range(0, 0.2f)]
    [SerializeField] float formationPeriod;

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] PlayerController playerController;

    float counter;

    public ShakeData FallShakeData;

    private void Update()
    {
        counter += Time.deltaTime;

        if (playerController.isGrounded && Mathf.Abs(playerRb.velocity.x) > speedToReach)
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
        if(other.CompareTag("Ground") || other.CompareTag("Trampoline"))
        {
            fallParticle.Play();
            //CameraShakerHandler.Shake(FallShakeData);
        }
    }
}
