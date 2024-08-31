using CodeBase.Character;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        private IAttack _attack;

        private void Awake()
        {
            _attack = GetComponent<IAttack>();
        }

        public void TryAttack()
        {
            if (_attack != null)
            {
                _attack.TryAttack();
            }
        }
    }
}
