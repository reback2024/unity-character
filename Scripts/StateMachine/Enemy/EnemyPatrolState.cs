using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///����Ѳ��״̬
///</summary>
public class EnemyPatrolState : IState
{
    private Enemy enemy;
    private Vector2 dir;
    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        GeneratePatrolPoint();//�������Ѳ�ߵ�
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
        //�Ƿ�����
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemyStateType.Hurt);
        }

        //�������
        enemy.GetPlayerTransform();
        if (enemy.player != null)
        {
            enemy.TransitionState(EnemyStateType.Chase);
        }

        //·�����Ƿ�Ϊ��
        if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0) 
        {
            GeneratePatrolPoint();
        }
        else
        {
            if (Vector2.Distance(enemy.transform.position, enemy.pathPointlist[enemy.currentIndex]) <= 0.1f)//����
            {
                enemy.currentIndex++;

                if (enemy.currentIndex >= enemy.pathPointlist.Count)
                {
                    enemy.TransitionState(EnemyStateType.Idle);
                }
                else
                {
                    dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                    enemy.MovementInput = dir;
                }
            }
            else//δ����
            {
                //�����������ٶ� < ���Ѳ���ٶȣ��һ�û�ߵ����һ��·����
                if (enemy.rb.velocity.magnitude < enemy.currentSpeed && enemy.currentIndex < enemy.pathPointlist.Count)
                {
                    // �����ٶ�Ϊ0��û��������ײǽ/��ס�ˣ�
                    if (enemy.rb.velocity.magnitude == 0)
                    {
                        dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                        enemy.MovementInput = dir;
                    }
                    else
                    {
                        //�����ٶȲ�Ϊ0��������û�ﵽ����ٶȣ�����ײ����������)
                        enemy.TransitionState(EnemyStateType.Idle);
                    }
                }
            }
        }
    }

    public void GeneratePatrolPoint()
    {
        while(true)
        {
            int i = Random.Range(0, enemy.patrolPoints.Length);
            //���ѡ��һ��Ѳ�ߵ� 

            if (enemy.targetPointIndex != i) 
            {
                enemy.targetPointIndex = i;
                break;
            }
        }
        //�ҵ�һ��Ѳ�ߵ�

        //��Ѳ�ߵ������·���㺯��
        enemy.GeneratePath(enemy.patrolPoints[enemy.targetPointIndex].position);
    }
}
