using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D myBody;
    [SerializeField]
    private GameManager GM;
    public static int healthPoints = 3;

    [SerializeField]
    private float moveSpeed = 5f;

    private float horizontalMovement;

    private PlayerAnimation playerAnim;

    [SerializeField]
    private float normalJumpForce = 5f, doubleJumpForce = 5f;

    private float jumpForce = 5f;

    private RaycastHit2D groundCast;
    private BoxCollider2D boxCol2D;

    [SerializeField]
    private LayerMask groundMask;

    private bool canDoubleJump;
    private bool jumped;

    private Vector2 startPosition;
    
    [SerializeField]private AudioSource jumpSound;
    [SerializeField]private AudioSource deathSound;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        boxCol2D = GetComponent<BoxCollider2D>();
        
        canDoubleJump = true;
        startPosition = transform.position;
    }

    private void Update()
    {  
        horizontalMovement = Input.GetAxisRaw(TagManager.HORIZONTAL_MOVEMENT_AXIS);

        HandleAnimation();

        HandleJumping();

        CheckToDoubleJump();

        FromJumpToWalkOrIdle();

    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        
        if (horizontalMovement > 0)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
        else if (horizontalMovement < 0)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }
        else
        {
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
        }
    }

   void HandleAnimation()
{
    if (myBody.velocity.y == 0f)
    {
        playerAnim.PlayWalk(Mathf.Abs((int)myBody.velocity.x));
    }

    playerAnim.ChangeFacingDirection((int)myBody.velocity.x);
    playerAnim.PlayJumpAndFall((int)myBody.velocity.y);
}



    void HandleJumping()
    {

        if (Input.GetButtonDown(TagManager.JUMP_BUTTON))
        {
            if (IsGrounded())
            {
                jumpForce = normalJumpForce;
                Jump();
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    jumpForce = doubleJumpForce;
                    Jump();
                }
            }
        }

    }

    bool IsGrounded()
    {

        //groundCast = Physics2D.Raycast(boxCol2D.bounds.center,
        //    Vector2.down, boxCol2D.bounds.extents.y + 0.02f, groundMask);

        //Debug.DrawRay(boxCol2D.bounds.center,
        //    Vector2.down * (boxCol2D.bounds.extents.y + 0.02f), Color.red);

        groundCast = Physics2D.BoxCast(boxCol2D.bounds.center,
            boxCol2D.bounds.size, 0f, Vector2.down, 0.01f, groundMask);
        
        //Debug.DrawRay(boxCol2D.bounds.center + new Vector3(boxCol2D.bounds.extents.x, 0f),
        //    Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);

        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x, 0f),
        //    Vector2.down * (boxCol2D.bounds.extents.y + 0.01f), Color.red);

        //Debug.DrawRay(boxCol2D.bounds.center - new Vector3(boxCol2D.bounds.extents.x,
        //    boxCol2D.bounds.extents.y + 0.01f),
        //    Vector2.right * boxCol2D.bounds.size.x, Color.red);

        return groundCast.collider != null;

    }

    void Jump()
    {
        
        jumpSound.Play();
        myBody.velocity = Vector2.up * jumpForce;
        jumped = true;
    }

    void CheckToDoubleJump()
    {
        if (!canDoubleJump && myBody.velocity.y == 0f)
            canDoubleJump = true;
    }

    void FromJumpToWalkOrIdle()
    {

        if (jumped && myBody.velocity.y == 0f)
        {

            jumped = false;

            if (Mathf.Abs((int)myBody.velocity.x) > 0f)
            {
                //we are walking
                playerAnim.PlayAnimationWithName(TagManager.WALK_ANIMATION_NAME);
                
            }
            else
            {
                // we are idle
                playerAnim.PlayAnimationWithName(TagManager.IDLE_ANIMATION_NAME);
            }

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EndLevel"))
        {
            Debug.Log("end oflevel");
            GM.Level2();
        }
        else if (collision.collider.CompareTag("Danger")) // Ersetze "Enemy" durch den tatsächlichen Tag deiner Gegner
        {
            // Spiele den Todes-Sound ab, falls vorhanden
            if (deathSound) deathSound.Play();

            // Setze den Spieler zurück zu seiner Ausgangsposition
            if (healthPoints > 0)
            {
                transform.position = startPosition;
                myBody.velocity = Vector2.zero; // Setze die Geschwindigkeit zurück
                healthPoints--;
            }
            else
            {
                //End Game
                healthPoints = 3;
                GM.EndGame();
            }
        }
    }

} // class