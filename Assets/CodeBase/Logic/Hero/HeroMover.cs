using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.Animations;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class HeroMover : MonoBehaviour, IStatsReceiver
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _dumpTime;

        private float _movementSpeed;
        private IInputService _inputService;
        private Camera _camera;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Move();
        }

        public void Receive(Stats stats)
        {
            _movementSpeed = stats.MoveSpeed;
        }

        private void Move()
        {
            Vector3 movementVector = CalculateMovementVector();

            if (IsMoving(movementVector))
            {
                RotateTowardsMovementDirection(movementVector);
                _animator.Move(_characterController.velocity.magnitude, _dumpTime);
            }
            else
            {
                _animator.StopMoving();
            }

            ApplyGravity(ref movementVector);
            MoveCharacter(movementVector);
        }

        private Vector3 CalculateMovementVector()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude >= 0.0001f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();
            }

            return movementVector;
        }

        private bool IsMoving(Vector3 movementVector)
        {
            return movementVector.sqrMagnitude > 0;
        }

        private void RotateTowardsMovementDirection(Vector3 movementVector)
        {
            transform.forward = movementVector;
        }

        private void ApplyGravity(ref Vector3 movementVector)
        {
            movementVector += Physics.gravity;
        }

        private void MoveCharacter(Vector3 movementVector)
        {
            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }
    }
}