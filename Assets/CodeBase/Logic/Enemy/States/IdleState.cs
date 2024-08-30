﻿using CodeBase.Character;
using CodeBase.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Enemy.States
{
    public class IdleState : EnemyState
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _minDistance;

        public override void Enter()
        {

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

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}