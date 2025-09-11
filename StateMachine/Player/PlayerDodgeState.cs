using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家闪避状态
/// </summary>
public class PlayerDodgeState : IState
{
    Player player;
    private float dodgeTimer = 0f;

    // 构造函数
    public PlayerDodgeState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        // 进入翻滚状态时开启无敌
        player.invulnerable = true;
        // 播放翻滚动画
        player.ani.SetBool("isDodge", true);
        // 重置计时器
        dodgeTimer = 0f;
    }

    public void OnExit()
    {
        // 退出翻滚状态时关闭无敌
        player.invulnerable = false;
        // 重置动画状态
        player.ani.SetBool("isDodge", false);
    }

    public void OnFixedUpdate()
    {
        Dodge();
    }

    public void OnUpdate()
    {
        // 检测是否结束翻滚
        if (!player.isDodging)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        // 受伤状态切换（翻滚时无敌，此判断可保留但实际不会触发伤害）
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }

    // 翻滚逻辑实现
    void Dodge()
    {
        if (!player.isDodegOnCooldown)
        {
            if (dodgeTimer <= player.dodgeDuration)
            {
                // 施加翻滚力（使用输入方向作为翻滚方向）
                player.rb.AddForce(player.inputDirection * player.dodgeForce, ForceMode2D.Impulse);
                dodgeTimer += Time.fixedDeltaTime;
            }
            else
            {
                // 结束翻滚
                player.rb.velocity = Vector2.zero;
                player.isDodging = false;
                player.isDodegOnCooldown = true;
                player.DodgeOnCooldown(); // 启动冷却
                Debug.Log("闪避结束，开始冷却");
                dodgeTimer = 0;
            }
        }
    }
}
