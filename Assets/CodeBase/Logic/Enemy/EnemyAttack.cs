using CodeBase.Logic.Combat;
using UnityEngine;

namespace CodeBase.Enemy
{
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
