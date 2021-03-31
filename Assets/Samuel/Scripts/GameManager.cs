using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Character.Faction currentFaction;
    public Character currentCharacter;
    public CameraFollow cameraFollow;
    public Image healthBarFill;

    public void PlayerDeath(Character _killer)
    {
        currentCharacter = _killer;
        healthBarFill.color = currentFaction == Character.Faction.Blue ? Color.red : Color.blue;
        currentCharacter.PlayerControlled(true);
        currentFaction = currentCharacter.faction;
        cameraFollow.target = currentCharacter.transform;
        //TODO ui changes with faction
    }
    
}