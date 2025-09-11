using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家近战攻击状态
/// </summary>
public class PlayerMeleeAttackState : IState
{
    Player player;

    //构造函数 
    public PlayerMeleeAttackState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.ani.SetBool("isMeleeAttack", player.isMeleeAttack);

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
        if (player.isMeleeAttack == false)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        //受伤
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }
}
