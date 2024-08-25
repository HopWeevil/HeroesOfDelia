using CodeBase.Character;
using CodeBase.Data;
using CodeBase.Logic;
using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth, IStatsReceiver
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

        public float GetCurrent()
        {
            return _current;
        }

        public float GetMax()
        {
            return _max;
        }

        public void Receive(Stats stats)
        {
            _current = stats.Hp;
            _max = stats.Hp;
        }
    }
}
