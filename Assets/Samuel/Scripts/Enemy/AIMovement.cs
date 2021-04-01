using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICombat))]
public class AIMovement : MonoBehaviour
{
    [Range(0f, 10f)]
    public float range;
    [Range(0f, 1f)]
    public float deAggroTimer;
    [Range(0f, 2f)]
    public float attackRange;

    private AICombat aiCombat;
	private NavMeshAgent agent;
	private Transform target;

    [HideInInspector]
    public bool attacking = false;
    public bool drawRangeGizmos;

	private void Start()
	{
        aiCombat = GetComponent<AICombat>();
		agent = GetComponent<NavMeshAgent>();

		agent.updateRotation = false;
		agent.updateUpAxis = false;
        agent.isStopped = true;
	}

    private void Update()
    {
        if (!agent || attacking) return;

        transform.up = agent.velocity.normalized;
        agent.SetDestination(target.position);

        if (Vector2.Distance(transform.position, target.position) < range)
            agent.isStopped = false;
        else if (agent.remainingDistance > range && !agent.isStopped)
            Invoke(nameof(DeAggro), deAggroTimer);
        else
            agent.isStopped = true;

        if(Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            aiCombat.Attack();
        }
    }
    public void UpdateTarget(Transform _target)
    {
		target = _target;
	}

    private void DeAggro()
    {
        if (agent.remainingDistance > range)
            agent.isStopped = true;
    }

    private void OnDrawGizmos()
    {
        if (!drawRangeGizmos) return;

        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
