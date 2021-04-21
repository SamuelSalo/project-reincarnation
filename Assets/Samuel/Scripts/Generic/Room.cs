using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faction = Character.Faction;

public class Room : MonoBehaviour
{
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
    }

    public void UpdateRoom()
    {
        npcs.Clear();

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("AI")) npcs.Add(t);
        }
    }
}
