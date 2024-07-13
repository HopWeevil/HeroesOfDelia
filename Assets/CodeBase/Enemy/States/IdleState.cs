﻿using CodeBase.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Enemy.States
{
    public class IdleState : EnemyState
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minDistance;

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

            if (distance >= _minDistance && distance <= _maxDistance)
            {
                return true;
            }

            return false;
        }
    }
}