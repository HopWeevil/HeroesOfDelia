using UnityEngine;


namespace CodeBase.Enemy.States
{
    public class AttackState : EnemyState
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minDistance;
        [SerializeField] private EnemyAttack _enemyAttack;
        [SerializeField] private RotateToHero _rotateToHero;

        public override void Enter()
        {
            _enemyAttack.EnableAttack();
            _rotateToHero.EnableRotate();
        }

        public override void Exit()
        {
            _enemyAttack.DisableAttack();
            _rotateToHero.DisableRotate();
        }

        public override bool ShouldTransit()
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);

            if (distance >= _minDistance && distance <= _maxDistance)
            {
                return true;
            }

            return false;
        }
    }
}