using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Character.Faction playerFaction;
    [HideInInspector] public Character playerCharacter;
    [HideInInspector] public StatsManager statsManager;
    [HideInInspector] public InventoryManager invManager;

    public bool canPause;
    public CameraFollow cameraFollow;
    public LightFollow lightFollow;

    [Space] [Header("UI")]
    public Slider healthBar;
    public TMP_Text healthText;
    public Image healthBarFill;
    public Slider staminaBar;
    public TMP_Text permaDeathText;
    public Slider permaDeathBar;
    public TMP_Text livesText;
    [Space]
    public GameObject deathUI;
    public GameObject gameUI;
    private List<AI> enemyAIs;

    [Space] [Header("PermaDeath")]
    public bool permaDeath;
    public float permaDeathTimer = 300f;
    public int lives = 3;

    private void FixedUpdate()
    {
        UpdatePermaDeath();

        if(playerCharacter)
            UpdateUI();
    }

    private void Start()
    {
        statsManager = GetComponent<StatsManager>();
        invManager = GetComponent<InventoryManager>();
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

        SetPlayer(_killer);
        playerCharacter.PlayerControlled(true);
        playerCharacter.transform.SetParent(null);

        if(_killer.room && _killer.room.npcs.Contains(_killer.transform))
            _killer.room.npcs.Remove(_killer.transform);
    }
    
    /// <summary>
    /// Update list of enemies when player character is swapped.
    /// Then set that updated list's target as the player.
    /// TODO: faction differences
    /// </summary>
    public void UpdateAIs()
    {
        enemyAIs = new List<AI>();

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("AI"))
        {
            enemyAIs.Add(g.GetComponent<AI>());
        }
        foreach(AI ai in enemyAIs)
        {
            ai.target = playerCharacter.transform;
        }  
    }
    /// <summary>
    /// Player killed someone?
    /// Call this to update list of enemies to avoid nullreference errors.
    /// </summary>
    public void PlayerKill(Character character)
    {
        UpdateAIs();

        if (statsManager == null)
            statsManager = GetComponent<StatsManager>();

        statsManager.GiveXP(50);
        invManager.GiveGold(25);
        statsManager.UpdateCharacterStats();
    }

    /// <summary>
    /// TODO: permadeath
    /// </summary>
    private void PermanentDeath()
    {
        deathUI.SetActive(true);
        gameUI.SetActive(false);
        canPause = false;
        Time.timeScale = 0f;
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
    private void UpdateUI()
    {
        healthBar.maxValue = playerCharacter.maxHealth;
        healthBar.value = playerCharacter.health;
        healthText.text = $"{playerCharacter.health} / {playerCharacter.maxHealth}";
        staminaBar.maxValue = playerCharacter.maxStamina;
        staminaBar.value = playerCharacter.player.stamina;
    }

    /// <summary>
    /// Sets player character as param
    /// Sets other stuff accordingly
    /// </summary>
    public void SetPlayer(Character _playerCharacter)
    {
        playerCharacter = _playerCharacter;
        playerFaction = playerCharacter.faction;
        healthBarFill.color = playerFaction == Character.Faction.Red ? Color.red : Color.blue;
        cameraFollow.target = playerCharacter.transform;
        lightFollow.target = playerCharacter.transform;

        UpdateAIs();
        statsManager.UpdateCharacterStats();
    }
}