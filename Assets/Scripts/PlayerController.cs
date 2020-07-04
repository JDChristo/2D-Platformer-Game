using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //public
    public Animator animator;
    public float jump, speed;
    public Vector2 boxCrouchOffset, boxCrouchSize;

    //private
    private BoxCollider2D collider2D;
    private bool isCrouch = false;
    private Vector2 boxSize, boxOffset;
    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        boxOffset = collider2D.offset;
        boxSize = collider2D.size;
    }

    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        if (!isCrouch)
        {
            PlayerMovement(xAxis);
        }
        PlayerAnimation(xAxis);
    }

    private void PlayerMovement(float xAxis)
    {
        // Move player
        Vector3 position = transform.localPosition;
        position.x += xAxis * speed * Time.deltaTime;
        transform.position = position;
    }

    private void PlayerAnimation(float xAxis)
    {
        animator.SetFloat("Speed", Mathf.Abs(xAxis));

        //Flip Player
        Vector3 scale = transform.localScale;
        if ((xAxis < 0 && scale.x > 0) || (xAxis > 0 && scale.x < 0))
        {
            scale.x *= -1;
        }
        transform.localScale = scale;

        //Crouch Player
        isCrouch = Input.GetKey(KeyCode.LeftControl);
        animator.SetBool("isCrouch", isCrouch);
        collider2D.size = (isCrouch) ? boxCrouchSize : boxSize;
        collider2D.offset = (isCrouch) ? boxCrouchOffset : boxOffset;

    }
}
