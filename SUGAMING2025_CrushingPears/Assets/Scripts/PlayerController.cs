using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


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
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    public bool m_FacingRight = true;

    public Image StaminaBar;

    [Header("Stamina bar Settings")]
    [SerializeField] private float maxStamin;
    [SerializeField] private float currStamina;
    [SerializeField] private float moveCost = 10f;
    [SerializeField] private float chargeRate = 5f;

    private Coroutine recharge;

    private PlayerStats playerStats;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

       
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            currStamina -= moveCost * Time.deltaTime;
            if (currStamina < 0)
            {
                currStamina = 0;
            }
            StaminaBar.fillAmount = currStamina / maxStamin;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) { 

             if(jumpTimeCounter > 0)
              {
                 rb.velocity = new Vector2(rb.velocity.x, jumpForce*jumpTimeMultiplier);
                 jumpTimeCounter -= Time.deltaTime;
              }
            else
            {
                isJumping=false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {

        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput != 0f) {
            currStamina -= moveCost * Time.deltaTime;
            if (currStamina < 0)
            {
                currStamina = 0;
            }
            StaminaBar.fillAmount = currStamina / maxStamin;

            if(recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }

        if (moveInput > 0f && !m_FacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && m_FacingRight) {
            Flip();
        }
        
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while(currStamina < maxStamin)
        {
            currStamina += chargeRate / 10f;
            if(currStamina > maxStamin)
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

        // Better than Rotate - more predictable
        transform.localScale = new Vector3(
            m_FacingRight ? 1 : -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

}
    

