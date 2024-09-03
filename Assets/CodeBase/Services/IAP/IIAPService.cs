using System;
using System.Collections.Generic;

namespace CodeBase.Services.IAP
{
    public interface IIAPService
    {
        bool IsInitialized { get; }

        event Action Initialized;

        List<ProductConfig> GetProducts();
        void Initialize();
        void StartPurchase(string productId);
    }
}