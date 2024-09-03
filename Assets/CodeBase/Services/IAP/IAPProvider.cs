using CodeBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace CodeBase.Services.IAP
{
    public class IAPProvider : IDetailedStoreListener
    {
        private const string IAPConfigsPath = "IAP/Products";
        private Dictionary<string, ProductConfig> _configs;
        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private IAPService _iapService;

        public event Action Initialized;
        public bool IsInitialized => _controller != null && _extensions != null;

        public IReadOnlyDictionary<string, ProductConfig> Configs => _configs;

        public IAPProvider(IAPService iapService)
        {
            _iapService = iapService;
            _configs = new Dictionary<string, ProductConfig>();

        }

        public void Initialize()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            _configs = LoadProductConfigs();
            foreach(ProductConfig productConfig in _configs.Values)
            {
                builder.AddProduct(productConfig.Id, productConfig.ProductType);
            }         

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string productId)
        {
            _controller.InitiatePurchase(productId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            Initialized?.Invoke();

            Debug.Log("UnityPurchasing initialization success!");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"UnityPurchasing initialization failed! {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"UnityPurchasing initialization failed! {error}, message: {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogError($"Product {product.definition.id} purchase failed, reason: {failureDescription.message}, transaction id: {product.transactionID}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Product {product.definition.id} purchase failed, reason: {failureReason}, transaction id: {product.transactionID}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Unity purchase success! Item id: {purchaseEvent.purchasedProduct.definition.id}");

            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        private Dictionary<string, ProductConfig> LoadProductConfigs()
        {
            return Resources.Load<TextAsset>(IAPConfigsPath).text.ToDeserialized<ProductConfigWrapper>().Configs.ToDictionary(x => x.Id, x => x);
        }    
    }
}