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
        public float StoppingDistance => _agent.stoppingDistance;

        private void Update()
        {
            if (IsInitialized())
            {              
                _animator.Move(_agent.velocity.magnitude);
            }
        }

        public void StartMove(Vector3 position) 
        {
            enabled = true;
            SetDestination(position);
        }

        public void StopMove()
        {
            enabled = false;
            _agent.ResetPath();
            _animator.StopMoving();
        }

        public void SetDestination(Vector3 target)
        {
            _agent.destination = target;
        }

        private bool IsInitialized()
        {
            return _agent.destination != Vector3.zero;
        }

        public void Receive(Stats stats)
        {
            _agent.speed = stats.MoveSpeed;
        }
    }
}