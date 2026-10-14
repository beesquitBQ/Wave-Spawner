using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveConfig
{
    [SerializeField] private List<EnemyBase> enemyPrefabs;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float delayBeforeNextWave = 4f;

    public List<EnemyBase> EnemyPrefabs => enemyPrefabs;
    public int EnemyCount => enemyCount;
    public float SpawnInterval => spawnInterval;
    public float DelayBeforeNextWave => delayBeforeNextWave;
}