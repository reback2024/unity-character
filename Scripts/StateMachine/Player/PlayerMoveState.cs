using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ƶ�״̬
/// </summary>

public class PlayerMoveState : IState
{
    Player player;

    //���캯�� 
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

        //����
        if (player.rb.velocity.magnitude < 0.01f)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        //����
        if (player.isDodging)
        {
            player.TransitionState(PlayerStateType.Dodge);
        }

        //��ս����
        if (player.isMeleeAttack)
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
