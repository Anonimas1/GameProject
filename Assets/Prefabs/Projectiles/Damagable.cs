using System;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int MaxHealth = 10;
    public int CurrentHealth = 10;
    public bool DestroyOnHealthZero;
    public event EventHandler<int> CurrentHealthChangedEventHandler;

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            if (DestroyOnHealthZero)
            {
                Destroy(gameObject);
            }
            return;
        }
        CurrentHealthChangedEventHandler?.Invoke(this, CurrentHealth);
    }
}

