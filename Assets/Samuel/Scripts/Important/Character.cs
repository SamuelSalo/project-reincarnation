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
    [HideInInspector] public Room room;

    [Space]

    public float attackRate = 0.75f;
    public bool isPlayer;
    public bool isBoss = false;
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float staminaRecovery = 10f;
    public float damage = 25f;

    [HideInInspector] public float health;

    private void Start()
    {
        player = GetComponent<Player>();
        spriteFlasher = GetComponent<SpriteFlash>();
        ai = GetComponent<AI>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameSFX = GameObject.FindWithTag("AudioManager").GetComponent<GameSFX>();

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

        if(!_controlled)
            room = transform.parent.GetComponent<Room>();

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
        gameSFX.PlayHealSFX();
    }

    /// <summary>
    /// Updates animator movement variables.
    /// Attack animations are handled separately.
    /// </summary>
    public void UpdateAnimator()
    {
        if (isPlayer)
        {
            if(player.moveDirection == Vector2.zero)
            {
                animator.SetFloat("xMove", 0f);
                animator.SetFloat("yMove", 0f);
                return;
            }
            var velocity = transform.InverseTransformVector(player.rb.velocity.normalized);
            animator.SetFloat("xMove", velocity.x);
            animator.SetFloat("yMove", velocity.y);
        }
        else
        {
            var velocity = transform.InverseTransformVector(agent.velocity.normalized);
            animator.SetFloat("xMove", velocity.x);
            animator.SetFloat("yMove", velocity.y);
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
        }  
        else
        {
            gameManager.PlayerKill(this);
            room.npcs.Remove(transform);

            if (_killer.faction == Faction.Red)
                _killer.RestoreHealth(20f);
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