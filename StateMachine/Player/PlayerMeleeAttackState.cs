using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҽ�ս����״̬
/// </summary>
public class PlayerMeleeAttackState : IState
{
    Player player;

    //���캯�� 
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

        //����
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }
}
