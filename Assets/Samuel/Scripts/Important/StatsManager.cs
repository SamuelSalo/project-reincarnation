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

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
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

    public int maxHPBonus;
    public float maxStaminaBonus;
    public float staminaRecoveryBonus;
    public float atkDamageBonus;
    public float atkRateReduction;
    public float dashSpeedBonus;
    public float dashCooldownReduction;

    [Space]

    public int itemMaxHpBonus;

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
        XP += _amount;

        if (XP >= ((currentLevel + 1) * 100))
        {
            LevelUp();
        }

        UpdateStatUI();
        UpdateCharacterStats();
    }

    public void LevelUp()
    {
        GameObject.FindWithTag("Tooltip").GetComponent<Tooltip>().ShowTooltip("You gained a skill point! Press 'I' to level up!", 5f);

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
        if (!paused && !gameManager.canPause) return;

        paused = !paused;
        statsUI.SetActive(!statsUI.activeSelf);
        gameManager.gameUI.SetActive(!gameManager.gameUI.activeSelf);
        Time.timeScale = statsUI.activeSelf ? 0f : 1f;
        gameManager.canPause = !gameManager.canPause;

        UpdateStatUI();
    }

    public void UpdateCharacterStats()
    {
        var characterStats = gameManager.playerCharacter.characterStats;
        var character = gameManager.playerCharacter;

        character.damage = characterStats.damage + atkDamageBonus;
        character.maxHealth = characterStats.maxHealth + maxHPBonus + itemMaxHpBonus;
        character.maxStamina = characterStats.maxStamina + maxStaminaBonus;
        character.staminaRecovery = characterStats.staminaRecovery + staminaRecoveryBonus;
        character.attackRate = characterStats.attackRate - atkRateReduction;
        character.dashSpeed = characterStats.dashSpeed + dashSpeedBonus;
        character.UpdateHealthbar();
    }
}