using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    [Range(0, 20)]
    public float range;
    public float deAggroTimer;

	private NavMeshAgent agent;
	private Transform target;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        agent.isStopped = true;
	}

    private void FixedUpdate()
    {
		if (!agent) return;
		transform.up = agent.velocity.normalized;
		agent.SetDestination(target.position);
    }
	
    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < range)
            agent.isStopped = false;
        else if (agent.remainingDistance > range && !agent.isStopped)
            Invoke(nameof(DeAggro), deAggroTimer);
        else
            agent.isStopped = true;
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
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
