using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour,IDamageable
{
    Rigidbody2D rb;
    Collider2D col;

    public int health;

    public int Health
    {
        get 
        {
            return health; 
        } 
        set 
        {
            health = value;
            if(health<=0)
            {
                gameObject.BroadcastMessage("OnDie");
                Tra = false;
            }
            else
            {
                gameObject.BroadcastMessage("OnDamage");
            }
        }
    }

    bool tra;
    public bool Tra
    {
        get
        {
            return tra;
        }
        set
        {
            tra = value;
            if (!tra)
            {
                rb.simulated = false;
            }
        }
    }

    public void OnHit(int damage,Vector2  knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    
    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
}
