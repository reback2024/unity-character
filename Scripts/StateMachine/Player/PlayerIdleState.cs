using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家待机状态
/// </summary>
public class PlayerIdleState : IState
{
    Player player;

    //构造函数 
    public PlayerIdleState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.ani.Play("playeridle");
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        player.ani.SetFloat("speed", player.rb.velocity.magnitude);

        //移动
        if (player.inputDirection != Vector2.zero)
        {
            player .TransitionState(PlayerStateType.Move);
        }

        //翻滚
        if (player.isDodging)
        {
            player.TransitionState(PlayerStateType.Dodge);
        }

        //近战攻击
        if(player.isMeleeAttack)
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
