using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly DiContainer _container;
        private Canvas _uiRoot;
        public UIFactory(DiContainer container, IAssetProvider assets, IPersistentProgressService progressService)
        {
            _assets = assets;
            _progressService = progressService;
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
            _uiRoot = UnityEngine.Object.Instantiate(prefab).GetComponent<Canvas>();
        }
    }
}