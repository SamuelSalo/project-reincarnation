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
    [HideInInspector] public SpriteTint spriteTinter;

    public bool invincible; 
    public bool isPlayer = false;
    public bool isBoss = false;

    private int bleedDamage;

    private void Start()
    {
        player = GetComponent<Player>();
        spriteTinter = GetComponent<SpriteTint>();
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

    #region Damage Handling
    /// <summary>
    /// This character deals damage to another character.
    /// </summary>
    public void DealDamage(float _damage, Character _target)
    {
        float adjDamage = _damage;
        bool crit = false;
        //perks
        if(isPlayer)
        {
            if (PerkManager.instance.perkDictionary["Whetstone"] > 0 && PercentageChance(5 * PerkManager.instance.perkDictionary["Whetstone"]))
            {
                adjDamage *= 2;
                CameraShake.instance.Shake(0.25f, 0.5f);
                crit = true;
            }

            if (PerkManager.instance.perkDictionary["Tearstone Pendant"] > 0 && health < maxHealth / 3)
            {
                adjDamage *= 1f + (.1f * PerkManager.instance.perkDictionary["Tearstone Pendant"]);
            }
        }

        if (!crit)
            GameSFX.instance.PlayHurtSFX();
        else
            GameSFX.instance.PlayCritSFX();

        _target.TakeDamage(adjDamage, this);
    }

    /// <summary>
    /// This character takes damage from another character.
    /// </summary>
    public void TakeDamage(float _damage, Character _source)
    {
        float adjDamage = _damage;

        if (invincible || (!isPlayer && ai.WillDodgeAttack())) return;

        if (isPlayer)
        {
            if (PerkManager.instance.perkDictionary["Lucky Charm"] > 0 && PercentageChance(5 * PerkManager.instance.perkDictionary["Lucky Charm"]))
            {
                spriteTinter.FlashColor(Color.white);
                return;
            }
            if (PerkManager.instance.perkDictionary["Gravelord's Curse"] > 0)
            {
                InventoryManager.instance.SpendTokens(5 * PerkManager.instance.perkDictionary["Gravelord's Curse"]);
            }
            if (PerkManager.instance.perkDictionary["Thornmail Armor"] > 0)
            {
                _source.Bleed(3 * PerkManager.instance.perkDictionary["Thornmail Armor"], this);
            }
            if (PerkManager.instance.perkDictionary["Hemorrhage"] > 0)
            {
                Bleed(PerkManager.instance.perkDictionary["Hemorrhage"] * 3, this);
            }
            if (PerkManager.instance.perkDictionary["Plate Armor"] > 0)
            {
                adjDamage *= 1f - (.1f * PerkManager.instance.perkDictionary["Plate Armor"]);
            }
            if (PerkManager.instance.perkDictionary["Tearstone Pendant"] > 0 && health < maxHealth / 3)
            {
                adjDamage *= 1f - (.1f * PerkManager.instance.perkDictionary["Tearstone Pendant"]);
            }
        }

        if (!isPlayer && _source.isPlayer)
            _source.PlayerProcOnHitEffects(this, adjDamage);

        health -=adjDamage;
        if (health <= 0)
            Death(_source);

        spriteTinter.FlashColor(SpriteTint.DamageRed);
        UpdateHealthbar();
        CombatText.instance.ShowDamageText(_damage, (Vector2)transform.position + (Vector2)Random.onUnitSphere);
    }

    private void TakeBleedDamage(float _damage, Character _source)
    {
        health -= _damage;

        if (health <= 0)
            Death(_source);

        spriteTinter.FlashColor(SpriteTint.BloodRed);
        UpdateHealthbar();

        CombatText.instance.ShowDamageText(_damage, (Vector2)transform.position + (Vector2)Random.onUnitSphere, new Color32(120,0,0,255));
    }
    /// <summary>
    /// Restore health to this character.
    /// </summary>
    public void RestoreHealth(float _amount)
    {
        health = Mathf.Clamp(health + _amount, 0, maxHealth);
        spriteTinter.FlashColor(SpriteTint.HealGreen);

        UpdateHealthbar();
        GameSFX.instance.PlayHealSFX();

        CombatText.instance.ShowDamageText(_amount, (Vector2)transform.position + (Vector2)Random.onUnitSphere, Color.green);
    }

    /// <summary>
    /// Handle character death.
    /// Tells GameManager what died, and what killed it.
    /// </summary>
    public void Death(Character _killer)
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
    #endregion

    /// <summary>
    /// Updates animator movement variables.
    /// Attack animations are handled separately.
    /// </summary>
    public void UpdateAnimator(Vector2 _velocity)
    {
        animator.SetFloat("movementMagnitude", _velocity.normalized.magnitude);
        if (_velocity.normalized.magnitude > 0)
        {
            animator.SetFloat("movementY", _velocity.y);
            animator.SetFloat("movementX", _velocity.x);
            animator.SetFloat("idleY", _velocity.y);
            animator.SetFloat("idleX", _velocity.x);
        }
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

    /// <summary>
    /// Update character stats
    /// </summary>
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
        player.dashCooldown = dashCooldown;
        player.dashSpeed = dashSpeed;
    }

    private bool PercentageChance(float _percentage)
    {
        return Random.Range(0, 100) <= _percentage;
    }

    #region Bleed
    public void Bleed(int _amount, Character _source)
    {
        
        if(bleedDamage == 0)
        {
            bleedDamage += _amount;
            StartCoroutine(BleedRoutine(_source));
        }
        else
        {
            bleedDamage += _amount;
        }
    }

    private IEnumerator BleedRoutine(Character _source)
    {
        while(bleedDamage > 0)
        {
            bleedDamage--;
            TakeBleedDamage(1, _source);
            yield return new WaitForSeconds(0.33f);
        }
    }
    #endregion
    #region Slow
    public void Slow(float _duration, float _strength)
    {
        StopCoroutine(nameof(SlowRoutine));
        StartCoroutine(SlowRoutine(_duration, _strength));
        spriteTinter.DurationTint(SpriteTint.SlowLBlue, _duration);
    }

    private IEnumerator SlowRoutine(float _duration, float _strength)
    {
        float normalSpeed;

        if(isPlayer)
        {
            player.slowed = true;
            normalSpeed = player.currentSpeed;
            player.currentSpeed = normalSpeed - normalSpeed * (_strength / 100);
            yield return new WaitForSeconds(_duration);
            player.currentSpeed = normalSpeed;
            player.slowed = false;
        }
        else
        {
            ai.slowed = true;
            normalSpeed = ai.chaseSpeed;
            agent.speed = normalSpeed - normalSpeed * (_strength / 100);
            yield return new WaitForSeconds(_duration);
            agent.speed = normalSpeed;
            ai.slowed = false;
        }
    }
    #endregion

    public void PlayerProcOnHitEffects(Character _target, float _damage)
    {
        if (PerkManager.instance.perkDictionary["Serrated Blade"] > 0)
        {
            _target.Bleed(3 * PerkManager.instance.perkDictionary["Serrated Blade"], this);
        }
        if (PerkManager.instance.perkDictionary["Frost Relic"] > 0)
        {
            _target.Slow(2, 20f * PerkManager.instance.perkDictionary["Frost Relic"]);
        }
        if (PerkManager.instance.perkDictionary["Vampiric Blade"] > 0)
        {
            RestoreHealth(_damage * (PerkManager.instance.perkDictionary["Vampiric Blade"] * 0.1f));
        }
        if (PerkManager.instance.perkDictionary["Death Mark"] > 0)
        {
            if (PerkManager.instance.DeathMark(_target))
            {
                CameraShake.instance.Shake(0.25f, 0.5f);
                _target.Death(this);
            }
        }
    }
}