using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private float jumpForce;
    
    [SerializeField]
    private float walkSpeed;
    
    [SerializeField]
    private float runSpeed;
    
    [SerializeField]
    private Vector2 boxCrouchOffset;
    
    [SerializeField]
    private Vector2 boxCrouchSize;

    //private
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D rb2d;
    private Vector2 initialPos;
    private Vector2 boxSize, boxOffset;
    private bool isGround;
    

    private void Start()
    {
        initialPos = transform.position;
        playerCollider = GetComponent<CapsuleCollider2D>();
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
        if(yAxis && isGround)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            animator.SetTrigger("isJump");
            isGround = false;
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

    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            transform.position = initialPos;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = false;
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
