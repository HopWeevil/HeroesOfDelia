using System;
using CodeBase.Character;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private CharacterAnimator _animator;

        private float _current;

        private float _max;

        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public void TakeDamage(float damage)
        {
            Current -= damage;
            HealthChanged?.Invoke();
            if (Current > 0)
            {
                _animator.PlayHit();
            }

        }

    }
}