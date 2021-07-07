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

    public AIVariables aiVariables;
    public CharacterStats characterStats;

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
    [HideInInspector] public Animator animator;
    [HideInInspector] public Player player;
    [HideInInspector] public FloatingHealthbar floatingHealthbar;
    [HideInInspector] public Room room;
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

        UpdateStats();

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
        float adjDamage = _damage;

        if(PerkManager.instance.whetstone > 0 && PercentageChance(20) && isPlayer)
        {
            adjDamage += 10 * PerkManager.instance.whetstone;
        }

        _target.TakeDamage(adjDamage, this);
    }

    /// <summary>
    /// This character takes damage from another character.
    /// </summary>
    public void TakeDamage(float _damage, Character _source)
    {
        if (invincible || (!isPlayer && ai.WillDodgeAttack())) return;

        if(PerkManager.instance.luckyCharm > 0 && PercentageChance(5 * PerkManager.instance.luckyCharm) && isPlayer)
        {
            //TODO perk fx
            return;
        }

        if(PerkManager.instance.gravelordsCurse > 0 && isPlayer)
        {
            InventoryManager.instance.SpendTokens(5 * PerkManager.instance.gravelordsCurse);
        }

        GameSFX.instance.PlayHurtSFX();
        health -= _damage;

        if (health <= 0)
            Death(_source);

        spriteFlasher.Flash();
        UpdateHealthbar();

        CombatText.instance.ShowDamageText(_damage, (Vector2)transform.position + (Vector2)Random.onUnitSphere);
    }
    /// <summary>
    /// Restore health to this character.
    /// </summary>
    public void RestoreHealth(float _amount)
    {
        health = Mathf.Clamp(health + _amount, 0, maxHealth);
        spriteFlasher.Flash();

        UpdateHealthbar();
        GameSFX.instance.PlayHealSFX();

        CombatText.instance.ShowHealText(_amount, (Vector2)transform.position + (Vector2)Random.onUnitSphere);
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
        GameSFX.instance.PlayDeathSFX();

        if (isPlayer)
        {
            GameManager.instance.PlayerDeath(_killer);
        }  
        else
        {
            GameManager.instance.PlayerKill(this);
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
        if (!floatingHealthbar) return;

        if (isPlayer) floatingHealthbar.visible = true;
        floatingHealthbar.SetFillColor(isPlayer ? Color.green : faction == Faction.Red ? Color.red : Color.blue);
        floatingHealthbar.SetHealthValue(health, maxHealth);
    }

    public void UpdateStats()
    {
        maxHealth = characterStats.maxHealth;
        maxStamina = characterStats.maxStamina;
        staminaRecovery = characterStats.staminaRecovery;
        damage = characterStats.damage;
        attackRate = characterStats.attackRate;
        dashCooldown = characterStats.dashCooldown;
        dashSpeed = characterStats.dashSpeed;
        movementSpeed = characterStats.movementSpeed;
        ai.aiVariables = aiVariables;
        agent.speed = movementSpeed;
        player.moveSpeed = movementSpeed;
        player.moveSmoothing = characterStats.moveSmoothing;
        player.turnSmoothing = characterStats.turnSmoothing;
    }

    /// <summary>
    /// Return bool from precentage chance random roll
    /// </summary>
    /// <param name="_percentage"></param>
    /// <returns></returns>
    private bool PercentageChance(float _percentage)
    {
        return Random.Range(0, 100) <= _percentage;
    }
}