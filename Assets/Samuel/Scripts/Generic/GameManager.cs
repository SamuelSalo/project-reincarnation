using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Character.Faction currentFaction;
    public Character currentCharacter;
    public CameraFollow cameraFollow;
    public Image healthBarFill;

    private List<AIMovement> enemyAIs;

    private void Start()
    {
        UpdateEnemies();
    }

    /// <summary>
    /// Switch character on player death.
    /// Update UI accordingly.
    /// </summary>
    public void PlayerDeath(Character _killer)
    {
        currentCharacter = _killer;
        healthBarFill.color = currentFaction == Character.Faction.Blue ? Color.red : Color.blue;
        currentCharacter.PlayerControlled(true);
        currentFaction = currentCharacter.faction;
        cameraFollow.target = currentCharacter.transform;
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

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
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
}