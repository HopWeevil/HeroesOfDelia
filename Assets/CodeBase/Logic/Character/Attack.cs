using CodeBase.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void InitializeStats(EnemyStaticData enemyStaticData);
    void TryAttack();
}

public abstract class Attack : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    protected float CooldownTimer;
    protected bool IsAttacking;

    protected abstract void PerformAttack();

    public void TryAttack()
    {
        if (CooldownTimer <= 0 && !IsAttacking)
        {
            PerformAttack();
            CooldownTimer = _cooldown;
        }
    }

    protected void UpdateCooldown()
    {
        if (CooldownTimer > 0)
        {
            CooldownTimer -= Time.deltaTime;
        }
    }
}