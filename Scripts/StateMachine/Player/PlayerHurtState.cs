using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������״̬
/// </summary>
public class PlayerHurtState : IState
{
    Player player;

    private AnimatorStateInfo info;

    //���캯�� 
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

        if (info.normalizedTime >= .95f) //�����ŵ�95%���ϵ�ʱ��
        {
            player.TransitionState(PlayerStateType.Idle);
        }
    }
}
