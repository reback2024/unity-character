using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("����")]
    [SerializeField]protected float maxHealth;
    [SerializeField]protected float curHealth;

    [Header("�޵�")]
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
            StartCoroutine(nameof(InvulnerableCoroutine));//�����޵�Э��
            
            //ִ�����˶���
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

    //�޵�
    protected virtual IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;

        //�ȴ��޵�ʱ��
        yield return new WaitForSeconds(invulnerableDuration);

        invulnerable = false;
    }
}
