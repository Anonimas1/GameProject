using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(IAttacker))]
public class EnemyController : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField]
    private string playerTag = "Player";

    [SerializeField]
    private LayerMask playerMask;

    [Header("Attack settings")]
    [SerializeField]
    private IAttacker meleeAttacker;

    private bool _isInAttackRange = false;

    [Header("Obstacle attack settings")]
    [SerializeField]
    private LayerMask damageableMask;

    [SerializeField]
    private LayerMask damageableExplosiveMask;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float movementThreshold = 0.2f;

    private int walkParameterId;

    private bool isAttackingObsticle;
    private Damageable CurrentObsticle;

    private Transform PlayerTransform;
    private Damageable PlayerDamagable;
    private Vector3 TargetPosition;

    private bool PathFound;
    private NavMeshPath _calculatedPath;
    private NavMeshAgent _agent;

    [Header("Debug")]
    [SerializeField]
    private float sphereRadius = 0.2f;

    [SerializeField]
    private float attackRangeOpacity = 0.2f;

    [SerializeField]
    private bool stayInPlace = false;

    [SerializeField]
    private float remainingDistance;

    [SerializeField]
    private State CurrentState;

    private enum State
    {
        ChasingPlayer = 0,
        AttackingPlayer,
        MovingToClosestPosition,
        AttackingObstacle,
        StandingStill
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _calculatedPath = new NavMeshPath();
        PlayerTransform = GameObject.FindWithTag(playerTag).transform;
        PlayerDamagable = PlayerTransform.GetComponent<Damageable>();
        TargetPosition = PlayerTransform.position;
        meleeAttacker = GetComponent<IAttacker>();
        CurrentState = State.StandingStill;
        walkParameterId = Animator.StringToHash("Walk Forward");
    }


    private void UpdateState()
    {
        if (stayInPlace && CurrentState != State.StandingStill)
        {
            CurrentState = State.StandingStill;
            return;
        }

        _isInAttackRange = Physics.CheckSphere(transform.position, meleeAttacker.Range, playerMask);

        if (_isInAttackRange)
        {
            currentAttackTarget = PlayerDamagable;
            CurrentState = State.AttackingPlayer;
            return;
        }

        UpdatePath();

        if (PathFound)
        {
            CurrentState = State.ChasingPlayer;
            return;
        }

        CurrentState = State.MovingToClosestPosition;
    }

    private Damageable currentAttackTarget;


    private void Update()
    {
        animator.SetBool(walkParameterId, _agent.velocity.magnitude > movementThreshold);
        
        switch (CurrentState)
        {
            case State.AttackingPlayer:
            case State.AttackingObstacle:
                if (currentAttackTarget != null && Vector3.Distance(currentAttackTarget.transform.position, transform.position) <= meleeAttacker.Range)
                {
                    _agent.destination = transform.position;
                    Quaternion neededRotation = Quaternion.LookRotation(currentAttackTarget.transform.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * 360f);  
                    meleeAttacker.Attack(currentAttackTarget);
                }
                else
                {
                    UpdateState();
                }
                break;
            case State.MovingToClosestPosition:
                if (_calculatedPath.corners.Any())
                {
                    _agent.SetDestination(_calculatedPath.corners.Last());   
                }
                else
                {
                    _agent.SetDestination(transform.position);
                }
                if (_agent.remainingDistance < meleeAttacker.Range)
                {
                    var colliders = Physics.OverlapSphere(transform.position, meleeAttacker.Range, damageableMask);
                    if (colliders.Any())
                    {
                        currentAttackTarget = colliders[0].GetComponent<Damageable>();
                        CurrentState = State.AttackingObstacle;
                        return;
                    }
                }

                UpdateState();
                break;
            case State.ChasingPlayer:
                _agent.SetDestination(PlayerTransform.position);
                UpdateState();
                break;
            case State.StandingStill:
                _agent.SetDestination(transform.position);
                UpdateState();
                break;
        }
    }

    private void UpdatePath()
    {
        PathFound = _agent.CalculatePath(PlayerTransform.position, _calculatedPath) && _calculatedPath.status == NavMeshPathStatus.PathComplete;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var corner in _calculatedPath.corners)
        {
            Gizmos.DrawSphere(corner, sphereRadius);
        }
        
        remainingDistance = _agent.remainingDistance;
        if (_isInAttackRange)
        {
            var color = Color.red;
            color.a = attackRangeOpacity;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, meleeAttacker.Range);
        }
        else
        {
            var color = Color.green;
            color.a = attackRangeOpacity;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, meleeAttacker.Range);
        }
    }
}