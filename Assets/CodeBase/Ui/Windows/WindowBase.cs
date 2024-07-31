using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            Init();
            SubscribeUpdates();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void OnAwake()
        {
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }

        protected virtual void Init() { }
        protected virtual void SubscribeUpdates() { }
        protected virtual void Cleanup() { }
    }
}