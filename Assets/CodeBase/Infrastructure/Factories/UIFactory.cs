using CodeBase.Enums;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Windows;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly DiContainer _container;
        private Canvas _uiRoot;
        public UIFactory(DiContainer container, IAssetProvider assets, IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _assets = assets;
            _progressService = progressService;
            _staticData = staticData;
            _container = container;
        }

        public async Task<GameObject> CreateInventory()
        {
            return await CreateWindow(AssetAddress.InventoryWindow);
        }

        public async Task<GameObject> CreateShop()
        {
            return await CreateWindow(AssetAddress.ShopWindow);
        }

        public async Task<GameObject> CreateMainMenu()
        {
            return await CreateWindow(AssetAddress.MainMenu);
        }

        public async Task<EquipmentItemWindow> CreateEquipmentInfoWindow(EquipmentStaticData data)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.EquipmentItemWindow);
            EquipmentItemWindow window = _container.InstantiatePrefab(prefab, _uiRoot.transform).GetComponent<EquipmentItemWindow>();
            window.SetEquipmentData(data);
            return window;
        }

        public async Task<InventorySlotView> CreateInventorySlot(RectTransform parent, InventoryItem item)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.InventorySlot);
            InventorySlotView slot = Object.Instantiate(prefab, parent).GetComponent<InventorySlotView>();
            EquipmentStaticData data = _staticData.ForEquipment(item.EquipmentTypeId);
            slot.SetEquipmentdata(data, item);
            return slot;
        }

        public async Task<GameObject> CreateWindow(string address)
        {
            GameObject prefab = await _assets.Load<GameObject>(address);
            GameObject window = _container.InstantiatePrefab(prefab, _uiRoot.transform);
            return window;
        }

        public async Task<GameObject> CreateWindow(WindowId windowId)
        {
            GameObject prefab = await _assets.Load<GameObject>(windowId.ToString());
            GameObject window = _container.InstantiatePrefab(prefab, _uiRoot.transform);
            return window;
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.UIRootPath);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

    }
}