using CodeBase.Character;
using CodeBase.Data;
using System;
using System.Collections;
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

        private Coroutine _currentMoveCoroutine;

        public void Receive(Stats stats)
        {
            _agent.speed = stats.MoveSpeed;
        }

        public void StartMoveToTarget(Vector3 destination, Action onTargetReach = null)
        {
            _currentMoveCoroutine = StartCoroutine(MoveToTarget(destination, onTargetReach));
        }

        public void StartMoveToTarget(Transform destination, Action onTargetReach = null)
        {
            _currentMoveCoroutine = StartCoroutine(MoveToTarget(destination, onTargetReach));
        }

        public void StopMove()
        {
            _agent.ResetPath();
            _animator.StopMoving();

            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
                _currentMoveCoroutine = null;
            }
        }

        private IEnumerator MoveToTarget(Vector3 destination, Action onTargetReach = null)
        {
            while (!IsDestinationReached(destination))
            {
                _agent.SetDestination(destination);
                _animator.Move(_agent.velocity.magnitude);
                yield return null;
            }
            onTargetReach?.Invoke();
            StopMove();
        }

        private IEnumerator MoveToTarget(Transform destination, Action onTargetReach = null)
        {
            while (!IsDestinationReached(destination.position))
            {
                _agent.SetDestination(destination.position);
                _animator.Move(_agent.velocity.magnitude);
                yield return null;
            }
            onTargetReach?.Invoke();
            StopMove();
        }

        private bool IsDestinationReached(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) <= StoppingDistance;
        }
    }
}