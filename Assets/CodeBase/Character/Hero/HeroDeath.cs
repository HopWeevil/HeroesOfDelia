using CodeBase.Character;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth _health;
        [SerializeField] private HeroMover _mover;
        [SerializeField] private HeroAttack _attack;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private GameObject DeathFx;

        private bool _isDead;

        private void Start()
        {
            _health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (!_isDead && _health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            _mover.enabled = false;
            _attack.enabled = false;
            _animator.PlayDeath();

            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }
    }
}