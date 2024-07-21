using CodeBase.Character;
using CodeBase.Logic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    [SerializeField] private CharacterAnimator _animator;
    [SerializeField] private LayerMask _targetsLayer;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _cleavage = 0.5f;
    [SerializeField] private float _effectiveDistance = 1f;
    [SerializeField] private float _damage = 10f;

    private float _cooldownTimer;

    private void Update()
    {
        UpdateCooldown();
    }

    public void TryAttack()
    {
        if (CooldownIsUp() && !_animator.IsAttacking)
        {
            _animator.PlayAttack();
            _cooldownTimer = _attackCooldown;
        }
    }

    public void InitializeStats(EnemyStaticData enemyStaticData)
    {
        _damage = enemyStaticData.Damage;
        _cleavage = enemyStaticData.Cleavage;
        _effectiveDistance = enemyStaticData.EffectiveDistance;
        _attackCooldown = enemyStaticData.AttackCooldown;
    }

    private void UpdateCooldown()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private bool CooldownIsUp()
    {
        return _cooldownTimer <= 0;
    }

    private void OnAttack()
    {
        if (TryGetTargets(out Collider[] hits))
        {
            foreach (Collider hit in hits)
            {
                hit.transform.GetComponentInChildren<IHealth>()?.TakeDamage(_damage);
            }
        }
    }

    private bool TryGetTargets(out Collider[] targets)
    {
        targets = Physics.OverlapSphere(CalculateStartPoint(), _cleavage, _targetsLayer);
        return targets.Length > 0;
    }

    private Vector3 CalculateStartPoint()
    {
        return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * _effectiveDistance;
    }
}