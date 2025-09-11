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

    // ���캯��
    public PlayerDodgeState(Player player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        // ���뷭��״̬ʱ�����޵�
        player.invulnerable = true;
        // ���ŷ�������
        player.ani.SetBool("isDodge", true);
        // ���ü�ʱ��
        dodgeTimer = 0f;
    }

    public void OnExit()
    {
        // �˳�����״̬ʱ�ر��޵�
        player.invulnerable = false;
        // ���ö���״̬
        player.ani.SetBool("isDodge", false);
    }

    public void OnFixedUpdate()
    {
        Dodge();
    }

    public void OnUpdate()
    {
        // ����Ƿ��������
        if (!player.isDodging)
        {
            player.TransitionState(PlayerStateType.Idle);
        }

        // ����״̬�л�������ʱ�޵У����жϿɱ�����ʵ�ʲ��ᴥ���˺���
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }

    // �����߼�ʵ��
    void Dodge()
    {
        if (!player.isDodegOnCooldown)
        {
            if (dodgeTimer <= player.dodgeDuration)
            {
                // ʩ�ӷ�������ʹ�����뷽����Ϊ��������
                player.rb.AddForce(player.inputDirection * player.dodgeForce, ForceMode2D.Impulse);
                dodgeTimer += Time.fixedDeltaTime;
            }
            else
            {
                // ��������
                player.rb.velocity = Vector2.zero;
                player.isDodging = false;
                player.isDodegOnCooldown = true;
                player.DodgeOnCooldown(); // ������ȴ
                Debug.Log("���ܽ�������ʼ��ȴ");
                dodgeTimer = 0;
            }
        }
    }
}
