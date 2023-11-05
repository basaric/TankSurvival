using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float repeatRate = 10f;
    public int spawnMin = 3;
    public int spawnMax = 5;
    public float spawnRadius = 8f;
    public bool usePlayerPosition = true;

    void Start() {
        Invoke("SpawnEnemies", repeatRate);
    }

    void SpawnEnemies() {
        for (int i = 0; i < Random.Range(spawnMin, spawnMax); i++) {
            Vector3 randomPosition = GetRandomNavMeshPosition();
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
        Invoke("SpawnEnemies", repeatRate);
    }

    Vector3 GetRandomNavMeshPosition() {
        Vector3 origin = usePlayerPosition ? GameObject.FindWithTag("Player").transform.position : transform.position;

        if (NavMesh.SamplePosition(Utils.getRandomOffsetInRadius(origin, spawnRadius), out NavMeshHit hit, 10.0f, NavMesh.AllAreas)) {
            return hit.position;
        }
        else {
            return origin;
        }
    }
}
