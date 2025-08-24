using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家受伤状态
/// </summary>
public class PlayerHurtState : IState
{
    Player player;

    private AnimatorStateInfo info;

    //构造函数 
    public PlayerHurtState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.ani.SetTrigger("Hurt");
    }

    public void OnExit()
    {
        player.isHurt = false;
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        info = player.ani.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= .95f) //当播放到95%以上的时候
        {
            player.TransitionState(PlayerStateType.Idle);
        }
    }
}
