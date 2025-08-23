using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///敌人攻击状态
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
            enemy.AttackColdown();//冷却时间
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
        //判断敌人是否受伤
        if(enemy.isHurt)
        {
            enemy.TransitionState(EnemySateType.Hurt);
        }

        //禁止敌人移动
        enemy.rb.velocity = Vector2.zero;
        //人物翻转
        float x = enemy.player.position.x - enemy.transform.position.x;
        if (x > 0)
        {
            enemy.sr.flipX = true;
        }
        else
        {
            enemy.sr.flipX = false;
        }

        //获取敌人角色当前播放的动画状态的信息
        info = enemy.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1f)//播放完后切换动画
        {
            enemy.TransitionState(EnemySateType.Idle);
        }
    }
}
