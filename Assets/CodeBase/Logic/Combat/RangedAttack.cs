using CodeBase.Logic.Animations;
using CodeBase.Data;
using CodeBase.SO;
using UnityEngine;

namespace CodeBase.Logic.Combat
{
    public class RangedAttack : MonoBehaviour, IAttack, IStatsReceiver
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private LayerMask _targetsLayer;

        private float _cooldownTimer;
        private Stats _stats;

        private void Update()
        {
            UpdateCooldown();
        }

        public void TryAttack()
        {
            if (CooldownIsUp() && !_animator.IsAttacking)
            {
                _animator.PlayAttack();
                _cooldownTimer = _stats.AttackCooldown;
            }
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
            projectile.Initialize(_stats.Damage, direction, _targetsLayer);
        }

        public void Receive(Stats stats)
        {
            _stats = stats;
        }
    }
}