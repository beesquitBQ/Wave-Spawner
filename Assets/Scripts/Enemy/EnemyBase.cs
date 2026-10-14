using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public event Action<EnemyBase> OnDied;

    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float moveSpeed = 3.5f;

    protected float currentHealth;
    protected Transform target;
    protected NavMeshAgent agent;

    public EnemyBase PrefabOrigin { get; set; }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public virtual void Spawn(Vector3 position, Transform targetTransform)
    {
        gameObject.SetActive(true);

        if (!agent.enabled)
        {
            agent.enabled = true;
        }

        agent.Warp(position);
        agent.isStopped = false;
        agent.ResetPath();

        target = targetTransform;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        if (target != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        agent.enabled = false;
        OnDied?.Invoke(this);
    }
}