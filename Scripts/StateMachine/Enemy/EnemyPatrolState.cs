using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///敌人巡逻状态
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
        GeneratePatrolPoint();//生成随机巡逻点
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
        //是否受伤
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemyStateType.Hurt);
        }

        //发现玩家
        enemy.GetPlayerTransform();
        if (enemy.player != null)
        {
            enemy.TransitionState(EnemyStateType.Chase);
        }

        //路径点是否为空
        if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0) 
        {
            GeneratePatrolPoint();
        }
        else
        {
            if (Vector2.Distance(enemy.transform.position, enemy.pathPointlist[enemy.currentIndex]) <= 0.1f)//到达
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
            else//未到达
            {
                //条件：敌人速度 < 最大巡逻速度，且还没走到最后一个路径点
                if (enemy.rb.velocity.magnitude < enemy.currentSpeed && enemy.currentIndex < enemy.pathPointlist.Count)
                {
                    // 敌人速度为0（没动，可能撞墙/卡住了）
                    if (enemy.rb.velocity.magnitude == 0)
                    {
                        dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                        enemy.MovementInput = dir;
                    }
                    else
                    {
                        //敌人速度不为0，但还是没达到最大速度（可能撞其他敌人了)
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
            //随机选择一个巡逻点 

            if (enemy.targetPointIndex != i) 
            {
                enemy.targetPointIndex = i;
                break;
            }
        }
        //找到一个巡逻点

        //把巡逻点给生成路径点函数
        enemy.GeneratePath(enemy.patrolPoints[enemy.targetPointIndex].position);
    }
}
