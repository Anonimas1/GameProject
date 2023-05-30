using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeAttacker : MonoBehaviour, IAttacker
{
    [SerializeField]
    private GameObject thrownProjectilePlaceholderPrefab;

    [SerializeField]
    private GameObject projectileToThrowPrefab;

    [SerializeField]
    private float throwForce;

    [FormerlySerializedAs("throwPoint")]
    [SerializeField]
    private Transform plaheolderThrowPoint;

    [SerializeField]
    private Transform trueThrowPoint;

    [SerializeField]
    private float throwSpawnDelay;

    [SerializeField]
    private float timeBetweenAttacks = 1f;

    [SerializeField]
    private float _range = 2f;

    public float Range => _range;

    [SerializeField]
    private Animator animator;

    private int AttackParameter = Animator.StringToHash("PunchTrigger");

    [Header("debug")]
    [SerializeField]
    private bool attacked;
    [SerializeField]
    private bool hasBoulderInHand = false;
    private GameObject boulder;


    private void OnDestroy()
    {
        Destroy(boulder);
    }

    private void Update()
    {
        if (!attacked && !hasBoulderInHand)
        {
            boulder = Instantiate(thrownProjectilePlaceholderPrefab, plaheolderThrowPoint.position, Quaternion.identity);
            hasBoulderInHand = true;
        }

        if (hasBoulderInHand)
        {
            boulder.transform.position = plaheolderThrowPoint.position;
        }
    }

    public void Attack(Damageable target)
    {
        if (!attacked)
        {
            AttackInternal(target);
        }
    }

    private void AttackInternal(Damageable target)
    {
        transform.LookAt(target.transform);
        attacked = true;
        animator.SetTrigger(AttackParameter);
        StartCoroutine(Throw(target));
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private IEnumerator Throw(Damageable target)
    {
        yield return new WaitForSeconds(throwSpawnDelay);
        animator.gameObject.transform.localPosition = Vector3.zero;
        var forceDirection = (target.GetComponent<AttackTarget>().AttackPoint.position - trueThrowPoint.position).normalized;
        var projectile = Instantiate(projectileToThrowPrefab, trueThrowPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(forceDirection * throwForce);
        hasBoulderInHand = false;
        Destroy(boulder);
    }

    private void ResetAttack()
    {
        attacked = false;
    }
}

public interface IAttacker
{
    float Range { get; }
    void Attack(Damageable target);
}