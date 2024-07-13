using CodeBase.Logic;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{

    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private float _patrolCooldown = 3f;
        [SerializeField] private float _patrolRadius = 2f;
        [SerializeField] private EnemyMover _agentMover;

        private bool _isMoving;
        private float _currentCooldown;

        public void Execute()
        {
            UpdateCooldown();
            if (CanMove())
            {
                if (TryGetRandomPosition(transform.position, _patrolRadius, _agentMover.StoppingDistance, out Vector3 position))
                {
                    StartCoroutine(MoveToTarget(position, DisablePatrol));
                }
            }
        }
        public void DisablePatrol()
        {
            _agentMover.StopMove();
            _isMoving = false;
            _currentCooldown = _patrolCooldown;
        }

        private IEnumerator MoveToTarget(Vector3 destination, Action onTargetReach)
        {
            _isMoving = true;
            _agentMover.SetDestination(destination);

            while (IsDestinationReached(destination))
            {
                _agentMover.Execute();
                yield return null;
            }

            //yield return new WaitUntil(() => !IsDestinationReached(destination));

            onTargetReach?.Invoke();
        }

        private bool IsDestinationReached(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) >= _agentMover.StoppingDistance;
        }

        private bool TryGetRandomPosition(Vector3 center, float radius, out Vector3 position)
        {
            return TryGetRandomPosition(center, radius, radius, out position);
        }

        private bool TryGetRandomPosition(Vector3 center, float minRadius, float maxRadius, out Vector3 position)
        {
            float randomRadius = UnityEngine.Random.Range(minRadius, maxRadius);
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * randomRadius;
            Vector3 sampledPosition = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(sampledPosition, out NavMeshHit hit, maxRadius, 1))
            {
                position = hit.position;
                return true;
            }

            position = center;
            return false;
        }

        private bool CanMove()
        {
            if (!_isMoving && CooldownIsUp())
            {
                return true;
            }
            return false;
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