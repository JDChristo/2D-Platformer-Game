using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //public
    public Animator animator;
    public float jumpForce, walkSpeed, runSpeed;
    public Vector2 boxCrouchOffset, boxCrouchSize;

    //private
    private BoxCollider2D playerCollider;
    private Rigidbody2D rb2d;
    private bool isCrouch = false;
    private bool isWalking = false;
    private Vector2 boxSize, boxOffset;
    private void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        boxOffset = playerCollider.offset;
        boxSize = playerCollider.size;
    }

    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal"); //Horizontal Movement

        bool yAxis = Input.GetKeyDown(KeyCode.Space); //Vertical Movement

        isWalking = Input.GetKey(KeyCode.LeftShift); //Walk Player

        isCrouch = Input.GetKey(KeyCode.LeftControl); //Crouch Player
        
        if (!isCrouch)
        {
            PlayerMovement(xAxis, yAxis);
        }
        PlayerAnimation(xAxis, yAxis);
    }

    private void PlayerMovement(float xAxis, bool yAxis)
    {

        //Set Speed
        float speed = (isWalking) ? walkSpeed : runSpeed;

        // Move player
        Vector3 position = transform.localPosition;
        position.x += xAxis * speed * Time.deltaTime;
        transform.position = position;

        //Jump Player
        if(yAxis)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }

    private void PlayerAnimation(float xAxis, bool yAxis)
    {
        // Move player animation
        animator.SetFloat("Speed", Mathf.Abs(xAxis));

        //Flip Player
        Vector3 scale = transform.localScale;
        if ((xAxis < 0 && scale.x > 0) || (xAxis > 0 && scale.x < 0))
        {
            scale.x *= -1;
        }
        transform.localScale = scale;

        //Walk Player Animation
        animator.SetBool("isWalk", isWalking);

        //Crouch Player Animation
        animator.SetBool("isCrouch", isCrouch);
        playerCollider.size = (isCrouch) ? boxCrouchSize : boxSize;
        playerCollider.offset = (isCrouch) ? boxCrouchOffset : boxOffset;

        //Jump Player Animation
        if (yAxis)
        {
            animator.SetTrigger("isJump");
        }

    }
}
