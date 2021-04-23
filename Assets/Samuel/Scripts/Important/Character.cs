using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(AI))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public enum Faction { Blue, Red };
    public Faction faction;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Player player;
    [HideInInspector] public FloatingHealthbar floatingHealthbar;

    private AI ai;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private SpriteFlash spriteFlasher;
    private GameSFX gameSFX;
    private Room room;

    [Space] [Range(0f, 1f)] public float attackRate;
    public bool isPlayer;
    public bool isBoss;
    public float maxHealth;
    public float maxStamina;
    public float staminaRecovery;
    public float damage;

    [HideInInspector] public float health;

    private void Start()
    {
        player = GetComponent<Player>();
        gameSFX = GetComponent<GameSFX>();
        spriteFlasher = GetComponent<SpriteFlash>();
        ai = GetComponent<AI>();
        agent = GetComponent<NavMeshAgent>();
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
        room = _controlled ? null : transform.parent.GetComponent<Room>();
        isPlayer = _controlled;

        player.enabled = _controlled;
        ai.enabled = !_controlled;
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
        gameSFX.PlayHurtSFX();
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
            animator.SetFloat("xMove", player.moveDirection.x);
            animator.SetFloat("yMove", player.moveDirection.y);
            animator.SetFloat("moveMagnitude", player.moveDirection.normalized.magnitude);
            animator.SetBool("rotationLock", player.rotationLock);
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
        gameSFX.PlayDeathSFX();

        if (isPlayer)
        {
            gameManager.PlayerDeath(_killer);
            room.npcs.Remove(_killer.transform);
        }  
        else
        {
            gameManager.PlayerKill(this);
            room.npcs.Remove(transform);
        }

        Destroy(gameObject);
        floatingHealthbar.Dispose();
    }

    /// <summary>
    /// Call combat scripts to activate hurtboxes.
    /// </summary>
    public void ActivateHurtbox()
    {
        if (isPlayer)
            player.ActivateAttackHurtbox();
        else
            ai.ActivateAttackHurtbox();
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