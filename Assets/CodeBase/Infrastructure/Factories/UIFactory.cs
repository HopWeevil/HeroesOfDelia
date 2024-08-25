using CodeBase.Enums;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Windows;
using DG.Tweening;
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
            return await CreateWindow(AssetAddress.HeroEquipmentWindow);
        }

        public async Task<GameObject> CreateShop()
        {
            return await CreateWindow(AssetAddress.ShopWindow);
        }

        public async Task<GameObject> CreateMainMenu()
        {
            return await CreateWindow(AssetAddress.MainMenu);
        }

        public async Task<EquipmentItemWindow> CreateEquipmentInfoWindow(EquipmentItem item)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.EquipmentItemWindow);
            EquipmentItemWindow window = _container.InstantiatePrefab(prefab, _uiRoot.transform).GetComponent<EquipmentItemWindow>();
            EquipmentStaticData data = _staticData.ForEquipment(item.EquipmentTypeId);
            window.SetEquipment(item, data);
            return window;
        }

        public async Task<EquipmentItemView> CreateEquipmentItemView(RectTransform parent, EquipmentItem item)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.InventorySlot);
            EquipmentItemView slot = _container.InstantiatePrefab(prefab, parent).GetComponent<EquipmentItemView>();

            EquipmentStaticData data = _staticData.ForEquipment(item.EquipmentTypeId);
            slot.SetEquipmentData(data, item);

            slot.transform.localScale = Vector3.zero;
            slot.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

            return slot;
        }

        public async Task<LevelCard> CreateLevelCard(LevelStaticData data, RectTransform parent)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LevelCard);
            LevelCard card = _container.InstantiatePrefab(prefab, parent).GetComponent<LevelCard>();
            card.SetLevelData(data);
            card.SetInfo();
            return card;
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

        public async Task<PopupMessage> CreatePopupMessage(Color color, string text)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.PopupMessage);
            PopupMessage message = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<PopupMessage>();
            message.SetColor(color);
            message.SetText(text);
            return message;
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.UIRootPath);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HudPath);
            GameObject hud = Object.Instantiate(prefab);
            _container.InjectGameObject(hud);
            return hud;
        }
    }
}