using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������״̬
/// </summary>
public class PlayerDeathState : IState
{
    Player player;

    //���캯�� 
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
