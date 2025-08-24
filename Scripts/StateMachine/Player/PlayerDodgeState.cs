using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家闪避状态
/// </summary>
public class PlayerDodgeState : IState
{
    Player player;

    private float dodgeTimer = 0f;

    //构造函数 
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

        //受伤
        if (player.isHurt)
        {
            player.TransitionState(PlayerStateType.Hurt);
        }
    }

    //翻滚
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
                player.DodgeOnCooldown();//冷却调用
                Debug.Log("闪避开始冷却,时间为1s");
                dodgeTimer = 0;
            }
        }
    }
}
