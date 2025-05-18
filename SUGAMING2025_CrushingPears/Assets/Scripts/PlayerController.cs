using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
// using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float maxJumpTime = 0.3f;
    [SerializeField] private float jumpTimeMultiplier = 0.8f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    public bool isGrounded;
    public bool isJumping;
    private bool canAirJump = false;
    private float jumpTimeCounter;
    public bool m_FacingRight = true;

    public Image StaminaBar;

    [Header("Stamina bar Settings")]
    [SerializeField] private float maxStamin;
    [SerializeField] private float currStamina;
    [SerializeField] private float moveCost = 10f;
    [SerializeField] private float chargeRate = 5f;

    [Header("Grabbing Mechanics")]
    public bool collidingWithGrab = false;
    public Collider2D ledge;

    [SerializeField] private PhysicsMaterial2D slipperyMaterial;
    [SerializeField] private PhysicsMaterial2D superRoughMaterial;
    [SerializeField] private bool grabbing;
    [SerializeField] private float climbRate;
    [SerializeField] private bool jumpCooling;

    private Animator animator;

    private Coroutine recharge;

    private PlayerStats playerStats;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        //Debug.Log("Current Y Velocity: " + rb.velocity.y);
        animator.SetFloat("verticalVelocity", rb.velocity.y);
        animator.SetFloat("horizontalVelocity", Math.Abs(rb.velocity.x));
        Debug.Log("Current Y Velocity: " + rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            jumpCooling = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || canAirJump))
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
            canAirJump = false;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            currStamina -= moveCost * Time.deltaTime;
            if (currStamina < 0)
            {
                currStamina = 0;
            }
            StaminaBar.fillAmount = currStamina / maxStamin;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {

            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeMultiplier);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) && collidingWithGrab)
        {
            canAirJump = true;
            ledge.GetComponent<PolygonCollider2D>().sharedMaterial = superRoughMaterial;
            grabbing = true;
        }

        if (ledge != null && currStamina<=0f)
            {
                ledge.GetComponent<PolygonCollider2D>().sharedMaterial = slipperyMaterial;
                if (!jumpCooling)
                {
                    jumpCooling = true;
                    JumpCooldown(5000);
                }
                Debug.Log("Jump Cooldown Called");
            }

        if (Input.GetKeyUp(KeyCode.Q) && collidingWithGrab)
        {
            grabbing = false;
            if (ledge != null)
            {
                ledge.GetComponent<PolygonCollider2D>().sharedMaterial = slipperyMaterial;
                if (!jumpCooling)
                {
                    jumpCooling = true;
                    JumpCooldown(5000);
                }
                Debug.Log("Jump Cooldown Called");
            }
        }

        if (!collidingWithGrab && !isGrounded)
        {
            grabbing = false;
            if (ledge != null)
            {
                ledge.GetComponent<PolygonCollider2D>().sharedMaterial = slipperyMaterial;
                if (!jumpCooling)
                {
                    jumpCooling = true;
                    JumpCooldown(5000);
                }
                Debug.Log("Jump Cooldown Called");
            }
        }


        if (Input.GetKeyDown(KeyCode.E) && grabbing)
        {
            Vector2 playerTransform = new Vector2();
            playerTransform.Set(transform.localPosition.x, transform.localPosition.y + climbRate);
            transform.SetPositionAndRotation(playerTransform, transform.rotation);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }

    void FixedUpdate()
    {

        float moveInput = Input.GetAxis("Horizontal");
        if (grabbing)
        {
            jumpCooling = false;
            currStamina -= moveCost * Time.deltaTime;
            if (currStamina < 0)
            {
                currStamina = 0;
            }
            StaminaBar.fillAmount = currStamina / maxStamin;

            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }

        if (moveInput > 0f && !m_FacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && m_FacingRight)
        {
            Flip();
        }

        rb.velocity = new Vector2(grabbing?(1f * moveSpeed * (m_FacingRight?1:-1)):(moveInput * moveSpeed), rb.velocity.y);
    }

    private async void JumpCooldown(int milliseconds)
    {
        Debug.Log("Jump Cooldown Starting");
        await Task.Delay(milliseconds);
        Debug.Log("Jump Cooldown Ended");
        canAirJump = false;
    }



    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (currStamina < maxStamin)
        {
            currStamina += chargeRate / 10f;
            if (currStamina > maxStamin)
            {
                currStamina = maxStamin;
            }
            StaminaBar.fillAmount = currStamina / maxStamin;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

       
        transform.localScale = new Vector3(
            m_FacingRight ? 1 : -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }
    public void DisablePlayer()
    {
        enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void EnablePlayer()
    {
        enabled = true;
        GetComponent<Collider2D>().enabled = true;
        rb.isKinematic = false;
        GetComponent<SpriteRenderer>().enabled = true;
        currStamina = maxStamin;
        StaminaBar.fillAmount = 1f;
    }
}
