using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damagable : MonoBehaviour
{
    public int MaxHealth = 10;
    public int CurrentHealth = 10;
    public bool DestroyOnHealthZero;
    public ScoreTracker ScoreTracker;  
    public event EventHandler<int> CurrentHealthChangedEventHandler;

    public void Start()
    {
        ScoreTracker = GameObject.FindWithTag("ScoreTracker").GetComponent<ScoreTracker>();
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            if (DestroyOnHealthZero)
            {
                Destroy(gameObject);
            }

            ScoreTracker.EnemyKilled();
            return;
        }
        CurrentHealthChangedEventHandler?.Invoke(this, CurrentHealth);
    }
}

