using CodeBase.Infrastructure.Factories;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private RectTransform _itemsContainer;

        private IStaticDataService _staticData;  
        private IUIFactory _uIFactory;

        [Inject]
        private void Construct(IStaticDataService staticData, IUIFactory factory)
        {
            _staticData = staticData;
            _uIFactory = factory;
        }

        private void Start()
        {
            CreateRewardItems();
        }

        private async void CreateRewardItems()
        {
            foreach (ResourceRewardStaticData data in _staticData.GetAllRewardItems())
            {
                await _uIFactory.CreateRewardedAdItem(data, _itemsContainer);
            }
        }
    }
}