using CodeBase.Character;
using CodeBase.Enemy.StateMachine;
using CodeBase.UI;
using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(CharacterAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private GameObject _deathFx;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private EnemyStateMachine _stateMachine;
        [SerializeField] private HpBar _hpBar;
        [SerializeField] private float _destroyAfterDeathTime;
        public event Action Happened;

        private void Start()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (_health.Current <= 0)
            {
                Die();
            }          
        }

        private void Die()
        {
            _animator.PlayDeath();
            _boxCollider.enabled = false;
            _stateMachine.enabled = false;
            _hpBar.enabled = false;
            SpawnDeathFx();
            StartCoroutine(DestroyTimer());
      
            Happened?.Invoke();
        }

        private void SpawnDeathFx()
        {
            Instantiate(_deathFx, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_destroyAfterDeathTime);
            Destroy(gameObject);
        }
    }
}