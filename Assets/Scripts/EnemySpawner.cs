using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Complete;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float repeatRateMin = 10f;
    public float repeatRateMax = 10f;
    public int spawnMin = 3;
    public int spawnMax = 5;
    public float spawnRadius = 8f;
    public float spawnRadius2 = 3f;
    public bool usePlayerPosition = true;
    public float staggerDuration = 0.1f;
    public int maxEnemiesCount = 20;
    public UnityEvent onFinished;

    private int spawnedCount = 0;
    private int killedCount = 0;

    void OnEnable() {
        Invoke("SpawnEnemies", getRepeatDuration());
    }

    void SpawnEnemies() {
        StartCoroutine(spawn());
    }
    private IEnumerator spawn() {
        int _spawnCount = Random.Range(spawnMin, spawnMax);
        int _count = 0;

        Vector3 origin = usePlayerPosition ? GameObject.FindWithTag("Player").transform.position : transform.position;
        Vector3 randomPosition = GetRandomNavMeshPosition(origin, spawnRadius);

        while (_count < _spawnCount && spawnedCount < maxEnemiesCount) {
            spawnedCount += 1;
            _count += 1;

            GameObject enemy = Instantiate(enemyPrefab, GetRandomNavMeshPosition(randomPosition, spawnRadius2), Quaternion.identity);
            enemy.GetComponent<TankHealth>().onKilled += onEnemyDestroyed;
            yield return new WaitForSeconds(staggerDuration);
        }

        if (spawnedCount < maxEnemiesCount) {
            Invoke("SpawnEnemies", getRepeatDuration());
        }
    }
    private float getRepeatDuration() {
        return Random.Range(repeatRateMin, repeatRateMax);
    }
    void onEnemyDestroyed(TankHealth health) {
        killedCount += 1;
        if (killedCount >= maxEnemiesCount) {
            CancelInvoke("SpawnEnemies");
            onFinished.Invoke();
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 position, float radius) {
        Vector3 randomOffset = Utils.getRandomOffsetInRadius(position, radius);

        if (NavMesh.SamplePosition(randomOffset, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
            return hit.position;
        }
        else {
            return randomOffset;
        }
    }
}
