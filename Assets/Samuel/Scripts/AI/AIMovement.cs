using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICombat))]
public class AIMovement : MonoBehaviour
{
    public enum State { Patrolling, Chasing, Attacking }

    [Header("Properties")]
    public State state = State.Patrolling;

    [Space]
    [Range(0,10)]public float chaseRange;
    [Range(0, 10)] public float patrolRange;
    [Range(0, 10)] public float attackRange;

    [Space]
    [Range(0, 5)] public float chaseSpeed;
    [Range(0, 5)] public float patrolSpeed;

    [Space]
    [Range(0f, 0.3f)] public float turnSmoothing;

    [HideInInspector] public Transform target;

    private AICombat aiCombat;
	private NavMeshAgent agent;
    private Character character;

    private Vector2 patrolDestination = Vector2.zero;

    [Space]
    public bool drawRangeGizmos;
    private bool idling = false;

    private void Start()
	{
        aiCombat = GetComponent<AICombat>();
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
    }

    // Update state of AI, then do actions depending on said state.
    private void FixedUpdate()
    {
        UpdateAIState();
        character.UpdateAnimator();

        switch (state)
        {
            case State.Patrolling:
                UpdatePatrolCycle();
                agent.speed = patrolSpeed;
                break;

            case State.Chasing:
                UpdateChaseCycle();
                agent.speed = chaseSpeed;
                break;

            case State.Attacking:
                transform.up = (Vector2)(target.position - transform.position).normalized;
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
            && character.gameManager.currentFaction != character.faction)
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
            && character.gameManager.currentFaction != character.faction)
        {
            agent.isStopped = true;
            state = State.Attacking;
            aiCombat.Attack();
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

        if (Vector2.Distance(transform.position, patrolDestination) < attackRange)
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
        while (Physics2D.OverlapCircle(position, 0.5f, LayerMask.GetMask("Wall")));
        return position;
    }

    /// <summary>
    /// Wait for a random duration and reset the patrol destination.
    /// Next UpdatePatrolCycle call will automatically get a new destination.
    /// </summary>
    private IEnumerator ResetPatrol()
    {
        idling = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        patrolDestination = Vector2.zero;
        idling = false;
    }
}