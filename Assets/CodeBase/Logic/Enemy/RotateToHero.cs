using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : MonoBehaviour
    {
        public float Speed;

        private Transform _heroTransform;
        private Vector3 _positionToLook;
        private bool _canRotate;
        
        public void Construct(Transform heroTransfrom)
        {
            _heroTransform = heroTransfrom;
        }

        public void Update()
        {
            if (IsInitialized() && _canRotate)
            {
                RotateTowardsHero();
            }
        }

        public void EnableRotate()
        {
            _canRotate = true;
        }

        public void DisableRotate()
        {
            _canRotate = false;
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDelta = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDelta.x, transform.position.y, positionDelta.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook)
        {
            return Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());
        }

        private Quaternion TargetRotation(Vector3 position)
        {
            return Quaternion.LookRotation(position);
        }

        private float SpeedFactor()
        {
            return Speed * Time.deltaTime;
        }

        private bool IsInitialized()
        {
            return _heroTransform != null;
        }
    }
}