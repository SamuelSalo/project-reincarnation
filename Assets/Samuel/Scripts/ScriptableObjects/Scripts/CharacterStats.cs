using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterSheet", menuName = "ScriptableObject/New CharacterSheet")]
public class CharacterStats : ScriptableObject
{
    public Character.Faction faction;
    public AIVariables aiVariables;
    public float maxHealth;
    public float maxStamina;
    public float staminaRecovery;

    [Space]

    public float damage;
    public float attackRate;
    public float attackRange;
    public float dashCooldown;
    public float dashSpeed;

    [Space]

    public float movementSpeed;
    public float moveSmoothing;
    public float turnSmoothing;
}
