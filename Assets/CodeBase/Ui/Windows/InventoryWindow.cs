using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private List<EquipmentItemView> _inventorySlots = new List<EquipmentItemView>();

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
            _persistentProgressService.Inventory.InventoryItemRemove += OnInventoryItemRemove;
            _persistentProgressService.Inventory.InventoryItemAdd += OnInventoryItemAdd;
        }

        private void OnDisable()
        {
            _persistentProgressService.Inventory.InventoryItemRemove -= OnInventoryItemRemove;
            _persistentProgressService.Inventory.InventoryItemAdd -= OnInventoryItemAdd;
        }

        private async void OnInventoryItemAdd(int id)
        {
            await CreateSlot(id);
        }

        private void OnInventoryItemRemove(int id)
        {
            Destroy(_inventorySlots[id].gameObject);
            _inventorySlots.RemoveAt(id);
        }

        private async void CreateSlots()
        {
            for (int i = 0; i < _persistentProgressService.Inventory.InventoryItems.Count; i++)
            {
                await CreateSlot(i);
            }
        }

        private async Task CreateSlot(int id)
        {
            EquipmentItem item = _persistentProgressService.Inventory.InventoryItems[id];
            EquipmentItemView slot = await _uIFactory.CreateEquipmentItemView(_itemsContainer, item);
            _inventorySlots.Add(slot);
        }
    }
}