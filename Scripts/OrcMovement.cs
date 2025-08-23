using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    DetectionZone detectionZone;
    Animator animator;

    public float speed;
    public float knockbackForce;
    public int attactPower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        detectionZone = GetComponent<DetectionZone>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable=collider.GetComponent<IDamageable>();
        if (damageable != null && collider.tag == "Player")
        {
            Vector2 dir = collider.transform.position - transform.position;
            Vector2 force = dir.normalized * knockbackForce;

            damageable.OnHit(attactPower, force);
        }
    }

    public void OnWalk()
    {
        animator.SetBool("isWalking", true);
    }

    public void OnWalkStop()
    {
        animator.SetBool("isWalking",false);
    }

    void OnDamage()
    {
        animator.SetTrigger("isDamage");
    }

    void OnDie()
    {
        animator.SetTrigger("isDead");
    }
    private void FixedUpdate()
    {
        if (detectionZone.detectedObjs != null)
        {
            Vector2 dir = (detectionZone.detectedObjs.transform.position - transform.position);
            if (dir.magnitude <= detectionZone.viewRadius)
            {
                rb.AddForce(dir.normalized * speed);
                if (dir.x > 0) 
                {
                    spriteRenderer.flipX = false;
                }
                if (dir.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                OnWalk();
            }
            else
            {
                OnWalkStop();
            }
        }

    }

}
