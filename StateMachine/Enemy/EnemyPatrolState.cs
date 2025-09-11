using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人巡逻状态类，实现IState接口
/// 负责处理敌人的巡逻行为逻辑，包括路径点生成、移动控制和状态切换
/// </summary>
public class EnemyPatrolState : IState
{
    // 持有敌人组件的引用，用于访问敌人的属性和方法
    private Enemy enemy;
    // 巡逻移动方向向量
    private Vector2 dir;
    // 加速缓冲计时器（解决敌人从静止开始移动时初始速度为0的误判问题）
    private float accelerateBufferTimer = 0f;
    // 缓冲时间常量（可根据敌人加速性能调整，建议0.5~1秒）
    private const float AccelerateBufferTime = 0.5f;

    // 新增：跟踪移动失败次数
    private int moveFailCount = 0;
    // 新增：最大移动失败次数阈值
    private const int MaxMoveFailCount = 5;
    // 新增：速度判断阈值比例（当前速度低于预期的多少比例判定为异常）
    private const float SpeedThresholdRatio = 0.7f;
    // 新增：最小有效速度（避免微小速度被误判）
    private const float MinimumEffectiveSpeed = 0.1f;

    /// <summary>
    /// 构造函数，初始化巡逻状态
    /// </summary>
    /// <param name="enemy">敌人组件引用</param>
    public EnemyPatrolState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// 进入巡逻状态时调用
    /// 初始化巡逻路径并设置动画状态
    /// </summary>
    public void OnEnter()
    {
        // 生成巡逻路径点
        GeneratePatrolPoint();
        // 播放行走动画
        enemy.animator.Play("Walk");
        // 重置加速缓冲计时器，开始新的加速缓冲周期
        accelerateBufferTimer = 0f;
        // 重置移动失败计数器
        moveFailCount = 0;
    }

    /// <summary>
    /// 退出巡逻状态时调用
    /// 目前巡逻状态退出时无需特殊处理
    /// </summary>
    public void OnExit()
    {

    }

    /// <summary>
    /// 固定更新方法（物理帧更新）
    /// 处理敌人的移动逻辑
    /// </summary>
    public void OnFixedUpdate()
    {
        // 调用敌人的移动方法（物理相关的移动实现）
        enemy.Move();
    }

    /// <summary>
    /// 每帧更新方法
    /// 处理状态切换判断、路径点跟踪和碰撞检测
    /// </summary>
    public void OnUpdate()
    {
        // 受伤状态判断：如果敌人受伤，切换到受伤状态
        if (enemy.isHurt)
        {
            enemy.TransitionState(EnemyStateType.Hurt);
            return;
        }

        // 玩家检测：获取玩家位置，如果发现玩家则切换到追逐状态
        enemy.GetPlayerTransform();
        if (enemy.player != null)
        {
            enemy.TransitionState(EnemyStateType.Chase);
            return;
        }

        // 路径点逻辑处理
        // 如果路径点列表为空或没有路径点，重新生成巡逻路径
        if (enemy.pathPointlist == null || enemy.pathPointlist.Count <= 0)
        {
            GeneratePatrolPoint();
            accelerateBufferTimer = 0f; // 重新生成路径后，重置缓冲计时器
            moveFailCount = 0; // 重置失败计数
        }
        else
        {
            // 检查是否到达当前目标路径点（距离小于0.1视为到达）
            if (Vector2.Distance(enemy.transform.position, enemy.pathPointlist[enemy.currentIndex]) <= 0.1f)
            {
                // 到达当前路径点，索引移至下一个
                enemy.currentIndex++;
                accelerateBufferTimer = 0f; // 切换路径点后，重置缓冲计时器
                moveFailCount = 0; // 成功到达路径点，重置失败计数

                // 如果已到达所有路径点的终点，切换到 idle 状态
                if (enemy.currentIndex >= enemy.pathPointlist.Count)
                {
                    enemy.TransitionState(EnemyStateType.Idle);
                }
                else
                {
                    // 计算到下一个路径点的方向并设置移动输入
                    UpdateMovementDirection();
                }
            }
            else
            {
                // 未到达路径点：更新加速缓冲计时器
                accelerateBufferTimer += Time.deltaTime;

                // 关键逻辑：只有缓冲期结束后，才执行速度判断
                if (accelerateBufferTimer >= AccelerateBufferTime)
                {
                    // 缓冲期结束，正常判断是否卡住或碰撞
                    HandleMovementCheck();
                }
                // 缓冲期内：不做速度判断，让敌人完成加速过程
            }
        }
    }

    /// <summary>
    /// 更新移动方向
    /// </summary>
    private void UpdateMovementDirection()
    {
        dir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;
        enemy.MovementInput = dir;
    }

    /// <summary>
    /// 处理移动状态检查和碰撞应对
    /// </summary>
    private void HandleMovementCheck()
    {
        float currentSpeed = enemy.rb.velocity.magnitude;
        float expectedMinSpeed = enemy.currentSpeed * SpeedThresholdRatio;

        // 检查当前速度是否异常（低于预期且有一定速度值）
        if (currentSpeed < expectedMinSpeed && currentSpeed > MinimumEffectiveSpeed)
        {
            // 速度不足，可能遇到障碍物
            moveFailCount++;

            if (moveFailCount >= MaxMoveFailCount)
            {
                // 多次尝试失败，重新规划路径
                GeneratePatrolPoint();
                moveFailCount = 0; // 重置失败计数
                accelerateBufferTimer = 0f; // 重置缓冲计时器
            }
            else
            {
                // 尝试小幅调整方向
                AdjustDirectionAfterCollision();
            }
        }
        // 完全静止状态（卡住）
        else if (currentSpeed <= MinimumEffectiveSpeed)
        {
            moveFailCount++;

            if (moveFailCount >= MaxMoveFailCount)
            {
                // 多次卡住，重新生成巡逻点
                GeneratePatrolPoint();
                moveFailCount = 0;
                accelerateBufferTimer = 0f;
            }
            else
            {
                // 重新计算方向尝试移动
                UpdateMovementDirection();
            }
        }
        else
        {
            // 移动正常，重置失败计数
            moveFailCount = 0;
        }
    }

    /// <summary>
    /// 碰撞后调整方向
    /// </summary>
    private void AdjustDirectionAfterCollision()
    {
        // 尝试稍微调整方向以绕过障碍物
        Vector2 targetDir = (enemy.pathPointlist[enemy.currentIndex] - enemy.transform.position).normalized;

        // 随机小角度偏移（±15度）
        float angleOffset = Random.Range(-15f, 15f) * Mathf.Deg2Rad;
        float newX = targetDir.x * Mathf.Cos(angleOffset) - targetDir.y * Mathf.Sin(angleOffset);
        float newY = targetDir.x * Mathf.Sin(angleOffset) + targetDir.y * Mathf.Cos(angleOffset);

        dir = new Vector2(newX, newY).normalized;
        enemy.MovementInput = dir;
    }

    /// <summary>
    /// 生成巡逻目标点
    /// 从预设的巡逻点中随机选择一个与当前不同的点作为目标
    /// </summary>
    public void GeneratePatrolPoint()
    {
        // 循环直到随机选择一个与当前目标点不同的巡逻点
        int attempts = 0;
        while (true)
        {
            attempts++;
            // 从巡逻点数组中随机选择一个索引
            int i = Random.Range(0, enemy.patrolPoints.Length);
            // 确保不选择当前已有的目标点，或者尝试5次后强制选择
            if (enemy.targetPointIndex != i || attempts >= 5)
            {
                enemy.targetPointIndex = i;
                break;
            }
        }
        // 根据选择的巡逻点生成路径
        enemy.GeneratePath(enemy.patrolPoints[enemy.targetPointIndex].position);
    }
}
