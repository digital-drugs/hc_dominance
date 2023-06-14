using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcs;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform _spawnAroundPoint;

    [Range(1, 6)][SerializeField] private int warriorCount = 5;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 spawnAroundPoint = _spawnAroundPoint.position;

        for (int i = 0; i < warriorCount; i++)
        {
            GameObject npc = Instantiate(npcPrefab, new Vector3(Random.Range(spawnAroundPoint.x + 50, spawnAroundPoint.x - 50), spawnAroundPoint.y, Random.Range(spawnAroundPoint.z + 10, spawnAroundPoint.z - 10)), Quaternion.identity);
            npcs.Add(npc);
            npc = null;
        }
    }
}