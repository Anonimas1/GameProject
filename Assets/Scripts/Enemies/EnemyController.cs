using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    public Transform destination;
    public float destinationUpdateRate;
    public float attacksPerSecond = 1;
    public int damageDone = 2;
    
    
    private NavMeshAgent agent;
    private float secondsSinceLastAttack = 0f;
    private bool canAttack = true;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdateTargetPosition());
    }

    private void Update()
    {
        if (!canAttack)
        {
            secondsSinceLastAttack += Time.deltaTime;
        }

        if (secondsSinceLastAttack >= 1 / attacksPerSecond)
        {
            secondsSinceLastAttack = 0f;
            canAttack = true;
        }
    }

    private IEnumerator UpdateTargetPosition()
    {
        while (true)
        {
            agent.destination = destination.position;
            yield return new WaitForSeconds(destinationUpdateRate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Damageable>(out var damageable))
        {
            Attack(damageable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("stay");
        if (other.gameObject.TryGetComponent<Damageable>(out var damageable))
        {
            Attack(damageable);
        }
    }

    private void Attack(Damageable damageable)
    {
        if (canAttack)
        {
            damageable.TakeDamage(damageDone);
            canAttack = false;   
        }
    }
}
