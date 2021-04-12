using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Character.Faction currentFaction;
    public Character currentCharacter;
    public CameraFollow cameraFollow;
    public LightFollow lightFollow;

    [Space] [Header("UI")]
    public Slider healthBar;
    public TMP_Text healthText;
    public Image healthBarFill;
    public TMP_Text permaDeathText;
    public Slider permaDeathBar;
    public TMP_Text livesText;

    private List<AIMovement> enemyAIs;

    [Space] [Header("PermaDeath")]
    public bool permaDeath;
    public float permaDeathTimer = 300f;
    public int lives = 3;

    private void Start()
    {
        UpdateEnemies();
    }

    private void FixedUpdate()
    {
        UpdatePermaDeath();
        UpdateHealthBar();
    }

    /// <summary>
    /// Switch character on player death.
    /// Update UI accordingly.
    /// </summary>
    public void PlayerDeath(Character _killer)
    {
        lives--;

        if (_killer.isBoss || permaDeath)  
        {
            PermanentDeath();
            return;
        }

        currentCharacter = _killer;
        currentFaction = currentCharacter.faction;
        currentCharacter.PlayerControlled(true);
        currentCharacter.transform.SetParent(null);

        healthBarFill.color = currentFaction == Character.Faction.Red ? Color.red : Color.blue;
        _killer.UpdateHealthbar();

        cameraFollow.target = currentCharacter.transform;
        lightFollow.target = currentCharacter.transform;
        
        UpdateEnemies();
    }
    
    /// <summary>
    /// Update list of enemies when player character is swapped.
    /// Then set that updated list's target as the player.
    /// TODO: faction differences
    /// </summary>
    private void UpdateEnemies()
    {
        enemyAIs = new List<AIMovement>();

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("AI"))
        {
            enemyAIs.Add(g.GetComponent<AIMovement>());
        }
        foreach(AIMovement ai in enemyAIs)
        {
            ai.target = currentCharacter.transform;
        }  
    }
    /// <summary>
    /// Player killed someone?
    /// Call this to update list of enemies to avoid nullreference errors.
    /// </summary>
    public void PlayerKill(Character character)
    {
        UpdateEnemies();
    }

    /// <summary>
    /// TODO: permadeath
    /// </summary>
    private void PermanentDeath()
    {
        Debug.Log("Permanent death!");
    }

    /// <summary>
    /// Updates permadeath timers etc.
    /// </summary>
    private void UpdatePermaDeath()
    {
        permaDeathTimer -= Time.fixedDeltaTime;
        permaDeath = lives < 0 || permaDeathTimer <= 0;

        var ts = System.TimeSpan.FromSeconds(permaDeathTimer);
        permaDeathText.text = permaDeath ? "PERMADEATH" : string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        permaDeathText.color = permaDeath ? Color.red : Color.white;

        permaDeathBar.gameObject.SetActive(!permaDeath);
        permaDeathBar.value = permaDeathTimer;

        livesText.gameObject.SetActive(!permaDeath);
        livesText.text = "Lives: " + lives;
    }

    /// <summary>
    /// Update healthbar visuals
    /// </summary>
    private void UpdateHealthBar()
    {
        healthBar.maxValue = currentCharacter.maxHealth;
        healthBar.value = currentCharacter.health;
        healthText.text = $"{currentCharacter.health} / {currentCharacter.maxHealth}";
    }
}