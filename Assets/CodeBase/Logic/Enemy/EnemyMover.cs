using System;
using CodeBase.Character;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class EnemyMover : MonoBehaviour, IStatsReceiver
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CharacterAnimator _animator;

        private const float MinimalDistance = 1;
        private Vector3 _target;

        public float StoppingDistance => _agent.stoppingDistance;

        public void Update()
        {
            if (IsInitialized())
            {
                _agent.destination = _target;
                _animator.Move(_agent.velocity.magnitude);
            }
            if (IsDestinationReached(_target))
            {
                StopMove();
            }
        }

        public void StartMove() 
        {
            enabled = true;
        }

        public void StopMove()
        {
            enabled = false;
            _agent.ResetPath();
            _animator.StopMoving();
        }

        public void SetDestination(Vector3 target)
        {
            _target = target;
        }

        private bool IsInitialized()
        {
            return _target != Vector3.zero;
        }

        private bool IsDestinationReached(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) >= StoppingDistance;
        }

        public void Receive(Stats stats)
        {
            _agent.speed = stats.MoveSpeed;
        }
    }
}