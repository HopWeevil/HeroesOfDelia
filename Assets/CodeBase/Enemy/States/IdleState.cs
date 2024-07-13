using CodeBase.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Enemy.States
{
    public class IdleState : EnemyState
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float patrolDistance;
        public float _distance = 3f;
        [SerializeField] private float minDistance = 2f;

        public override void Enter()
        {
            Debug.Log("Enter idle");
        }

        public override void Execute()
        {
            Debug.Log("Now in idle");
        }

        public override void Exit()
        {
        }

        public override bool ShouldTransit()
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);

            if (distance >= minDistance && distance <= _distance)
            {
                return true;
            }

            return false;

            /* Transit = Vector3.Distance(transform.position, _target.transform.position) <= _distance;
             return Vector3.Distance(transform.position, _target.transform.position) <= _distance;*/
        }
    }
}