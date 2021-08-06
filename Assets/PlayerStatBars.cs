using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatBars : MonoBehaviour
{
    private float visualStamina, visualHealth, actualStamina, actualHealth;
    public float visualSmoothing = 8f;

    [Header("UI")]
    public Image healthBar;
    public Image staminaBar;
    public TMP_Text healthText;

    private void Start()
    {
        visualStamina = GameManager.instance.playerCharacter.maxStamina;
        visualHealth = GameManager.instance.playerCharacter.maxHealth;
    }

    private void Update()
    {
        if (!Mathf.Approximately(visualStamina, actualStamina))
        {
            visualStamina = Mathf.Lerp(visualStamina, actualStamina, Time.deltaTime * visualSmoothing);
            staminaBar.fillAmount = visualStamina;
        }

        if(!Mathf.Approximately(visualHealth, actualHealth))
        {
            visualHealth = Mathf.Lerp(visualHealth, actualHealth, Time.deltaTime * visualSmoothing);
            healthBar.fillAmount = visualHealth;
        }
    }

    public void SetHealth(float _health, float _maxHealth)
    {
        actualHealth = _health / _maxHealth;
        healthText.text = $"{(int)_health} | {(int)_maxHealth}";
    }

    public void SetStamina(float _stamina, float _maxStamina)
    {
        actualStamina = _stamina / _maxStamina;
    }

    /// <summary>
    /// Set color of healthbar
    /// </summary>
    public void SetHealthColor(Color _color)
    {
        healthBar.color = _color;
    }
}
