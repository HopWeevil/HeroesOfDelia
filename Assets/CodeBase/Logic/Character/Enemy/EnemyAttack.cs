using CodeBase.Character;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        private IAttack _attack;
        private bool _attackIsActive;

        private void Awake()
        {
            _attack = GetComponent<IAttack>();
        }

        public void Execute()
        {
            if (_attackIsActive)
            {
                _attack.TryAttack();
            }
        }

        public void EnableAttack()
        {
            _attackIsActive = true;
        }

        public void DisableAttack()
        {
            _attackIsActive = false;
        }
    }
}
