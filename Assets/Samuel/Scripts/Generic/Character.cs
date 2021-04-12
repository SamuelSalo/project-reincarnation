using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(AICombat))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public enum Faction { Blue, Red };
    public Faction faction;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public FloatingHealthbar floatingHealthbar;

    private AIMovement aiMovement;
    private AICombat aiCombat;
    private NavMeshAgent agent;
    private PlayerCombat playerCombat;
    private Rigidbody2D rb;
    private SpriteFlash spriteFlasher;

    [Space] [Range(0f, 1f)] public float attackRate;
    public bool isPlayer;
    public bool isBoss;
    public float maxHealth;
    public float damage;

    [HideInInspector] public float health;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteFlasher = GetComponent<SpriteFlash>();
        playerCombat = GetComponent<PlayerCombat>();
        aiMovement = GetComponent<AIMovement>();
        agent = GetComponent<NavMeshAgent>();
        aiCombat = GetComponent<AICombat>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        health = maxHealth;
        PlayerControlled(isPlayer);

        CreateHealthbar();
    }

    /// <summary>
    /// Update variables according to this character's controller (AI/Player).
    /// </summary>
    public void PlayerControlled(bool _controlled)
    {
        rb.bodyType = _controlled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;

        isPlayer = _controlled;

        playerMovement.enabled = _controlled;
        playerCombat.enabled = _controlled;
        aiMovement.enabled = !_controlled;
        aiCombat.enabled = !_controlled;
        agent.enabled = !_controlled;


        if (_controlled)
            transform.tag = "Player";
        else
            transform.tag = "AI";
    }
    /// <summary>
    /// This character deals damage to another character.
    /// </summary>
    public void DealDamage(float _damage, Character _target)
    {
        _target.TakeDamage(_damage, this);
    }

    /// <summary>
    /// This character takes damage from another character.
    /// </summary>
    public void TakeDamage(float _damage, Character _source)
    {
        health -= _damage;

        if (health <= 0)
            Death(_source);

        spriteFlasher.Flash();
        UpdateHealthbar();
    }
    /// <summary>
    /// Restore health to this character.
    /// </summary>
    public void RestoreHealth(float _amount)
    {
        health = Mathf.Clamp(health + _amount, 0, maxHealth);
        spriteFlasher.Flash();

        UpdateHealthbar();
    }

    /// <summary>
    /// Updates animator movement variables.
    /// Attack animations are handled separately.
    /// </summary>
    public void UpdateAnimator()
    {
        if (isPlayer)
        {
            animator.SetFloat("xMove", playerMovement.moveDirection.x);
            animator.SetFloat("yMove", playerMovement.moveDirection.y);
            animator.SetFloat("moveMagnitude", playerMovement.moveDirection.normalized.magnitude);
            animator.SetBool("rotationLock", playerMovement.rotationLock);
        }
        else
        {
            animator.SetFloat("moveMagnitude", agent.velocity.normalized.magnitude);
            animator.SetBool("rotationLock", false);
        }
    }

    /// <summary>
    /// Handle character death.
    /// Tells GameManager what died, and what killed it.
    /// </summary>
    private void Death(Character _killer)
    {
        if (isPlayer)
            gameManager.PlayerDeath(_killer);
        else
            gameManager.PlayerKill(this);

        Destroy(gameObject);

        floatingHealthbar.Dispose();
    }

    /// <summary>
    /// Call combat scripts to activate hurtboxes.
    /// </summary>
    public void ActivateHurtbox()
    {
        if (isPlayer)
            playerCombat.ActivateAttackHurtbox();
        else
            aiCombat.ActivateAttackHurtbox();
    }

    /// <summary>
    /// Creates a floating UI healthbar above the character
    /// </summary>
    public void CreateHealthbar()
    {
        var healthBarGO = Instantiate(Resources.Load("HealthbarPrefab"), GameObject.FindWithTag("HealthbarHolder").transform) as GameObject;
        floatingHealthbar = healthBarGO.GetComponent<FloatingHealthbar>();
        floatingHealthbar.target = transform;
        UpdateHealthbar();
    }

    /// <summary>
    /// Update value&color of healthbar.
    /// </summary>
    public void UpdateHealthbar()
    {
        if (isPlayer) floatingHealthbar.visible = true;
        floatingHealthbar.SetFillColor(isPlayer ? Color.green : faction == Faction.Red ? Color.red : Color.blue);
        floatingHealthbar.SetHealthValue(health, maxHealth);
    }
}