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

        bool isWalking = Input.GetKey(KeyCode.LeftShift); //Walk Player

        bool isCrouch = Input.GetKey(KeyCode.LeftControl); //Crouch Player
        
        if (!isCrouch)
        {
            PlayerMovement(xAxis, yAxis, walking : isWalking, crouching : isCrouch);
        }
        PlayerAnimation(xAxis, yAxis,walking : isWalking, crouching : isCrouch);
    }

    private void PlayerMovement(float xAxis, bool yAxis, bool walking, bool crouching)
    {

        //Set Speed
        float speed = (walking) ? walkSpeed : runSpeed;

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

    private void PlayerAnimation(float xAxis, bool yAxis, bool walking, bool crouching)
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
        animator.SetBool("isWalk", walking);

        //Crouch Player Animation
        animator.SetBool("isCrouch", crouching);
        playerCollider.size = (crouching) ? boxCrouchSize : boxSize;
        playerCollider.offset = (crouching) ? boxCrouchOffset : boxOffset;

        //Jump Player Animation
        if (yAxis)
        {
            animator.SetTrigger("isJump");
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("EnvTrigger"))
        {
            Animator anim = other.gameObject.GetComponent<Animator>();
            anim.Play("Fade Out");
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("EnvTrigger"))
        {
            Animator anim = other.gameObject.GetComponent<Animator>();
            anim.Play("Idle");
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("EnvTrigger"))
        {
            Animator anim = other.gameObject.GetComponent<Animator>();
            anim.Play("Fade In");
        }
    }
}
