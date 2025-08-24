using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家移动状态
/// </summary>

public class PlayerMoveState : IState
{
    Player player;

    //构造函数 
    public PlayerMoveState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        player.Move();
    }

    public void OnUpdate()
    {
        player.ani.SetFloat("speed", player.rb.velocity.magnitude);

        //待机
        if (player.rb.velocity.magnitude < 0.01f)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        //翻滚
        if (player.isDodging)
        {
            player.TransitionState(PlayerStateType.Dodge);
        }

        //近战攻击
        if (player.isMeleeAttack)
        {
            player.TransitionState(PlayerStateType.MeleeAttack);
        }

        //受伤
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }
}
