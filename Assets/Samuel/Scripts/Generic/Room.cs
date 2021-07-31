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

            GameObject enemy = faction switch
            {
                Faction.Blue => Instantiate(Resources.Load("Characters/NPC/NPCKnight"), spawnPosition.position, Quaternion.identity, transform) as GameObject,
                Faction.Red => Instantiate(Resources.Load("Characters/NPC/NPCPyramidHead"), spawnPosition.position, Quaternion.identity, transform) as GameObject,
                _ => null,
            };
            npcs.Add(enemy.transform);
            StartCoroutine(InitializeSpawnedAI(enemy.GetComponent<AI>()));
        }
    }

    private IEnumerator InitializeSpawnedAI(AI _enemyAI)
    {
        yield return new WaitForSeconds(0.1f);
        _enemyAI.InitializeAI();
        GameManager.instance.UpdateAIs();
    }
}
