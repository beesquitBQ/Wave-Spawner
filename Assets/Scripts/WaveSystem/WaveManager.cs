using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action<int, int> OnWaveStarted;
    public event Action<int> OnEnemiesCountChanged;
    public event Action OnAllWavesCompleted;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnemyPoolService poolService;
    [SerializeField] private NavMeshSpawnService spawnService;
    [SerializeField] private List<WaveConfig> waves;

    private int _currentWaveIndex = -1;
    private int _aliveEnemiesCount;
    private bool _isSpawning;

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        _currentWaveIndex++;

        if (_currentWaveIndex >= waves.Count)
        {
            OnAllWavesCompleted?.Invoke();
            return;
        }

        StartCoroutine(WaveRoutine(waves[_currentWaveIndex]));
    }

    private IEnumerator WaveRoutine(WaveConfig wave)
    {
        _isSpawning = true;
        _aliveEnemiesCount = 0;

        OnWaveStarted?.Invoke(_currentWaveIndex + 1, wave.EnemyCount);
        OnEnemiesCountChanged?.Invoke(_aliveEnemiesCount);

        for (int i = 0; i < wave.EnemyCount; i++)
        {
            SpawnEnemy(wave);
            _aliveEnemiesCount++;
            OnEnemiesCountChanged?.Invoke(_aliveEnemiesCount);

            yield return new WaitForSeconds(wave.SpawnInterval);
        }

        _isSpawning = false;
    }

    private void SpawnEnemy(WaveConfig wave)
    {
        if (wave.EnemyPrefabs.Count == 0) return;

        if (spawnService.TryGetSpawnPosition(playerTransform.position, out Vector3 spawnPosition))
        {
            EnemyBase prefab = wave.EnemyPrefabs[UnityEngine.Random.Range(0, wave.EnemyPrefabs.Count)];
            EnemyBase enemyInstance = poolService.Get(prefab);
            
            enemyInstance.OnDied += HandleEnemyDied;
            enemyInstance.Spawn(spawnPosition, playerTransform);
        }
    }

    private void HandleEnemyDied(EnemyBase enemy)
    {
        enemy.OnDied -= HandleEnemyDied;
        poolService.Release(enemy);

        _aliveEnemiesCount--;
        OnEnemiesCountChanged?.Invoke(_aliveEnemiesCount);

        if (!_isSpawning && _aliveEnemiesCount <= 0)
        {
            StartCoroutine(WaitAndStartNextWave(waves[_currentWaveIndex].DelayBeforeNextWave));
        }
    }

    private IEnumerator WaitAndStartNextWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextWave();
    }
}