using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        private IPersistentProgressService _persistentProgress;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress)
        {
            _persistentProgress = persistentProgress;           
        }

        private void Start()
        {
            _persistentProgress.Economy.CoinsAmountChanged += UpdateCounter;
            UpdateCounter();
        }

        private void OnDestroy()
        {
            _persistentProgress.Economy.CoinsAmountChanged -= UpdateCounter;
        }

        private void UpdateCounter()
        {
            _counter.text = _persistentProgress.Economy.GoldAmount.ToString();
        }
    }
}