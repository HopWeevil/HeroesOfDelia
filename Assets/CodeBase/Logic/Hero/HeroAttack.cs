using CodeBase.Logic.Animations;
using CodeBase.Services.Input;
using CodeBase.Logic.Combat;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;

        private IAttack _attack;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Awake()
        {
            _attack = GetComponent<IAttack>();
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonUp())
            {
                _attack.TryAttack();
            }
        }
    }
}