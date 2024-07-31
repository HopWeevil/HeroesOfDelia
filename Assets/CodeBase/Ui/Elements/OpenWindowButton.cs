using CodeBase.Infrastructure.Factories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowId _windowId;

        private IUIFactory _factory;

        [Inject]
        private void Construct(IUIFactory factory)
        {
            _factory = factory;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Open);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Open);
        }

        private void Open()
        {
            _factory.CreateWindow(_windowId);
        }
    }
}