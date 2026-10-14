using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawnService : MonoBehaviour
{
    [SerializeField] private float minSpawnDistance = 8f;
    [SerializeField] private float maxSpawnDistance = 20f;
    [SerializeField] private int maxSampleAttempts = 30;

    public bool TryGetSpawnPosition(Vector3 center, out Vector3 spawnPosition)
    {
        for (int i = 0; i < maxSampleAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 randomPoint = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }
}