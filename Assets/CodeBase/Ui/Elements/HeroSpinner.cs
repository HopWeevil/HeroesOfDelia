using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class HeroSpinner : MonoBehaviour
    {
        [SerializeField] private RawImage _heroRenderImage;
        [SerializeField] public float _rotationSpeed = 0.2f;
        [SerializeField] private float _damping = 0.95f;

        public Transform _hero;
        private Vector3 _lastMousePosition;
        private bool _isDragging;
        private float _inertia;
        private IShowcaseHeroFactory _showcaseHeroFactory;

        [Inject]
        private void Construct(IShowcaseHeroFactory showcaseHeroFactory)
        {
            _showcaseHeroFactory = showcaseHeroFactory;
        }

        private void OnEnable()
        {
            Debug.Log("Sub");
            _showcaseHeroFactory.OnCreated += OnHeroCreated;
        }

        private void OnDestroy()
        {
            _showcaseHeroFactory.OnCreated -= OnHeroCreated;
        }

        private void OnHeroCreated(GameObject hero)
        {
            _hero = hero.transform;
        }

        private void Update()
        {
            if (_hero == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && IsMouseOverImage())
            {
                _lastMousePosition = Input.mousePosition;
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                Vector3 delta = Input.mousePosition - _lastMousePosition;
                float rotationY = delta.x * _rotationSpeed;
                _hero.Rotate(Vector3.up, -rotationY, Space.World);
                _lastMousePosition = Input.mousePosition;
                _inertia = rotationY;
            }
            else if (Mathf.Abs(_inertia) > 0.01f)
            {
                _hero.Rotate(Vector3.up, -_inertia, Space.World);
                _inertia *= _damping;
            }
        }

        private bool IsMouseOverImage()
        {
            RectTransform rectTransform = _heroRenderImage.rectTransform;
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
            return rectTransform.rect.Contains(localMousePosition);
        }
    }
}