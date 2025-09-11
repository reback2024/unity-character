using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///µĞÈËËÀÍö×´Ì¬
///</summary>

public class EnemyDeathState : IState
{
    public Enemy enemy;
    public EnemyDeathState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void OnEnter()
    {
        enemy.animator.Play("Die");
        enemy.rb.velocity = Vector2.zero;
        enemy.enemyColler.enabled = false;//½ûÓÃÅö×²Ìå
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
