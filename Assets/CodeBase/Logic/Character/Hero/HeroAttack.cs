using CodeBase.Character;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private CharacterController _characterController;

        private IInputService _inputService;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        public Stats _stats;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonUp() && !_animator.IsAttacking)
            {
                _animator.PlayAttack();
            }
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); ++i)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        private int Hit()
        {
            return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);
        }

        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }
    }
}