using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ѳ��״̬�࣬ʵ��IState�ӿ�
/// ��������˵�Ѳ����Ϊ�߼�������·�������ɡ��ƶ����ƺ�״̬�л�
/// </summary>
public class EnemyPatrolState : IState
{
    // ���е�����������ã����ڷ��ʵ��˵����Ժͷ���
    private Enemy enemy;
    // Ѳ���ƶ���������
    private Vector2 dir;
    // ���ٻ����ʱ����������˴Ӿ�ֹ��ʼ�ƶ�ʱ��ʼ�ٶ�Ϊ0���������⣩
    private float accelerateBufferTimer = 0f;
    // ����ʱ�䳣�����ɸ��ݵ��˼������ܵ���������0.5~1�룩
    private const float AccelerateBufferTime = 0.5f;

    /// <summary>
    /// ���캯������ʼ��Ѳ��״̬
    /// </summary>
    /// <param name="enemy">�����������</param>
    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// ����Ѳ��״̬ʱ����
    /// ��ʼ��Ѳ��·�������ö���״̬
    /// </summary>
    public void OnEnter()
    {
        // ����Ѳ��·����
        GeneratePatrolPoint();
        // �������߶���
        enemy.animator.Play("Walk");
        // ���ü��ٻ����ʱ������ʼ�µļ��ٻ�������
        accelerateBufferTimer = 0f;
    }

    /// <summary>
    /// �˳�Ѳ��״̬ʱ����
    /// ĿǰѲ��״̬�˳�ʱ�������⴦��
    /// </summary>
    public void OnExit()
    {

    }

    /// <summary>
    /// �̶����·���������֡���£�
    /// ������˵��ƶ��߼�
    /// </summary>
    public void OnFixedUpdate()
    {
        // ���õ��˵��ƶ�������������ص��ƶ�ʵ�֣�
        enemy.Move();
    }

    /// <summary>
    /// ÿ֡���·���
    /// ����״̬�л��жϡ�·������ٺ���ײ���
    /// </summary>
    public void OnUpdate()
    {
        // ����״̬�жϣ�����������ˣ��л�������״̬
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemyStateType.Hurt);
            return;
        }

        // ��Ҽ�⣺��ȡ���λ�ã��������������л���׷��״̬
        enemy.GetPlayerTransform();
        if (enemy.player != null)
        {
            enemy.TransitionState(EnemyStateType.Chase);
            return;
        }

        // ·�����߼�����
        // ���·�����б�Ϊ�ջ�û��·���㣬��������Ѳ��·��
        if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0)
        {
            GeneratePatrolPoint();
            accelerateBufferTimer = 0f; // ��������·�������û����ʱ��
        }
        else
        {
            // ����Ƿ񵽴ﵱǰĿ��·���㣨����С��0.1��Ϊ���
            if (Vector2.Distance(enemy.transform.position, enemy.pathPointlist[enemy.currentIndex]) <= 0.1f)
            {
                // ���ﵱǰ·���㣬����������һ��
                enemy.currentIndex++;
                accelerateBufferTimer = 0f; // �л�·��������û����ʱ�����������¼��٣�

                // ����ѵ�������·������յ㣬�л��� idle ״̬
                if (enemy.currentIndex >= enemy.pathPointlist.Count)
                {
                    enemy.TransitionState(EnemyStateType.Idle);
                }
                else
                {
                    // ���㵽��һ��·����ķ��������ƶ�����
                    dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                    enemy.MovementInput = dir;
                }
            }
            else
            {
                // δ����·���㣺���¼��ٻ����ʱ��
                accelerateBufferTimer += Time.deltaTime;

                // �ؼ��߼���ֻ�л����ڽ����󣬲�ִ���ٶ��ж�
                // ������˸�����ʱ����ٹ����еĵ��ٶȱ�����Ϊ��ײ��ס
                if (accelerateBufferTimer >= AccelerateBufferTime)
                {
                    // �����ڽ����������ж��Ƿ�ס����ײ
                    // ��鵱ǰ�ٶ��Ƿ����Ӧ���ٶȣ��һ���δ�����·����
                    if (enemy.rb.velocity.magnitude < enemy.currentSpeed && enemy.currentIndex < enemy.pathPointlist.Count)
                    {
                        if (enemy.rb.velocity.magnitude == 0)
                        {
                            // �ٶ�Ϊ0���ж�Ϊ��ס�����¼��㷽��
                            dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
                            enemy.MovementInput = dir;
                        }
                        else
                        {
                            // �ٶȲ����ҷǾ�ֹ״̬���ж�Ϊ��ײ�ϰ���л��� idle ״̬
                            enemy.TransitionState(EnemyStateType.Idle);
                        }
                    }
                }
                // �������ڣ������ٶ��жϣ��õ�����ɼ��ٹ���
            }
        }
    }

    /// <summary>
    /// ����Ѳ��Ŀ���
    /// ��Ԥ���Ѳ�ߵ������ѡ��һ���뵱ǰ��ͬ�ĵ���ΪĿ��
    /// </summary>
    public void GeneratePatrolPoint()
    {
        // ѭ��ֱ�����ѡ��һ���뵱ǰĿ��㲻ͬ��Ѳ�ߵ�
        while (true)
        {
            // ��Ѳ�ߵ����������ѡ��һ������
            int i = Random.Range(0, enemy.patrolPoints.Length);
            // ȷ����ѡ��ǰ���е�Ŀ���
            if (enemy.targetPointIndex != i)
            {
                enemy.targetPointIndex = i;
                break;
            }
        }
        // ����ѡ���Ѳ�ߵ�����·��
        enemy.GeneratePath(enemy.patrolPoints[enemy.targetPointIndex].position);
    }
}