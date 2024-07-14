using CodeBase.Enemy;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace CodeBase.Enemy.States
{
    public class PatrolState : EnemyState
    {   
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minDistance;
        [SerializeField] private EnemyPatrol _patrol;

        public override void Enter()
        {

        }

        public override void Execute()
        {
            _patrol.Execute();
        }

        public override void Exit()
        {
            _patrol.DisablePatrol();
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