using System.Collections;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour, IAttacker
{
    [SerializeField]
    private int damage = 2;

    [SerializeField]
    private float timeBetweenAttacks = 1f;

    public float Range { get; } = 2f;
    
    [SerializeField]
    private float damageDealtDelay = 0.5f;
    
    [SerializeField]
    private Animator animator;

    private int AttackParameter = Animator.StringToHash("PunchTrigger");


    private bool attacked;

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
        StartCoroutine(TryDamage(target));
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    private IEnumerator TryDamage(Damageable target)
    {
        yield return new WaitForSeconds(damageDealtDelay);

        if (Vector3.Distance(target.transform.position, transform.position) < Range)
        {
            target.TakeDamage(damage);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }
}