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

        private const float MinimalDistance = 1f;

        public float StoppingDistance => _agent.stoppingDistance;
        private Vector3 test;

        private Coroutine _currentMoveCoroutine;


        public void StartMove()
        {
            enabled = true;
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

        public void StartMoveToTarget(Vector3 destination, Action onTargetReach = null)
        {
            _currentMoveCoroutine = StartCoroutine(MoveToTarget(destination, onTargetReach));
        }

        public void StartMoveToTarget(Transform destination, Action onTargetReach = null)
        {
            _currentMoveCoroutine = StartCoroutine(MoveToTarget(destination, onTargetReach));
        }

        private IEnumerator MoveToTarget(Vector3 destination, Action onTargetReach = null)
        {
            test = destination;

            while (!IsDestinationReached(destination))
            {
                _agent.SetDestination(destination);
                _animator.Move(_agent.velocity.magnitude);
                yield return null;
            }
            StopMove();
              onTargetReach?.Invoke();
        }

        private IEnumerator MoveToTarget(Transform destination, Action onTargetReach = null)
        {
            test = destination.position;

            while (!IsDestinationReached(destination.position))
            {
                _agent.SetDestination(destination.position);
                _animator.Move(_agent.velocity.magnitude);
                yield return null;
            }
            StopMove();
               onTargetReach?.Invoke();
        }

        private bool IsDestinationReached(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) <= StoppingDistance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(test, Vector3.one);
        }

        public void Receive(Stats stats)
        {
            _agent.speed = stats.MoveSpeed;
        }
    }
}
