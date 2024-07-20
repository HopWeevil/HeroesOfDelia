using System;
using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;

        private readonly int _speed = Animator.StringToHash("Speed");
        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _hit = Animator.StringToHash("Hit");
        private readonly int _die = Animator.StringToHash("Die");
        private readonly int _victory = Animator.StringToHash("Victory");
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _move = Animator.StringToHash("Move");
        private readonly int[] _attacks =
        {
            Animator.StringToHash("Attack_1"),
            Animator.StringToHash("Attack_2"),
            Animator.StringToHash("Attack_3")
        };

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        public AnimatorState State { get; private set; }
        public bool IsAttacking => State == AnimatorState.Attack;

        public void Move(float speed)
        {
            _animator.SetBool(_isMoving, true);
            _animator.SetFloat(_speed, speed, 0.1f, Time.deltaTime);
        }

        public void StopMoving()
        {
            _animator.SetBool(_isMoving, false);
        }

        public void PlayHit()
        {
            _animator.SetTrigger(_hit);
        }

        public void PlayAttack()
        {
            int randomIndex = UnityEngine.Random.Range(0, _attacks.Length);
            _animator.SetTrigger(_attacks[randomIndex]);
        }

        public void PlayDeath()
        {
            _animator.SetTrigger(_die);
        }

        public void ResetToIdle()
        {
            _animator.Play(_idle, -1);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idle)
            {
                state = AnimatorState.Idle;
            }
            else if (_attacks.Contains(stateHash))
            {
                return AnimatorState.Attack;
            }
            else if (stateHash == _move)
            {
                state = AnimatorState.Moving;
            }
            else if (stateHash == _die)
            {
                state = AnimatorState.Died;
            }
            else if (stateHash == _hit)
            {
                state = AnimatorState.GetHit;
            }
            else if (stateHash == _victory)
            {
                state = AnimatorState.Victory;
            }
            else
            {
                state = AnimatorState.Unknown;
            }
            return state;
        }
    }
}