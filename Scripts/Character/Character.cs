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

    [Header("UI")]
    public UnityEvent<float, float> OnhealthUpdate;

    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    protected virtual void OnEnable()
    {
        curHealth = maxHealth;
        OnhealthUpdate?.Invoke(maxHealth, curHealth);
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
            //死亡
            Die();
        }

        GameManager.Instance.ShowText("-" + damage, transform.position, Color.red);

        OnhealthUpdate?.Invoke(maxHealth, curHealth);//更新血量UI
    }

    //回复血量
    public virtual void RestoreHealth(float value)
    {
        if (curHealth == maxHealth) return;
        if (curHealth + value > maxHealth) curHealth = maxHealth;
        else curHealth += value;
        OnhealthUpdate?.Invoke(maxHealth, curHealth);//更新血量UI
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
