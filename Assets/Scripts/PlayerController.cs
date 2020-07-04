using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //public
    public Animator animator;
    public float jump, speed;

    //private
    private bool isCrouch = false;


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

    }
}
