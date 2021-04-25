using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    public enum State { Patrolling, Chasing, Attacking }

    [Header("Properties")]

    public State state = State.Patrolling;

    [Space]

    [Range(0,10)]public float chaseRange = 5f;
    [Range(0, 10)] public float patrolRange = 4f;
    [Range(0, 10)] public float attackRange = 1f;

    [Space]

    [Range(0, 5)] public float chaseSpeed = 2.5f;
    [Range(0, 5)] public float patrolSpeed = 1.5f;

    [Space]

    [Range(0f, 0.3f)] public float turnSmoothing = 0.2f;

    [HideInInspector] public Transform target;

	private NavMeshAgent agent;
    private Character character;

    private Vector2 patrolDestination = Vector2.zero;

    [Space]

    public bool drawRangeGizmos = false;
    private bool idling = false;
    private float timer;
    private float patrollingTimer;

    private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
        character = GetComponent<Character>();

		agent.updateRotation = false;
		agent.updateUpAxis = false;
        agent.isStopped = true;
	}

    private void OnDrawGizmos()
    {
        if (!drawRangeGizmos) return;

        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, patrolRange);
        Gizmos.DrawLine(transform.position, agent.destination);
    }

    // Update state of AI, then do actions depending on said state.
    private void FixedUpdate()
    {
        if (!target) return;

        UpdateAIState();
        character.UpdateAnimator();

        switch (state)
        {
            case State.Patrolling:
                UpdatePatrolCycle();
                agent.speed = patrolSpeed;
                character.floatingHealthbar.visible = false;
                break;

            case State.Chasing:
                UpdateChaseCycle();
                agent.speed = chaseSpeed;
                character.floatingHealthbar.visible = true;
                break;

            case State.Attacking:
                transform.up = (Vector2)(target.position - transform.position).normalized;
                character.floatingHealthbar.visible = true;
                break;
        }
    }

    /// <summary>
    /// If target is in chase range, go to chase state.
    /// If not, go to patrol state.
    /// If character is within attack range of player, stop  and attack him.
    /// </summary>
    private void UpdateAIState()
    {
        if (Vector2.Distance(transform.position, target.position) < chaseRange
            && character.gameManager.playerFaction != character.faction)
        {
            state = State.Chasing;
        }
            
        else if (agent.remainingDistance > chaseRange && !agent.isStopped)
        {
            Invoke(nameof(DeAggro), 1f);
        }
        else
        {
            state = State.Patrolling;
        } 

        if (Vector2.Distance(transform.position, target.position) <= attackRange
            && character.gameManager.playerFaction != character.faction)
        {
            agent.isStopped = true;
            state = State.Attacking;
            Attack();
        }
    }

    /// <summary>
    /// Stop chasing player after a delay of him leaving chase range.
    /// </summary>
    private void DeAggro()
    {
        if (agent.remainingDistance > chaseRange)
        {
            state = State.Patrolling;
        }
    }

    /// <summary>
    /// If AI has no destination, get one.
    /// If AI destination isn't that destination, set it.
    /// When destination is reached, reset it.
    /// </summary>
    private void UpdatePatrolCycle()
    {
        if (idling) return;

        if (patrolDestination == Vector2.zero)
            patrolDestination = RandomPatrolDestination();

        else if((Vector2)agent.destination != patrolDestination)
            agent.SetDestination(patrolDestination);

        transform.up = Vector2.Lerp(transform.up, agent.velocity.normalized, turnSmoothing);
        agent.isStopped = false;
        patrollingTimer += Time.fixedDeltaTime;

        if (Vector2.Distance(transform.position, patrolDestination) < attackRange || agent.isPathStale || patrollingTimer > 5f)
            StartCoroutine(ResetPatrol());
    }

    /// <summary>
    /// Update player position as chase destination.
    /// Rotate character to face traveling direction.
    /// </summary>
    private void UpdateChaseCycle()
    {
        transform.up = Vector2.Lerp(transform.up, agent.velocity.normalized, turnSmoothing);
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    /// <summary>
    /// Returns a random unobstructed location within a min/max radius of this character in world space.
    /// </summary>
    private Vector2 RandomPatrolDestination()
    {
        
        Vector2 position;

        do
        {
            position = Random.insideUnitCircle.normalized * Random.Range(2f, patrolRange);
            position = transform.TransformPoint(position);
        }
        while (Physics2D.OverlapCircle(position, 0.5f, LayerMask.GetMask("Wall")) || !Physics2D.OverlapCircle(position, 0.5f));
        return position;
    }

    /// <summary>
    /// Wait for a random duration and reset the patrol destination.
    /// Next UpdatePatrolCycle call will automatically get a new destination.
    /// </summary>
    private IEnumerator ResetPatrol()
    {
        patrollingTimer = 0f;
        idling = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        patrolDestination = Vector2.zero;
        idling = false;
    }

    /// <summary>
    /// Activates a temporary hurtbox and deals damage to all hostile characters inside it.
    /// </summary>
    public void ActivateAttackHurtbox()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position + transform.up, new Vector2(1f, 1f), 0f);
        if (hits.Length != 0)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.CompareTag("Player") && hit.transform.GetComponent<Character>().faction != character.faction)
                    character.DealDamage(character.damage, hit.transform.GetComponent<Character>());
            }
        }
    }
    /// <summary>
    /// AIMovement calls this to attack when in range.
    /// </summary>
    public void Attack()
    {
        if (Time.time < timer) return;
        timer = Time.time + (1f / character.attackRate);
        character.animator.SetTrigger("Attack");
        state = State.Chasing;
    }
}