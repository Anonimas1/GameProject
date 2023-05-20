using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Damageable))]
public class EnemyController : MonoBehaviour
{
    public Transform destination;

    [SerializeField]
    private float destinationUpdateRate;

    [SerializeField]
    public float attacksPerSecond = 1;

    [SerializeField]
    public int damageDone = 2;

    [Header("On death")]
    [SerializeField]
    private GameObject objectToSpawnOnDeath;

    [SerializeField]
    private float spawnChance = 0.2f;

    private Damageable damageable;
    private NavMeshAgent agent;
    private float secondsSinceLastAttack = 0f;
    private bool canAttack = true;

    void Start()
    {
        damageable = GetComponent<Damageable>();
        damageable.DamageableHealthBelowZeroHandler += (_, _) => SpawnOnDeath();
        agent = GetComponent<NavMeshAgent>();
        UpdateTargetPosition();
    }

    private void SpawnOnDeath()
    {
        if (Random.Range(0, 1f) < spawnChance)
        {
            Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!IsTargetReachable())
        {
            var origin = transform.position;
            var direction = destination.position - transform.position;
            var ray = new Ray(origin, direction);

            var hits = Physics.RaycastAll(ray, 200f);
            var firstBarrel = hits.FirstOrDefault(x => x.transform.gameObject.CompareTag("PlayerPlaced"));
            agent.destination = firstBarrel.transform.position; 
        }
        else
        {
            UpdateTargetPosition();
        }
        
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

    private bool IsTargetReachable()
    {
        var lasCorner = agent.path.corners.Last();
        if (destination.position.x != lasCorner.x && destination.position.z != lasCorner.z ||
            agent.path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }

        return true;
    }

    private void UpdateTargetPosition()
    {
        agent.destination = destination.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NonPlayerPlaced")
        {
            return;
        }
        
        if (other.gameObject.TryGetComponent<Damageable>(out var damageable))
        {
            Attack(damageable);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "NonPlayerPlaced")
        {
            return;
        }
        
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