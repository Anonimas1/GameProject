using System;
using DefaultNamespace;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Damageable : MonoBehaviour
{
    public int MaxHealth = 10;
    public int CurrentHealth = 10;
    public bool DestroyOnHealthZero;
    public ScoreTracker ScoreTracker;
    public event EventHandler<int> CurrentHealthChangedEventHandler;
    public event EventHandler DamageableHealthBelowZeroHandler;

    [SerializeField]
    private bool dropItemOnDeath;
    
    [SerializeField]
    private GameObject ammoPickup;

    public LootTable LootTable;
    
    
    [SerializeField]
    [CanBeNull]
    private AudioSource damageTakenAudio;

    private bool _isDamageTakenAudioNotNull;

    public void Start()
    {
        _isDamageTakenAudioNotNull = damageTakenAudio != null;
        ScoreTracker = GameObject.FindWithTag("ScoreTracker").GetComponent<ScoreTracker>();
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            if (dropItemOnDeath)
            {
                var amounts = LootTable.GetRandom();
                var obj = Instantiate(ammoPickup, transform.position, Quaternion.identity);
                var component = obj.GetComponent<AmmoPickup>();
                component.WeaponName = amounts.WeaponName;
                component.Ammount = amounts.Amount;
            }
            
            if (DestroyOnHealthZero)
            {
                Destroy(gameObject);
            }

            DamageableHealthBelowZeroHandler?.Invoke(this, EventArgs.Empty);
            ScoreTracker.EnemyKilled(gameObject.tag);
            return;
        }

        CurrentHealthChangedEventHandler?.Invoke(this, CurrentHealth);
        if (_isDamageTakenAudioNotNull)
        {
            damageTakenAudio.Play();   
        }
    }
}