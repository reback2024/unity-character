using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///µÐÈË×·»÷×´Ì¬
///</summary>
public class EnemyChaseState : IState
{
    private Enemy enemy;
    public EnemyChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        enemy.animator.Play("Walk");
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        Move();
    }

    public void OnUpdate()
    {
        //ÅÐ¶ÏµÐÈËÊÇ·ñÊÜÉË
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemySateType.Hurt);
        }

        enemy.GetPlayerTransform();

        enemy.Autopath();

        if(enemy.player != null)
        {
            if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0)
                return;

            //ÊÇ·ñÔÚ¹¥»÷·¶Î§ÄÚ
            if(enemy.distance<=enemy.attackDistance)
            {
                enemy.TransitionState(EnemySateType.Attack);
            }
            else
            {
                //×·ÖðÍæ¼Ò
                Vector2 direction = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                enemy.MovementInput = direction;
            }
        }
        else
        {
            //Í£Ö¹×·»÷£¬·µ»Ø´ý»ú×´Ì¬
            enemy.TransitionState(EnemySateType.Idle);
        }
    }
    //ÒÆ¶¯º¯Êý
    void Move()
    {
        if (enemy.MovementInput.magnitude > 0.1f && enemy.currentSpeed >= 0)
        {
            enemy.rb.velocity = enemy.MovementInput * enemy.currentSpeed;
            //Íæ¼Ò×óÓÒ·­×ª
            if (enemy.MovementInput.x < 0)
            {
                enemy.sr.flipX = false;
            }
            if (enemy.MovementInput.x > 0)
            {
                enemy.sr.flipX = true;
            }
        }
        else
        {
            enemy.rb.velocity = Vector2.zero;
        }
    }
}
