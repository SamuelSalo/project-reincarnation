using UnityEngine.UI;
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
    public enum Faction { Blue, Red};
    public Faction faction;

    public Slider healthBar;
    public Text healthText;

    private AIMovement aiMovement;
    private AICombat aiCombat;
    private NavMeshAgent agent;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private GameManager gameManager;
    private Rigidbody2D rb;

    public bool isPlayer;

    public float maxHealth;
    private float health;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        aiMovement = GetComponent<AIMovement>();
        agent = GetComponent<NavMeshAgent>();
        aiCombat = GetComponent<AICombat>();
        rb = GetComponent<Rigidbody2D>();

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        health = maxHealth;
        PlayerControlled(isPlayer);
    }

    public void PlayerControlled(bool _controlled)
    {
        isPlayer = _controlled;
        playerMovement.enabled = _controlled;
        playerCombat.enabled = _controlled;
        aiMovement.enabled = !_controlled;
        aiCombat.enabled = !_controlled;
        agent.enabled = !_controlled;
        rb.simulated = _controlled;
        
        if (_controlled)
            transform.tag = "Player";
        else
            transform.tag = "Enemy";

        UpdateHealthBar();
    }

    public void TakeDamage(float _damage, Character _source)
    {
        health -= _damage;

        if (health <= 0)
            Death(_source);

        if (isPlayer)
            UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthText.text = $"{health} / {maxHealth}";
    }

    public void DealDamage(float _damage, Character _target)
    {
        _target.TakeDamage(_damage, this);
    }

    private void Death(Character _killer)
    {
        if (isPlayer)
            gameManager.PlayerDeath(_killer);

        Destroy(gameObject);
        //TODO player death
    }
}