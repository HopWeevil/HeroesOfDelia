using CodeBase.Services.IAP;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Button _buyItemButton;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _itemIcon;

        private IIAPService _iapService;
        private string _productId;

        [Inject]
        private void Construct(IIAPService iapService)
        {
            _iapService = iapService;
        }

        private void OnEnable()
        {
            _buyItemButton.onClick.AddListener(OnBuyButtonClick);
        }

        private void OnDisable()
        {
            _buyItemButton.onClick.RemoveListener(OnBuyButtonClick);
        }

        public void SetProductID(string productId)
        {
            _productId = productId;
        }

        public void SetIcon(Sprite icon)
        {
            _itemIcon.sprite = icon;
        }

        public void SetQuantity(int quantity)
        {
            _quantityText.text = quantity.ToString();
        }

        public void SetPrice(string price)
        {
            _priceText.text = price;
        }

        private void OnBuyButtonClick()
        {
            _iapService.StartPurchase(_productId);
        }
    }
}