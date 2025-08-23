using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///���˴���״̬
///</summary>

public class EnemyIdleState: IState
{
    private Enemy enemy;
    //���캯��
    public EnemyIdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        enemy.animator.Play("Idle");
        enemy.rb.velocity = Vector2.zero;
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
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemySateType.Hurt);
        }

        enemy.GetPlayerTransform();

        if(enemy.player != null)
        {
            if (enemy.distance > enemy.attackDistance)
            {
                enemy.TransitionState(EnemySateType.Chase);
            }
            else
            {
                enemy.TransitionState(EnemySateType.Attack);
            }
        }
    }
}
