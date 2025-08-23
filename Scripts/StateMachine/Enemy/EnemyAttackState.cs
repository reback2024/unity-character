using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///���˹���״̬
///</summary>
public class EnemyAttackState : IState
{
    public Enemy enemy;

    public AnimatorStateInfo info;
    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        if(enemy.isAttack)
        {
            enemy.animator.Play("Attack");
            enemy.isAttack = false;
            enemy.AttackColdown();//��ȴʱ��
        }
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        //�жϵ����Ƿ�����
        if(enemy.isHurt)
        {
            enemy.TransitionState(EnemySateType.Hurt);
        }

        //��ֹ�����ƶ�
        enemy.rb.velocity = Vector2.zero;
        //���﷭ת
        float x = enemy.player.position.x - enemy.transform.position.x;
        if (x > 0)
        {
            enemy.sr.flipX = true;
        }
        else
        {
            enemy.sr.flipX = false;
        }

        //��ȡ���˽�ɫ��ǰ���ŵĶ���״̬����Ϣ
        info = enemy.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1f)//��������л�����
        {
            enemy.TransitionState(EnemySateType.Idle);
        }
    }
}
