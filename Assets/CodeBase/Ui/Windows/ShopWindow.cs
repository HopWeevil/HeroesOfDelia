using CodeBase.Infrastructure.Factories;
using CodeBase.Services.IAP;
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
        private IIAPService _iapService;

        [Inject]
        private void Construct(IStaticDataService staticData, IUIFactory factory, IIAPService iapService)
        {
            _staticData = staticData;
            _uIFactory = factory;
            _iapService = iapService;
        }

        private void Start()
        {
            CreateRewardItems();
            TryCreateShopItems();
        }

        private void OnEnable()
        {
            _iapService.Initialized += OnIAPServiceInitialized;
        }

        private void OnDisable()
        {
            _iapService.Initialized -= OnIAPServiceInitialized;
        }

        private async void CreateRewardItems()
        {
            foreach (ResourceRewardStaticData data in _staticData.GetAllRewardItems())
            {
                await _uIFactory.CreateRewardedAdItem(data, _itemsContainer);
            }
        }

        private async void TryCreateShopItems()
        {
            if(_iapService.IsInitialized == false)
            {
                return;
            }

            foreach (ProductConfig config in _iapService.GetProducts())
            {
                await _uIFactory.CreateShopItem(_itemsContainer, config);
            }
        }

        private void OnIAPServiceInitialized()
        {
            TryCreateShopItems();
        }
    }
}