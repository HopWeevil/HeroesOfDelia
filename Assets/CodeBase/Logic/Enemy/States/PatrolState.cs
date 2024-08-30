using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Enemy.States
{
    public class PatrolState : EnemyState
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minDistance;
        [SerializeField] private EnemyPatrol _patrol;

        public override void Enter()
        {
            _patrol.StartPatrol();
        }

        public override void Exit()
        {
            _patrol.StopPatrol();
        }

        public override bool ShouldTransit()
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);
            return distance >= _minDistance && distance <= _maxDistance;
        }
    }
}
