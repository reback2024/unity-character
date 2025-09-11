using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///���˴���״̬
///</summary>

public class EnemyIdleState: IState
{
    private Enemy enemy;

    private float Timer = 0f;
    //���캯��
    public EnemyIdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        enemy.animator.Play("Idle");
        //enemy.rb.velocity = Vector2.zero;
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
            enemy.TransitionState(EnemyStateType.Hurt);
        }

        enemy.GetPlayerTransform();

        if(enemy.player != null)
        {
            if (enemy.distance > enemy.attackDistance)
            {
                enemy.TransitionState(EnemyStateType.Chase);
            }
            else
            {
                enemy.TransitionState(EnemyStateType.Attack);
            }
        }
        else//������Ϊ�գ��ȴ�һ��ʱ�����Ѳ��״̬
        {
            if (Timer <= enemy.IdleDuration) 
            {
                Timer += Time.deltaTime;
            }
            else
            {
                Timer = 0;
                enemy.TransitionState(EnemyStateType.Patrol);
            }
        }
    }
}
