using System;
using CodeBase.Enums;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    [Serializable]
    public class ProductConfig
    {
        public string Id;
        public ProductType ProductType;
        public string Price;
        public string Icon;
        public ShopItemType ItemType;
        public int Quantity;
    }
}