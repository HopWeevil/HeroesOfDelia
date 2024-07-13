using UnityEngine;

namespace CodeBase.Enemy
{

    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;
        public float AttackCooldown = 2f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

        private Transform _heroTransform;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;
        public float _attackTimer;


        private void Start()
        {
            _attackTimer = AttackCooldown;
        }

        public void Execute()
        {
            _attackTimer += Time.deltaTime;

            if (_attackIsActive && !_isAttacking && _attackTimer >= AttackCooldown)
            {
                StartAttack();
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;
            _animator.StartPlayAttack();
            _attackTimer = 0f;
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _animator.StopPlayAttack();
        }

        public void EnableAttack()
        {
             _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _isAttacking = false;
            _attackIsActive = false;
            _animator.StopPlayAttack();
        }
    }
}