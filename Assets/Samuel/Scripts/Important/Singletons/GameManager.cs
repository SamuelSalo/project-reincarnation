using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Character.Faction playerFaction;
    [HideInInspector] public Character playerCharacter;
    public Room currentRoom;

    public bool canPause;
    public CameraFollow cameraFollow;
    public LightFollow lightFollow;

    [Space]
    [Header("UI")]
    public PlayerStatBars statBars;
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

    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }

        instance = this;
    }
    #endregion

    private void FixedUpdate()
    {
        UpdatePermaDeath();

        if(playerCharacter)
            UpdateUI();
    }

    /// <summary>
    /// Switch character on player death.
    /// Update UI accordingly.
    /// </summary>
    public void PlayerDeath(Character _killer)
    {
        if (_killer.isBoss || permaDeath || _killer == playerCharacter)  
        {
            PermanentDeath();
            return;
        }

        lives--;

        SetPlayer(_killer);
        playerCharacter.PlayerControlled(true);
        playerCharacter.transform.SetParent(null);
        _killer.UpdateHealthbar();

        if(_killer.room && _killer.room.npcs.Contains(_killer.transform))
            _killer.room.npcs.Remove(_killer.transform);
    }
    
    /// <summary>
    /// Overload for instant player death without source
    /// </summary>
    public void PlayerDeath()
    {
        PermanentDeath();
    }
    /// <summary>
    /// Update list of enemies when player character is swapped.
    /// Then set that updated list's target as the player.
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

        StatsManager.instance.GiveXP(50);
        InventoryManager.instance.GiveTokens(25);
        StatsManager.instance.UpdateCharacterStats();
    }

    /// <summary>
    /// Handle permanent death.
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
        statBars.SetHealth(playerCharacter.health, playerCharacter.maxHealth);
        statBars.SetStamina(playerCharacter.player.stamina, playerCharacter.maxStamina);
    }

    /// <summary>
    /// Sets player character as param
    /// Sets other stuff accordingly
    /// </summary>
    public void SetPlayer(Character _playerCharacter)
    {
        playerCharacter = _playerCharacter;
        playerFaction = playerCharacter.characterStats.faction;
        statBars.SetHealthColor(playerFaction == Character.Faction.Red ? Color.red : Color.blue);
        cameraFollow.target = playerCharacter.transform;
        lightFollow.target = playerCharacter.transform;

        UpdateAIs();
        StatsManager.instance.UpdateCharacterStats();
    }
}