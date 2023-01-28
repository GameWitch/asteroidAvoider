using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event EventHandler OnDeath;
    public event EventHandler<OnHealthChangeEventArgs> OnHealthDecrease;
    public event EventHandler<OnHealthChangeEventArgs> OnHealthIncrease;
    public class OnHealthChangeEventArgs : EventArgs { public int _health; }


    int health = 5;
    bool winState = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (winState) return;
        IDealDamage damageDealer = collision.gameObject.GetComponent<IDealDamage>();
        if (damageDealer != null)
        {
            TakeDamage(damageDealer.DealDamage());
        }
    }

    public int GetHealthValue()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthDecrease?.Invoke(this, new OnHealthChangeEventArgs { _health = health });

        if (health <= 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);            
        }
    }

    public void IncreaseHealth(int healthToAdd)
    {
        health += healthToAdd;
        OnHealthIncrease?.Invoke(this, new OnHealthChangeEventArgs { _health = health });
    }

    public void Win()
    {
        winState = true;
    }
}
