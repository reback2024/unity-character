using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ҵ���״̬
/// </summary>
public class PlayerIdleState : IState
{
    Player player;

    //���캯�� 
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

        //�ƶ�
        if (player.inputDirection != Vector2.zero)
        {
            player .TransitionState(PlayerStateType.Move);
        }

        //����
        if (player.isDodging)
        {
            player.TransitionState(PlayerStateType.Dodge);
        }

        //��ս����
        if(player.isMeleeAttack)
        {
            player.TransitionState(PlayerStateType.MeleeAttack);
        }

        //����
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }

    }
}
