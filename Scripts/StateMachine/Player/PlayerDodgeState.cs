using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������״̬
/// </summary>
public class PlayerDodgeState : IState
{
    Player player;

    private float dodgeTimer = 0f;

    //���캯�� 
    public PlayerDodgeState(Player player)
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
        Dodge();
    }

    public void OnUpdate()
    {
        player.ani.SetBool("isDodge", player.isDodging);

        if (player.isDodging == false)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        //����
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }

    //����
    void Dodge()
    {
        if (!player.isDodegOnCooldown)
        {

            if (dodgeTimer <= player.dodgeDuration)
            {
                player.rb.AddForce(player.inputDirection * player.dodgeForce, ForceMode2D.Impulse);
                dodgeTimer += Time.fixedDeltaTime;
            }
            else
            {
                player.isDodging = false;
                player.isDodegOnCooldown = true;
                player.DodgeOnCooldown();//��ȴ����
                Debug.Log("���ܿ�ʼ��ȴ,ʱ��Ϊ1s");
                dodgeTimer = 0;
            }
        }
    }
}
