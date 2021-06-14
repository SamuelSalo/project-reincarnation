using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AI))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public enum Faction { Blue, Red };
    [HideInInspector] public Faction faction;

    public CharacterSheet characterSheet;

    //Stats
    [HideInInspector] public float attackRate;
    [HideInInspector] public float dashSpeed;
    [HideInInspector] public float dashCooldown;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float maxStamina;
    [HideInInspector] public float staminaRecovery;
    [HideInInspector] public float damage;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float health;

    //Components
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Player player;
    [HideInInspector] public FloatingHealthbar floatingHealthbar;
    [HideInInspector] public Room room;
    [HideInInspector] public GameSFX gameSFX;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public AI ai;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public SpriteFlash spriteFlasher;

    public bool invincible; 
    public bool isPlayer = false;
    public bool isBoss = false;
    

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

        maxHealth = characterSheet.maxHealth;
        maxStamina = characterSheet.maxStamina;
        staminaRecovery = characterSheet.staminaRecovery;
        damage = characterSheet.damage;
        attackRate = characterSheet.attackRate;
        dashCooldown = characterSheet.dashCooldown;
        dashSpeed = characterSheet.dashSpeed;
        movementSpeed = characterSheet.movementSpeed;
        faction = characterSheet.faction;
        ai.aiVariables = characterSheet.aiVariables;
        agent.speed = movementSpeed;
        player.moveSpeed = movementSpeed;
        player.moveSmoothing = characterSheet.moveSmoothing;
        player.turnSmoothing = characterSheet.turnSmoothing;

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
        if (invincible || (!isPlayer && ai.WillDodgeAttack())) return;

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
            var velocity = transform.InverseTransformVector(rb.velocity.normalized);
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
        }
        var remains = Instantiate(Resources.Load("Remains"), transform.position, Quaternion.identity) as GameObject;
        remains.GetComponent<Remains>().faction = faction;
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