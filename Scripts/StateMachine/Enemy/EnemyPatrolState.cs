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

    // �����������ƶ�ʧ�ܴ���
    private int moveFailCount = 0;
    // ����������ƶ�ʧ�ܴ�����ֵ
    private const int MaxMoveFailCount = 5;
    // �������ٶ��ж���ֵ��������ǰ�ٶȵ���Ԥ�ڵĶ��ٱ����ж�Ϊ�쳣��
    private const float SpeedThresholdRatio = 0.7f;
    // ��������С��Ч�ٶȣ�����΢С�ٶȱ����У�
    private const float MinimumEffectiveSpeed = 0.1f;

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
        // �����ƶ�ʧ�ܼ�����
        moveFailCount = 0;
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
            moveFailCount = 0; // ����ʧ�ܼ���
        }
        else
        {
            // ����Ƿ񵽴ﵱǰĿ��·���㣨����С��0.1��Ϊ���
            if (Vector2.Distance(enemy.transform.position, enemy.pathPointlist[enemy.currentIndex]) <= 0.1f)
            {
                // ���ﵱǰ·���㣬����������һ��
                enemy.currentIndex++;
                accelerateBufferTimer = 0f; // �л�·��������û����ʱ��
                moveFailCount = 0; // �ɹ�����·���㣬����ʧ�ܼ���

                // ����ѵ�������·������յ㣬�л��� idle ״̬
                if (enemy.currentIndex >= enemy.pathPointlist.Count)
                {
                    enemy.TransitionState(EnemyStateType.Idle);
                }
                else
                {
                    // ���㵽��һ��·����ķ��������ƶ�����
                    UpdateMovementDirection();
                }
            }
            else
            {
                // δ����·���㣺���¼��ٻ����ʱ��
                accelerateBufferTimer += Time.deltaTime;

                // �ؼ��߼���ֻ�л����ڽ����󣬲�ִ���ٶ��ж�
                if (accelerateBufferTimer >= AccelerateBufferTime)
                {
                    // �����ڽ����������ж��Ƿ�ס����ײ
                    HandleMovementCheck();
                }
                // �������ڣ������ٶ��жϣ��õ�����ɼ��ٹ���
            }
        }
    }

    /// <summary>
    /// �����ƶ�����
    /// </summary>
    private void UpdateMovementDirection()
    {
        dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
        enemy.MovementInput = dir;
    }

    /// <summary>
    /// �����ƶ�״̬������ײӦ��
    /// </summary>
    private void HandleMovementCheck()
    {
        float currentSpeed = enemy.rb.velocity.magnitude;
        float expectedMinSpeed = enemy.currentSpeed * SpeedThresholdRatio;

        // ��鵱ǰ�ٶ��Ƿ��쳣������Ԥ������һ���ٶ�ֵ��
        if (currentSpeed < expectedMinSpeed && currentSpeed > MinimumEffectiveSpeed)
        {
            // �ٶȲ��㣬���������ϰ���
            moveFailCount++;

            if (moveFailCount >= MaxMoveFailCount)
            {
                // ��γ���ʧ�ܣ����¹滮·��
                GeneratePatrolPoint();
                moveFailCount = 0; // ����ʧ�ܼ���
                accelerateBufferTimer = 0f; // ���û����ʱ��
            }
            else
            {
                // ����С����������
                AdjustDirectionAfterCollision();
            }
        }
        // ��ȫ��ֹ״̬����ס��
        else if (currentSpeed <= MinimumEffectiveSpeed)
        {
            moveFailCount++;

            if (moveFailCount >= MaxMoveFailCount)
            {
                // ��ο�ס����������Ѳ�ߵ�
                GeneratePatrolPoint();
                moveFailCount = 0;
                accelerateBufferTimer = 0f;
            }
            else
            {
                // ���¼��㷽�����ƶ�
                UpdateMovementDirection();
            }
        }
        else
        {
            // �ƶ�����������ʧ�ܼ���
            moveFailCount = 0;
        }
    }

    /// <summary>
    /// ��ײ���������
    /// </summary>
    private void AdjustDirectionAfterCollision()
    {
        // ������΢�����������ƹ��ϰ���
        Vector2 targetDir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;

        // ���С�Ƕ�ƫ�ƣ���15�ȣ�
        float angleOffset = Random.Range(-15f, 15f) * Mathf.Deg2Rad;
        float newX = targetDir.x * Mathf.Cos(angleOffset) - targetDir.y * Mathf.Sin(angleOffset);
        float newY = targetDir.x * Mathf.Sin(angleOffset) + targetDir.y * Mathf.Cos(angleOffset);

        dir = new Vector2(newX, newY).normalized;
        enemy.MovementInput = dir;
    }

    /// <summary>
    /// ����Ѳ��Ŀ���
    /// ��Ԥ���Ѳ�ߵ������ѡ��һ���뵱ǰ��ͬ�ĵ���ΪĿ��
    /// </summary>
    public void GeneratePatrolPoint()
    {
        // ѭ��ֱ�����ѡ��һ���뵱ǰĿ��㲻ͬ��Ѳ�ߵ�
        int attempts = 0;
        while (true)
        {
            attempts++;
            // ��Ѳ�ߵ����������ѡ��һ������
            int i = Random.Range(0, enemy.patrolPoints.Length);
            // ȷ����ѡ��ǰ���е�Ŀ��㣬���߳���5�κ�ǿ��ѡ��
            if (enemy.targetPointIndex != i || attempts >= 5)
            {
                enemy.targetPointIndex = i;
                break;
            }
        }
        // ����ѡ���Ѳ�ߵ�����·��
        enemy.GeneratePath(enemy.patrolPoints[enemy.targetPointIndex].position);
    }
}
