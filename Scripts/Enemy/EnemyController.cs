using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //[Header("����")]
    //[SerializeField] private float currentSpeed = 0;
    //public Vector2 MovementInput { get; set; }

    //[Header("����")]
    //[SerializeField] private bool isAttack = true;
    //[SerializeField] private float attackCoolDuration = 1;

    //[Header("����")]
    //[SerializeField] private bool isKnokback = true;
    //[SerializeField] private float KnokbackForce = 10f;
    //[SerializeField] private float knokbackForceDuration = 0.1f;

    //private Rigidbody2D rb;
    //private Collider2D enemyCollider;
    //private SpriteRenderer sr;
    //private Animator anim;

    //private bool isHurt;
    //private bool isDead;
    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    sr = GetComponent<SpriteRenderer>();
    //    enemyCollider = GetComponent<Collider2D>();
    //    anim = GetComponent<Animator>();
    //}

    //private void FixedUpdate()
    //{
    //    if (!isDead && !isHurt)
    //        move();
    //    SetAnimation();
    //}

    //void move()
    //{
    //    if (MovementInput.magnitude > 0.1f && currentSpeed >= 0)
    //    {
    //        rb.velocity = MovementInput * currentSpeed;
    //        //������ҷ�ת
    //        if (MovementInput.x < 0)
    //        {
    //            sr.flipX = false;
    //        }
    //        if (MovementInput.x > 0)
    //        {
    //            sr.flipX = true;
    //        }
    //    }
    //    else
    //    {
    //        rb.velocity = Vector2.zero;
    //    }
    //}

    //public void Attack()
    //{
    //    if (isAttack)
    //    {
    //        isAttack = false;
    //        StartCoroutine(nameof(AttackCoroutine));
    //    }
    //}

    //IEnumerator AttackCoroutine()
    //{
    //    anim.SetTrigger("Attack");

    //    yield return new WaitForSeconds(attackCoolDuration);
    //    isAttack = true;
    //}

    //public void EmenyHurt()
    //{
    //    isHurt = true;
    //    anim.SetTrigger("Hurt");
    //}
    //public void Konckback(Vector3 pos)
    //{
    //    //ʩ�ӻ���Ч��
    //    if (!isKnokback || isDead) return;

    //    StartCoroutine(KnockbackCoroutine(pos));

    //}

    //IEnumerator KnockbackCoroutine(Vector3 pos)
    //{
    //    var dir = (transform.position - pos).normalized;
    //    rb.AddForce(dir * KnokbackForce, ForceMode2D.Impulse);
    //    yield return new WaitForSeconds(knokbackForceDuration);
    //    isHurt = false;
    //}

    //public void EmenyDead()
    //{
    //    rb.velocity = Vector2.zero;
    //    isDead = true;
    //    enemyCollider.enabled = false;
    //}

    //void SetAnimation()
    //{
    //    anim.SetBool("isWalk", MovementInput.magnitude > 0);
    //    anim.SetBool("isDead", isDead);
    //}
    //public void DestoryEnemy()
    //{
    //    Destroy(this.gameObject);
    //}

}
