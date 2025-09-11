using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///敌人待机状态
///</summary>

public class EnemyIdleState: IState
{
    private Enemy enemy;

    private float Timer = 0f;
    //构造函数
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
        //判断敌人是否受伤
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
        else//如果玩家为空，等待一段时间进入巡逻状态
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
