using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private BoxCollider2D playerCollider;
    private SpriteRenderer spriteRenderer; //playerin spriteina ulasmak icin
    public Animator animator; //animatordeki parametreleri kullanmak icin

    private playerInfo playerInfo;

    private float h_Input;
    private bool jump_pressed = false;
    private int jumpCount = 0;

    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float jumpAmount = 3.5f;
    [SerializeField] private float raycastDistance = 0.8f;
    [SerializeField] private LayerMask groundLayer;

    public bool isGrounded = false;

    public Transform firePoint;

    [SerializeField] ParticleSystem fallParticle;

    [SerializeField] private AudioSource jumpSound; // ziplama ses efekti
    [SerializeField] private AudioSource landSound; // yere inis ses efekti
    [SerializeField] private AudioSource walkSound; // yurume ses efekti
    private bool isWalking = false; // yuruyor mu

    private void Awake()
    {
        playerRB = this.GetComponent<Rigidbody2D>();
        playerCollider = this.GetComponent<BoxCollider2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        playerInfo = FindObjectOfType<playerInfo>();
    }

    void Update()
    {
        GetInput();
        CheckInput();
        CheckGround();
        animator.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));

        // yuruyorsa yurume ses efektini cal
        if (Mathf.Abs(playerRB.velocity.x) > 0.1f && isGrounded)
        {
            if (!isWalking)
            {
                walkSound.Play();
                isWalking = true;
            }
        }
        else
        {
            if (isWalking)
            {
                walkSound.Stop();
                isWalking = false;
            }
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        flipImage();
    }

    void GetInput()
    {
        h_Input = Input.GetAxis("Horizontal"); //yatay duzlemde input al
    }

    void ApplyMovement()
    {
        if (playerInfo.currentHealth > 0)
        {
            playerRB.velocity = new Vector2(h_Input * movementSpeed, playerRB.velocity.y);

            if (jump_pressed && (isGrounded || jumpCount < 2))
            {
                jump_pressed = false;
                Jump();
            }
        }
    }

    void CheckInput()
    {
        h_Input = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumpCount < 1))
        {
            jump_pressed = true;
            fallParticle.Play();
            jumpSound.Play();
        }
    }

    void CheckGround()
    {
        Vector2 raycastOrigin = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 1f); //rayi karakterin altindan baslatmak icin
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer); //sadece groundlayera carpmasi icin

        Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance, Color.red);

        if (hit.collider != null)
        {
            if (!isGrounded) // yere deydiginde yere inis ses efektini cal
            {
                landSound.Play();
            }
            isGrounded = true;
            jumpCount = 0;
            animator.SetBool("isJumping", false);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }

    public void Jump()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpAmount);
        jumpCount++;
    }

    void flipImage() //playerin saga ve sola bakmasini saglamak icin ve firepoint objesinin yonunu degistirmek icin
    {
        if (h_Input > 0 && playerInfo.currentHealth > 0)
        {
            spriteRenderer.flipX = true;
            firePoint.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (h_Input < 0 && playerInfo.currentHealth > 0)
        {
            spriteRenderer.flipX = false;
            firePoint.localEulerAngles = new Vector3(0, 0, 180);
        }
    }
}
