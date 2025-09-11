using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public InputActions action;
    //private Vector2 input;
    //public float speed;
    //private Rigidbody2D rb;
    //private SpriteRenderer spr;
    //private Animator animator;

    //[Header("近战攻击")]
    //public bool isMeleeAttack;

    //[Header("闪避")]
    //public bool isDodging = false;
    //public float dodgeForce;
    //public float dodgeTime = 0f;
    //public float dodgeDuration = 0f;
    //public float dodgeCooldown = 1f;//闪避冷却时间
    //private bool isDodegOnCooldown = false;//闪避是否在冷却时间中

    //public bool isDead;
    //private void Awake()
    //{
    //    action = new InputActions();
    //    rb = GetComponent<Rigidbody2D>();
    //    spr = GetComponent<SpriteRenderer>();
    //    animator = GetComponent<Animator>();

    //    action.Gameplay.MeleeAttack.started += MelleAttack;
    //    action.Gameplay.Dodge.started += isDodge;
    //}

    //private void OnEnable()
    //{
    //    action.Enable();
    //}
    //private void OnDisable()
    //{
    //    action.Disable();
    //}
    //private void Update()
    //{
    //    input = action.Gameplay.Move.ReadValue<Vector2>();
    //    SetAni();
    //}
    //private void FixedUpdate()
    //{
    //    Move();

    //    Dodge();
    //}

    //private void Move()
    //{
    //    rb.velocity = input * speed;
    //    if (input.x > 0) spr.flipX = false;
    //    if (input.x < 0) spr.flipX = true;
    //}
    //void Dodge()
    //{
    //    if (isDodegOnCooldown)
    //    {
    //        dodgeTime += Time.fixedDeltaTime;
    //        if (dodgeTime > dodgeCooldown)
    //        {
    //            isDodegOnCooldown = false;
    //            dodgeTime = 0;
    //            Debug.Log("闪避冷却结束");
    //        }
    //    }
    //    if (isDodging)
    //    {
    //        if (!isDodegOnCooldown)
    //        {

    //            if (dodgeTime <= dodgeDuration)
    //            {
    //                rb.AddForce(input * dodgeForce, ForceMode2D.Impulse);
    //                dodgeTime += Time.fixedDeltaTime;
    //            }
    //            else
    //            {
    //                isDodging = false;
    //                isDodegOnCooldown = true;
    //                Debug.Log("闪避开始冷却,时间为1s");
    //                dodgeTime = 0;
    //            }
    //        }
    //    }
    //}
    //private void MelleAttack(InputAction.CallbackContext context)
    //{
    //    if (!isDodging)
    //    {
    //        animator.SetTrigger("meleeAttack");
    //        isMeleeAttack = true;
    //    }

    //}

    //public void playerHurt()
    //{
    //    animator.SetTrigger("Hurt");
    //}

    //public void PlayerDead()
    //{
    //    isDead = true;
    //    SwitchActionMap(action.UI);//切换为ui表
    //}

    //private void isDodge(InputAction.CallbackContext context)
    //{
    //    if (!isDodging && !isDodegOnCooldown) isDodging = true;
    //}

    //void SetAni()
    //{
    //    animator.SetFloat("speed", rb.velocity.magnitude);
    //    animator.SetBool("isMeleeAttack", isMeleeAttack);
    //    animator.SetBool("isDodge", isDodging);
    //    animator.SetBool("isDead", isDead);
    //}

    
}
