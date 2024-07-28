using CodeBase.Character;
using CodeBase.SO;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttack
{
    [SerializeField] private CharacterAnimator _animator;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private LayerMask _targetsLayer;

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
        Vector3 direction = transform.forward;
        Projectile projectile = Instantiate(_projectile, _projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        projectile.Initialize(_damage, direction, _targetsLayer);
    }
}