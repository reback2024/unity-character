using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家死亡状态
/// </summary>
public class PlayerDeathState : IState
{
    Player player;

    //构造函数 
    public PlayerDeathState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.ani.SetBool("isDead",player.isDead);
    }

    public void OnExit()
    {
        
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
