using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faction = Character.Faction;

public class Room : MonoBehaviour
{
    public bool enemiesRespawn;
    public Transform spawnPosition;
    public Faction faction;
    public List<Transform> npcs;
    public bool cleared => npcs.Count == 0;

    private void Start()
    {
        npcs = new List<Transform>();

        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("AI")) npcs.Add(t);
        }

        if(enemiesRespawn)
            InvokeRepeating(nameof(RespawnEnemies), 60f, 60f);
    }

    private void RespawnEnemies()
    {
        if(cleared && spawnPosition)
        {
            Debug.Log("Enemies respawned in " + gameObject.name);

            Transform tr = faction switch
            {
                Faction.Blue => (Transform)Instantiate(Resources.Load("Characters/NPC/NPCKnight"), spawnPosition),
                Faction.Red => (Transform)Instantiate(Resources.Load("Characters/NPC/NPCPyramidHead"), spawnPosition),
                _ => null,
            };
            npcs.Add(tr);
        }
    }
}
