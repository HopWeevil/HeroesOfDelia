using CodeBase.Character;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private float _patrolCooldown = 3f;
        [SerializeField] private float _patrolRadius = 2f;
        [SerializeField] private EnemyMover _enemyMover;

        private bool _isPatrolling;
        private float _currentCooldown;
        private Coroutine _patrolCoroutine;

        public void StartPatrol()
        {
            if (_patrolCoroutine != null)
            {
                StopCoroutine(_patrolCoroutine);
            }
            _patrolCoroutine = StartCoroutine(PatrolRoutine());
        }

        public void StopPatrol()
        {
            if (_patrolCoroutine != null)
            {
                StopCoroutine(_patrolCoroutine);
                _patrolCoroutine = null;
            }
            ResetPatrol();
        }

        private IEnumerator PatrolRoutine()
        {
            while (true)
            {
                HandlePatrol();

                yield return null;
            }
        }

        private void HandlePatrol()
        {
            UpdateCooldown();

            if (CanMove())
            {
                if (TryGetRandomPosition(transform.position, _patrolRadius, out Vector3 position))
                {
                    StartCoroutine(MoveToTargetRoutine(position, ResetPatrol));
                }
            }
        }

        private void ResetPatrol()
        {
            _isPatrolling = false;
            _currentCooldown = _patrolCooldown;
            _enemyMover.StopMove();
        }

        private IEnumerator MoveToTargetRoutine(Vector3 destination, Action onTargetReach)
        {
            _isPatrolling = true;
            _enemyMover.StartMove(destination);

            yield return new WaitUntil(() => IsDestinationReached(destination));

            onTargetReach?.Invoke();
        }

        private bool IsDestinationReached(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) <= _enemyMover.StoppingDistance;
        }

        private bool TryGetRandomPosition(Vector3 center, float radius, out Vector3 position)
        {
            float randomRadius = UnityEngine.Random.Range(0f, radius);
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * randomRadius;
            Vector3 sampledPosition = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(sampledPosition, out NavMeshHit hit, radius, 1))
            {
                position = hit.position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        private bool CanMove()
        {
            return !_isPatrolling && CooldownIsUp();
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
            {
                _currentCooldown -= Time.deltaTime;
            }
        }

        private bool CooldownIsUp()
        {
            return _currentCooldown <= 0;
        }
    }
}
