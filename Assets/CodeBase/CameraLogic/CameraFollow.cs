using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offsetY;

        private Transform _following;

        [Inject]
        private void Construct(GameObject hero)
        {
            _following = hero.transform;
            Debug.Log(_following.gameObject);
        }

        private void LateUpdate()
        {
            if (_following == null)
            {
                return;
            }

            Quaternion rotation = Quaternion.Euler(_rotationAngleX, 0, 0);
            Vector3 position = rotation * new Vector3(0, 0, -_distance) + FollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following)
        {
            _following = following.transform;
        }

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = _following.position;
            followingPosition.y += _offsetY;

            return followingPosition;
        }
    }
}