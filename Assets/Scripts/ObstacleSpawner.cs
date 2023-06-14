using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> Dummies { get; private set; } = new List<GameObject>();
    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private Transform _spawnAroundPoint;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnAroundPoint = _spawnAroundPoint.position;
        for (int i = 0; i < 6; i++)
        {
            GameObject dummy = Instantiate(dummyPrefab, new Vector3(Random.Range(spawnAroundPoint.x +70, spawnAroundPoint.x - 70),spawnAroundPoint.y, Random.Range(spawnAroundPoint.z + 70, spawnAroundPoint.z - 70)), Quaternion.identity);
           Dummies.Add(dummy);
            dummy = null;
        }
    }
}
