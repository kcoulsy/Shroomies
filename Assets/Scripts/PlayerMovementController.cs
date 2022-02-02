using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D rb = null;
    private BoxCollider2D boxCollider2D = null;

    [SerializeField] LayerMask platformLayerMask;

    [SerializeField] float horizontalSpeed = 4f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float groundCheckHeight = 2f;
    [SerializeField] int currentJumps = 0;
    [SerializeField] int maxJumps = 2;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        HandleHorizontalMovement();
        HandleGravity();
        HandleJump();
    }



    private void HandleHorizontalMovement()
    {
        float horizonalMovement = Input.GetAxis("Horizontal");

        rb.velocity = new Vector3(horizonalMovement * horizontalSpeed, rb.velocity.y, 0);
    }

    private void HandleGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        bool isGrounded = IsGrounded();

        if (rb.velocity.y == 0)
        {
            currentJumps = 0;
        }

        bool canJump = (currentJumps > 0 || isGrounded);

        if (canJump && Input.GetButtonDown("Jump") && currentJumps < maxJumps)
        {
            currentJumps = currentJumps + 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, groundCheckHeight, platformLayerMask);

        // Color rayColor = Color.green;
        // if (raycastHit.collider == null)
        // {
        //     rayColor = Color.red;
        // }
        // Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + groundCheckHeight), rayColor);
        // Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + groundCheckHeight), rayColor);
        // Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y), Vector2.right * (boxCollider2D.bounds.extents.y), rayColor);
        // Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
    }
}
