using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///����׷��״̬
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
       enemy.Move();
    }

    public void OnUpdate()
    {
        //�жϵ����Ƿ�����
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemyStateType.Hurt);
        }

        enemy.GetPlayerTransform();

        enemy.Autopath();

        if(enemy.player != null)
        {
            if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0)
                return;

            //�Ƿ��ڹ�����Χ��
            if(enemy.distance<=enemy.attackDistance)
            {
                enemy.TransitionState(EnemyStateType.Attack);
            }
            else
            {
                //׷�����
                Vector2 direction = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                enemy.MovementInput = direction;
            }
        }
        else
        {
            //ֹͣ׷�������ش���״̬
            enemy.TransitionState(EnemyStateType.Idle);
        }
    }
    

}
