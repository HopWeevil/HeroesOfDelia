using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;
using System;
using CodeBase.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Services.IAP
{
    public class IAPService : IIAPService
    {
        private readonly IAPProvider _provider;
        private readonly IPersistentProgressService _progressService;

        public bool IsInitialized => _provider.IsInitialized;
        public event Action Initialized;

        public IAPService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _provider = new IAPProvider(this);
        }

        public void Initialize()
        {     
            _provider.Initialize();
            _provider.Initialized += () => Initialized?.Invoke();
        }

        public List<ProductConfig> GetProducts() 
        {
            return _provider.Configs.Values.ToList();
        }

        public void StartPurchase(string productId)
        {
            _provider.StartPurchase(productId);
        }

        public PurchaseProcessingResult ProcessPurchase(Product purchasedProduct)
        {
            ProductConfig productConfig = _provider.Configs[purchasedProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ShopItemType.SmallCoinPack:
                case ShopItemType.BigCoinPack:
                    _progressService.Economy.IncreaseResourceAmount(ResourceTypeId.Coin, productConfig.Quantity);
                    break;

                case ShopItemType.SmallGemPack:
                case ShopItemType.BigGemPack:
                    _progressService.Economy.IncreaseResourceAmount(ResourceTypeId.Gem, productConfig.Quantity);
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }
    }
}