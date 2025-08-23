using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("属性")]
    [SerializeField]protected float maxHealth;
    [SerializeField]protected float curHealth;

    [Header("无敌")]
    public bool invulnerable;
    public float invulnerableDuration;

    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    protected virtual void OnEnable()
    {
        curHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (invulnerable) return ;
        if (curHealth - damage > 0f) 
        {
            curHealth -= damage;
            StartCoroutine(nameof(InvulnerableCoroutine));//启动无敌协程
            
            //执行受伤动画
            OnHurt?.Invoke();
        }
        else
        {
            Die();
        }
    }

    public virtual void Die()
    {
        curHealth = 0f;
        OnDie?.Invoke();
    }

    //无敌
    protected virtual IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;

        //等待无敌时间
        yield return new WaitForSeconds(invulnerableDuration);

        invulnerable = false;
    }
}
