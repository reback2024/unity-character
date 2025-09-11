using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///��������״̬
///</summary>
public class EnemyHurtState : IState
{
    public Enemy enemy;

    private Vector2 dirction;
    private float Timer;//��ʱ��
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
        //ʩ�ӻ���Ч��
        if (Timer <= enemy.knokbackForecDuration) 
        {
            enemy.rb.AddForce(dirction * enemy.knokbackForce, ForceMode2D.Impulse);
            Timer += Time.fixedDeltaTime;
        }
        else
        {
            Timer = 0;
            enemy.isHurt = false;
            //�л�������״̬
            enemy.TransitionState(EnemyStateType.Idle);
        }
    }

    public void OnUpdate()
    {
        //�ж��Ƿ���Ա����� 
        if(enemy.isKnokback)
        {
            if (enemy.player != null)
            {
                dirction = (enemy.transform.position - enemy.player.transform.position).normalized;
            }
            else
            {
                //��������׷����Χ�����
                Transform player = GameObject.FindWithTag("Player").transform;
                dirction = (enemy.transform.position - player.position).normalized;
            }
        }
    }
}
