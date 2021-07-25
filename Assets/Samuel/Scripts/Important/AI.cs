using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using SFXType = GameSFX.SFXType;

[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    public enum State { Patrolling, Chasing, Attacking }
    public State state = State.Patrolling;

    [HideInInspector] public AIVariables aiVariables;
    public Transform target;

    private float chaseRange;
    private float patrolRange;
    private float attackRange;
    [HideInInspector]public float chaseSpeed;
    private float patrolSpeed;
    private float turnSmoothing;

    private Character character;
    private NavMeshAgent agent;
    private Vector2 patrolDestination = Vector2.zero;
    private Vector2 facingDirection;

    [HideInInspector] public bool slowed;
    public bool drawRangeGizmos = false;
    private bool idling = false;
    private float timer;
    private float patrollingTimer;
    private bool attacking;
    private bool dashing;
    private float attackDuration;

    private void Start()
	{
        character = GetComponent<Character>();
    }

    public void InitializeAI()
    {
        attackDuration = 0.5f;
        agent = character.agent;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;
        transform.rotation = Quaternion.identity;
        chaseRange = aiVariables.chaseRange;
        patrolRange = aiVariables.patrolRange;
        attackRange = aiVariables.attackRange;
        chaseSpeed = character.characterStats.movementSpeed;
        patrolSpeed = aiVariables.patrolSpeed;
        turnSmoothing = character.characterStats.turnSmoothing;
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
        if (target==null || dashing) return;
        UpdateAIState();
        character.UpdateAnimator(facingDirection);

        switch (state)
        {
            case State.Patrolling:
                UpdatePatrolCycle();
                if (!slowed) agent.speed = patrolSpeed;
                character.floatingHealthbar.visible = false;
                break;

            case State.Chasing:
                UpdateChaseCycle();
                if (!slowed) agent.speed = chaseSpeed;
                character.floatingHealthbar.visible = true;
                break;

            case State.Attacking:
                character.floatingHealthbar.visible = true;
                break;
        }
        if(attacking)
        {
            agent.velocity = Vector3.zero;
        }

        facingDirection = agent.velocity.normalized;
        facingDirection.x = Mathf.Round(facingDirection.x);
        facingDirection.y = Mathf.Round(facingDirection.y);
    }

    /// <summary>
    /// If target is in chase range, go to chase state.
    /// If not, go to patrol state.
    /// If character is within attack range of player, stop  and attack him.
    /// </summary>
    private void UpdateAIState()
    {
        if (target == null) return;

        if (Vector2.Distance(transform.position, target.position) < chaseRange
            && GameManager.instance.playerFaction != character.faction &&
            Vector2.Distance(transform.position, target.position) > attackRange
            && GameManager.instance.currentRoom == character.room)
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
            && GameManager.instance.playerFaction != character.faction)
        {
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
            StartCoroutine(RandomPatrolDestination());

        else if ((Vector2)agent.destination != patrolDestination)
            agent.SetDestination(patrolDestination);

        agent.isStopped = false;
        patrollingTimer += Time.fixedDeltaTime;
        //chaseRange = PerkManager.instance.perkDictionary["Hooded Cloak"] > 0 ? aiVariables.chaseRange * 0.8f : aiVariables.chaseRange;
        if (Vector2.Distance(transform.position, patrolDestination) < attackRange || agent.isPathStale || patrollingTimer > 5f)
            StartCoroutine(ResetPatrol());
    }

    /// <summary>
    /// Update player position as chase destination.
    /// Rotate character to face traveling direction.
    /// </summary>
    private void UpdateChaseCycle()
    {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    /// <summary>
    /// Returns a random unobstructed location within a min/max radius of this character in world space.
    /// </summary>
    private IEnumerator RandomPatrolDestination()
    {
        Vector2 position;
        var range = patrolRange;
        do
        {
            range += 0.1f;
            position = Random.insideUnitCircle.normalized * Random.Range(attackRange * 2, range);
            position = transform.TransformPoint(position);
        }
        while (Physics2D.OverlapCircle(position, 0.5f, LayerMask.GetMask("Wall")) || !Physics2D.OverlapCircle(position, 0.5f));
        patrolDestination = position;
        yield return null;
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingDirection, 0.5f);
        bool miss = true;
        if (hits.Length != 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.CompareTag("Player") && hit.collider.isTrigger)
                {
                    miss = false;

                    var hitCharacter = hit.transform.GetComponent<Character>();
                    if (hitCharacter.faction != character.faction)
                        character.DealDamage(character.damage, hitCharacter);
                }
                else miss = true;

                if (!miss)
                {
                    GameSFX.instance.PlaySFX(SFXType.Hit);
                }
                else
                {
                    GameSFX.instance.PlaySFX(SFXType.Miss);
                }
            }
        }
    }
    /// <summary>
    /// AIMovement calls this to attack when in range.
    /// </summary>
    public void Attack()
    {
        if (Time.time < timer) return;
        StartCoroutine(AttackRoutine());
        timer = Time.time + (1f / character.attackRate);
        character.animator.SetTrigger("attack");
    }


    /// <summary>
    /// Randomly choose to dodge attack or not
    /// </summary>
    /// <returns></returns>
    public bool WillDodgeAttack()
    {
        if(Random.Range(0,100) >= 30)
        {
            return false;
        }
        else
        {
            var dir = -(target.position - transform.position).normalized;
            StartCoroutine(AIPerformDash(dir));
            return true;
        }
    }

    /// <summary>
    /// AI Dashing is performed here
    /// RB interferes with AI, so disable AI for the duration.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public IEnumerator AIPerformDash(Vector2 direction)
    {
        GameSFX.instance.PlaySFX(SFXType.Dash);
        character.spriteTinter.DurationTint(new Color(1, 1, 1, 0.5f), 0.1f);
        dashing = true;
        agent.enabled = false;
        character.rb.bodyType = RigidbodyType2D.Dynamic;
        character.rb.velocity = direction * (character.dashSpeed / 2);
        yield return new WaitForSeconds(0.1f);
        character.rb.velocity = Vector2.zero;
        agent.enabled = true;
        character.rb.bodyType = RigidbodyType2D.Kinematic;
        dashing = false;
    }

    private IEnumerator AttackRoutine()
    {
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
    }
}