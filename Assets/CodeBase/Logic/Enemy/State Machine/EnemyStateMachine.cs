using CodeBase.Enemy.States;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Enemy.StateMachine
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [SerializeField] private EnemyState _defaultState;
        [SerializeField] private List<EnemyState> _states;

        private GameObject _hero;
        private EnemyState _currentState;

        public void Construct(GameObject hero)
        {
            _hero = hero;
        }

        private void Start()
        {
            foreach (var state in _states)
            {
                state.Initialize(_hero);
            }

            ChangeState(_defaultState);
        }

        private void Update()
        {
            if (_currentState != null)
            {
                foreach (var state in _states)
                {
                    if (state != _currentState && state.ShouldTransit())
                    {
                        ChangeState(state);
                        break;
                    }
                }
            }
        }

        public void ChangeState(EnemyState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }
    }
}