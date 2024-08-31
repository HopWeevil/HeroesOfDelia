﻿using CodeBase.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy.States
{
    public class AgroState : EnemyState
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private EnemyMover _enemyMover;
        [SerializeField] private EnemyAttack _enemyAttack;

        public override void Enter()
        {
            _enemyMover.StartMove(_target.transform.position);
        }

        public override void Tick()
        {
            _enemyMover.SetDestination(_target.transform.position);
        }

        public override void Exit()
        {
            _enemyMover.StopMove();
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