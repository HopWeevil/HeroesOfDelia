using CodeBase.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Enemy.States
{
    public class AttackState : EnemyState
    {
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private float minattackDistance = 2f;
        [SerializeField] private EnemyAttack _enemyAttack;
        [SerializeField] private RotateToHero _rotateToHero;

        public override void Enter()
        {
            _enemyAttack.EnableAttack();
        }

        public override void Execute()
        {
            _enemyAttack.Execute();
            _rotateToHero.Execute();
        }

        public override void Exit()
        {
            _enemyAttack.DisableAttack();
        }

        public override bool ShouldTransit()
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);

            if (distance >= minattackDistance && distance <= attackDistance)
            {
                return true;
            }

            return false;

        }
    }
}