using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("½üÕ½¹¥»÷")]
    public float meleeAttackDamage;
    public Vector2 attackSize = new Vector2(1f, 1f);//¹¥»÷·¶Î§
    public float offestX = 1f;//xÖáÆ«ÒÆÁ¿
    public float offestY = 1f;//yÖáÆ«ÒÆÁ¿
    public LayerMask enemyLayer;

    private Vector2 AttackAreaPos;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void MeleeAttackAnimEvent(float isAttack)
    {
        AttackAreaPos = transform.position;

        offestX = spriteRenderer.flipX ? -Mathf.Abs(offestX) : Mathf.Abs(offestX);

        AttackAreaPos.x += offestX;
        AttackAreaPos.y += offestY;

        

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(AttackAreaPos, attackSize, 0f, enemyLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Character>().TakeDamage(meleeAttackDamage * isAttack);
            //hitCollider.GetComponent<EnemyController>().Konckback(transform.position);
        }
    }

    //»æÍ¼ÓÃÓÚ²âÊÔ
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(AttackAreaPos,attackSize);
    }
}
