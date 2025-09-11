using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///敌人受伤状态
///</summary>
public class EnemyHurtState : IState
{
    public Enemy enemy;

    private Vector2 dirction;
    private float Timer;//计时器
    public EnemyHurtState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        enemy.animator.Play("Hurt");
    }

    public void OnExit()
    {
        enemy.isHurt = false;
    }

    public void OnFixedUpdate()
    {
        //施加击退效果
        if (Timer <= enemy.knokbackForecDuration) 
        {
            enemy.rb.AddForce(dirction * enemy.knokbackForce, ForceMode2D.Impulse);
            Timer += Time.fixedDeltaTime;
        }
        else
        {
            Timer = 0;
            enemy.isHurt = false;
            //切换到待机状态
            enemy.TransitionState(EnemyStateType.Idle);
        }
    }

    public void OnUpdate()
    {
        //判断是否可以被击退 
        if(enemy.isKnokback)
        {
            if (enemy.player != null)
            {
                dirction = (enemy.transform.position - enemy.player.transform.position).normalized;
            }
            else
            {
                //如果玩家在追击范围外击打
                Transform player = GameObject.FindWithTag("Player").transform;
                dirction = (enemy.transform.position - player.position).normalized;
            }
        }
    }
}
