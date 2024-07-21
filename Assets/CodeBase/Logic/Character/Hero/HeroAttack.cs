using CodeBase.Character;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private LayerMask _targetsLayer;
        [SerializeField] private CharacterAnimator _animator;
        //[SerializeField] private CharacterController _characterController;
        [SerializeField] private float _attackCooldown = 2f;

        private IInputService _inputService;

        private float _cooldownTimer;
        private Stats _stats;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Update()
        {
            UpdateCooldown();

            if (_inputService.IsAttackButtonUp() && !_animator.IsAttacking && CooldownIsUp())
            {
                Execute();
            }
        }

        private void UpdateCooldown()
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }

        public void Execute()
        {
            _animator.PlayAttack();
            _cooldownTimer = _attackCooldown;
        }

        private bool CooldownIsUp()
        {
            return _cooldownTimer <= 0;
        }

        private void OnAttack()
        {
            if(TryGetTargets(out Collider[] hits))
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
                }
            }       
        }

        private bool TryGetTargets(out Collider[] targets)
        {
            targets = Physics.OverlapSphere(CalculateStartPoint() + transform.forward, _stats.DamageRadius, _targetsLayer);
            return targets.Length > 0;
        }

        private Vector3 CalculateStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * 1;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }
    }
}