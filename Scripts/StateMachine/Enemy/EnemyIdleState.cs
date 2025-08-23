using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///敌人待机状态
///</summary>

public class EnemyIdleState: IState
{
    private Enemy enemy;
    //构造函数
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
        //判断敌人是否受伤
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
