using System;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _animator;

        private const float MinimalDistance = 1;
        private Vector3 _target;

        public float StoppingDistance => _agent.stoppingDistance;

        public void Execute()
        {
            if (IsInitialized())
            {
                _agent.destination = _target;
                _animator.Move(_agent.velocity.magnitude);
            }
        }

        public void StopMove()
        {
            _agent.ResetPath();
            _animator.StopMoving();
            enabled = false;
        }

        public void SetDestination(Vector3 target)
        {
            _target = target;
        }

        private bool IsInitialized()
        {
            return _target != Vector3.zero;
        }
     
        private bool IsTargetNotReached()
        {
            return transform.position.SqrMagnitudeTo(_target) >= MinimalDistance;
        }    
    }
}