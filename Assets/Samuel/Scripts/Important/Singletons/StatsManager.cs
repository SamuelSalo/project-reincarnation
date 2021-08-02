using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    #region InputActions
    private PlayerControls playerControls;

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.Gameplay.Stats.performed += context => ToggleStatScreen();
        }

        playerControls.Enable();
    }
    
    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
    #region Singleton

    public static StatsManager instance;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Multiple instances of " + name + "found!");
            return;
        }
        instance = this;
    }

    #endregion

    private void Start()
    {
        UpdateStatUI();
    }

    private bool paused;
    public int XP;
    public int skillPoints;
    public int currentLevel;

    public int vitLevel;
    public int endLevel;
    public int dexLevel;
    public int strLevel;

    [Space]

    [HideInInspector] public int maxHPBonus;
    [HideInInspector] public float maxStaminaBonus;
    [HideInInspector] public float staminaRecoveryBonus;
    [HideInInspector] public float atkDamageBonus;
    [HideInInspector] public float atkRateReduction;
    [HideInInspector] public float dashSpeedBonus;
    [HideInInspector] public float dashCooldownReduction;
    [HideInInspector] public float atkRangeBonus;

    [Space]

    public Slider inGameXpBar;
    public Button lvlUpButton;
    public TMP_Text inGameSpText, inMenuSpText;
    public GameObject statsUI;
    public TMP_Text vitText, endText, dexText, strText;
    public TMP_Text inGameCurrentLvlText, inGameNextLvlText;
    public TMP_Text statSceenLvlText;

    public void IncreaseStat(string _statName)
    {
        if (skillPoints == 0) return;
         
        switch(_statName)
        {
            case "Vitality":
                vitLevel++;
                
                maxHPBonus += 5;
                break;

            case "Endurance":
                endLevel++;

                maxStaminaBonus += 10;
                staminaRecoveryBonus += 2;
                break;

            case "Dexterity":
                dexLevel++;

                dashSpeedBonus += 1f;
                dashCooldownReduction += 0.2f;
                atkRateReduction += 0.05f;
                atkRangeBonus += 0.1f;
                break;

            case "Strength":
                strLevel++;

                atkDamageBonus += 5f;
                break;
        }

        skillPoints--;
        UpdateStatUI();
        UpdateCharacterStats();
    }

    public void GiveXP(int _amount)
    {
        int adjAmount = _amount;

        if(PerkManager.instance.perkDictionary["Wise Man's Journal"] > 0)
        {
            adjAmount += 10 * PerkManager.instance.perkDictionary["Wise Man's Journal"];
        }

        XP += adjAmount;

        if (XP >= ((currentLevel + 1) * 100))
        {
            LevelUp();
        }

        UpdateStatUI();
        UpdateCharacterStats();
    }

    public void LevelUp()
    {
        Tooltip.instance.ShowTooltip("You gained a skill point! Press 'I' to level up!", 5f);

        skillPoints++;
        currentLevel++;
    }

    public void UpdateStatUI()
    {
        inGameXpBar.minValue = currentLevel * 100;
        inGameXpBar.maxValue = (currentLevel + 1) * 100;
        inGameXpBar.value = XP;
        inGameSpText.text = "Skill Points: " + skillPoints;
        inMenuSpText.text = "Skill Points: " + skillPoints;

        inGameSpText.color = skillPoints > 0 ? Color.yellow : Color.white;
        inMenuSpText.color = skillPoints > 0 ? Color.yellow : Color.white;

        vitText.text = "Vitality: " + vitLevel;
        endText.text = "Endurance: " + endLevel;
        strText.text = "Strength: " + strLevel;
        dexText.text = "Dexterity: " + dexLevel;

        statSceenLvlText.text = "Character Level: " + currentLevel;
        inGameCurrentLvlText.text = currentLevel.ToString();
        inGameNextLvlText.text = (currentLevel + 1).ToString();

        lvlUpButton.gameObject.SetActive(skillPoints > 0);
    }

    private void ToggleStatScreen()
    {
        if (!paused && !GameManager.instance.canPause) return;

        paused = !paused;
        statsUI.SetActive(!statsUI.activeSelf);
        GameManager.instance.gameUI.SetActive(!GameManager.instance.gameUI.activeSelf);
        Time.timeScale = statsUI.activeSelf ? 0f : 1f;
        GameManager.instance.canPause = !GameManager.instance.canPause;

        UpdateStatUI();
    }

    public void UpdateCharacterStats()
    {
        var characterStats = GameManager.instance.playerCharacter.characterStats;
        var character = GameManager.instance.playerCharacter;

        character.damage = characterStats.damage + atkDamageBonus;
        character.maxHealth = characterStats.maxHealth + maxHPBonus;
        character.health = character.maxHealth;
        character.maxStamina = characterStats.maxStamina + maxStaminaBonus;
        character.staminaRecovery = characterStats.staminaRecovery + staminaRecoveryBonus;
        character.attackRate = characterStats.attackRate - atkRateReduction;
        character.attackRange = characterStats.attackRange + atkRangeBonus;
        character.dashSpeed = characterStats.dashSpeed + dashSpeedBonus;
        character.UpdateHealthbar();
    }
}