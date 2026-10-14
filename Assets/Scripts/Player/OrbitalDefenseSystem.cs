using System.Collections.Generic;
using UnityEngine;

public class OrbitalDefenseSystem : MonoBehaviour
{
    [SerializeField] private OrbitalProjectile projectilePrefab;
    [SerializeField] private int projectileCount = 3;
    [SerializeField] private float orbitRadius = 2.5f;
    [SerializeField] private float orbitSpeed = 150f;

    private Transform _orbitContainer;
    private readonly List<OrbitalProjectile> _spawnedProjectiles = new();

    private void Start()
    {
        _orbitContainer = new GameObject("OrbitContainer").transform;
        _orbitContainer.position = transform.position;

        SpawnProjectiles();
    }

    private void Update()
    {
        _orbitContainer.position = transform.position;
        _orbitContainer.Rotate(Vector3.up, orbitSpeed * Time.deltaTime);
    }

    private void SpawnProjectiles()
    {
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 spawnPosition = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * orbitRadius;

            OrbitalProjectile projectile = Instantiate(projectilePrefab, _orbitContainer);
            projectile.transform.localPosition = spawnPosition;
            _spawnedProjectiles.Add(projectile);
        }
    }
}