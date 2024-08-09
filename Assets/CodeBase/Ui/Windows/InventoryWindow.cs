using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class InventoryWindow : WindowBase
    {
        [SerializeField] private RectTransform _itemsContainer;

        private IUIFactory _uIFactory;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;

        private List<InventorySlotView> _inventorySlots = new List<InventorySlotView>();

        [Inject]
        private void Construct(IUIFactory uIFactory, IPersistentProgressService persistentProgress, IStaticDataService staticDataService)
        {
            _uIFactory = uIFactory;
            _persistentProgressService = persistentProgress;
            _staticDataService = staticDataService;
        }

        private void Start()
        {
            CreateSlots();
        }

        private void OnEnable()
        {
            _persistentProgressService.Inventory.OnInventoryItemRemove += OnInventoryItemRemove;
            _persistentProgressService.Inventory.OnInventoryItemAdd += OnInventoryItemAdd;
        }

        private async void OnInventoryItemAdd(int id)
        {
            InventoryItem item = _persistentProgressService.Inventory.InventoryItems[id];
            InventorySlotView slot = await _uIFactory.CreateInventorySlot(_itemsContainer, item);
            slot.OnClick += OnSlotClick;
            _inventorySlots.Add(slot);
        }

        private void OnInventoryItemRemove(int id)
        {
            Destroy(_inventorySlots[id].gameObject);
            _inventorySlots.RemoveAt(id);
        }

        private void OnDisable()
        {
            _persistentProgressService.Inventory.OnInventoryItemRemove -= OnInventoryItemRemove;
        }

        private void OnHeroEquip(HeroTypeId hero, EquipmentTypeId equipment)
        {
            //RefreshInventoryUI();
        }

        private async void CreateSlots()
        {
            for (int i = 0; i < _persistentProgressService.Inventory.InventoryItems.Count; i++)
            {
                InventoryItem item = _persistentProgressService.Inventory.InventoryItems[i];
                InventorySlotView slot = await _uIFactory.CreateInventorySlot(_itemsContainer, item);
                slot.OnClick += OnSlotClick;
                _inventorySlots.Add(slot);
            }
        }

        private void RefreshInventoryUI()
        {
            foreach (Transform child in _itemsContainer)
            {
                Destroy(child.gameObject);
            }

            CreateSlots();
        }

        private async void OnSlotClick(EquipmentStaticData data, InventoryItem item)
        {
            EquipmentItemWindow window = await _uIFactory.CreateEquipmentInfoWindow(data);
            window.SetItem(item);
        }
    }
}