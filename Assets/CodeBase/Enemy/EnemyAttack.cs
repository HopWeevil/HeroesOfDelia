using System.Linq;
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

        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];
        private bool _attackIsActive;
        private float _attackTimer;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        public void Execute()
        {
            UpdateCooldown();
            if (CanAttack())
            {
                StartAttack();
            }
        }

        private void StartAttack()
        {
            _isAttacking = true;
            _animator.PlayAttack();
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(CalculateStartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitCount > 0;
        }

        private Vector3 CalculateStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;
        }

        private void OnAttack()
        {
            if (Hit(out Collider collider))
            {
                Debug.Log(collider.name);
                PhysicsDebug.DrawDebug(CalculateStartPoint(), Cleavage, 1f);
                //collider.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _attackTimer = AttackCooldown;
        }

        public void EnableAttack()
        {
             _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _isAttacking = false;
            _attackIsActive = false;
        }

        private bool CanAttack()
        {
            return _attackIsActive && !_isAttacking && CooldownIsUp();
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
            {
                _attackTimer -= Time.deltaTime;
            }
        }

        private bool CooldownIsUp()
        {
            return _attackTimer <= 0;
        }
    }
}