using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    [SerializeField] private GameObject FishPrefab = null;
    [SerializeField] private GameObject Target = null;
    [SerializeField] private int MaxiumFish = 20;
    private int fishCount = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fishCount < MaxiumFish) {
            SpawnFish();
        }
    }

    private void SpawnFish() {
        Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), Random.Range(3, 10), Random.Range(-10, 10));
        var spawnedFish = Instantiate(FishPrefab, spawnPosition, Quaternion.identity, transform.transform);
        spawnedFish.GetComponent<Fish>().SetTarget(Target);
        fishCount++;
    }
}
