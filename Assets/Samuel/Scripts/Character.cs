using UnityEngine.UI;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Faction { Blue, Red};
    public Faction faction;

    public Slider healthBar;
    public Text healthText;

    private AIMovement aiMovement;
    private PlayerMovement playerMovement;
    private GameManager gameManager;

    public bool isPlayer;

    public float maxHealth;
    private float health;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        aiMovement = GetComponent<AIMovement>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        health = maxHealth;
        PlayerControlled(isPlayer);
    }

    private void Update()
    {
        if(isPlayer)
        {
            UpdateHealthBar();
        }
    }


    public void PlayerControlled(bool _controlled)
    {
        isPlayer = _controlled;
        playerMovement.enabled = _controlled;
        aiMovement.enabled = !_controlled;
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