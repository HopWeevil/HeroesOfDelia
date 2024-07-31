using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private ResourceTypeId _resourceTypeId;
        [SerializeField] private TextMeshProUGUI _counter;

        private IPersistentProgressService _persistentProgress;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress)
        {
            _persistentProgress = persistentProgress;           
        }

        private void Start()
        {
            _persistentProgress.Economy.ResourcesAmountChanged += UpdateCounter;
            UpdateCounter(_resourceTypeId);
        }

        private void OnDestroy()
        {
            _persistentProgress.Economy.ResourcesAmountChanged -= UpdateCounter;
        }

        private void UpdateCounter(ResourceTypeId typeId)
        {
            if (_resourceTypeId == typeId) 
            {
                _counter.text = _persistentProgress.Economy.GetResourceAmount(_resourceTypeId).ToString();
            }    
        }
    }
}