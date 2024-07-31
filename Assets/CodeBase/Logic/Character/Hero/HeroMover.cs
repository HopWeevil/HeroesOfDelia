using CodeBase.Character;
using CodeBase.Data;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class HeroMover : MonoBehaviour
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
        public void SetStats(float speed)
        {
            _movementSpeed = speed;
        }

        private void Move()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude >= 0.0001)
            {

                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();
                transform.forward = movementVector;
                _animator.Move(_characterController.velocity.magnitude, _dumpTime);
            }
            else
            {
                _animator.StopMoving();
            }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }
    }
}