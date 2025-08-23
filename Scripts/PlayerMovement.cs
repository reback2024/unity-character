using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;
    public float movespeed;
    Animator ani;
    SpriteRenderer spr;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput == Vector2.zero)
            ani.SetBool("isWalking", false);
        else
        {
            ani.SetBool("isWalking", true);
            if (moveInput.x > 0)
            {
                spr.flipX = false;
                gameObject.BroadcastMessage("IsFacingRight", true);

            }
            if (moveInput.x < 0)
            {
                spr.flipX = true;
                gameObject.BroadcastMessage("IsFacingRight", false);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveInput * movespeed);
    }
    void OnFire()
    {
        ani.SetTrigger("swordAttack");
    }
    void OnDamage()
    {
        ani.SetTrigger("isDamage");
    }
    void OnDie()
    {
        ani.SetTrigger("isDead");
    }

    #region Animation Method
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
    #endregion
}
