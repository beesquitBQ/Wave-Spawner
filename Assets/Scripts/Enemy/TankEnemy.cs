using UnityEngine;

public class TankEnemy : EnemyBase
{
    [SerializeField] private float armorDamageReduction = 5f;

    public override void TakeDamage(float amount)
    {
        float reducedAmount = Mathf.Max(1f, amount - armorDamageReduction);
        base.TakeDamage(reducedAmount);
    }
}