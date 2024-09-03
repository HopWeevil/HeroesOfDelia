using CodeBase.Data;
using CodeBase.Logic.Animations;
using UnityEngine;

namespace CodeBase.Logic.Combat
{
    public class MeleeAttack : MonoBehaviour, IAttack, IStatsReceiver
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private LayerMask _targetsLayer;

        private Stats _stats;

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
            if (TryGetTargets(out Collider[] hits))
            {
                foreach (Collider hit in hits)
                {
                    hit.transform.GetComponentInChildren<IHealth>()?.TakeDamage(_stats.Damage);
                }
            }
        }

        private bool TryGetTargets(out Collider[] targets)
        {
            targets = Physics.OverlapSphere(CalculateStartPoint(), _stats.AttackSplash, _targetsLayer);
            return targets.Length > 0;
        }

        private Vector3 CalculateStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * _stats.AttackDistance;
        }

        public void Receive(Stats stats)
        {
            _stats = stats;
        }
    }
}