using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] float curHealth = 3;
    public int team = 0;
    float maxHealth = 1;
    public HitEvent hitEvent;
    [SerializeField] float invincibleTime = 0f;
    public UnityEvent deathEvent;

    void Start()
    {
        StartStuff();
    }

    public void StartStuff()
    {
        maxHealth = curHealth;
    }
    public virtual void Hit(float damage)
    {
        if (IsInvoking("Invincible") == false)
        {
            curHealth -= damage;
            HealthEvent(curHealth / maxHealth * 100);
            hitEvent.Invoke(curHealth / maxHealth * 100);
            if (curHealth <= 0)
            {
                Die();
            }
            Invoke("Invincible", invincibleTime);
        }

    }

    void Invincible()
    {
        //invoke function
    }

    public void HealthEvent(float value)
    {
        hitEvent.AddListener(HealthEvent);
    }



    public virtual void Die()
    {
      //  Destroy(transform.root.gameObject);
      deathEvent.Invoke();
    }
}

[System.Serializable]
public class HitEvent : UnityEvent<float> { }
