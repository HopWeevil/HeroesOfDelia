using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Windows;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipmentSlotView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private EquipmentCategory _equipmentClass;

    private IPersistentProgressService _progressService;
    private IStaticDataService _staticDataService;
    private IUIFactory _factory;

    private Sprite _primalSprite;

    private EquipmentStaticData _data;
    private InventoryItem _equipment;

    [Inject]
    private void Construct(IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uIFactory)
    {
        _progressService = progressService;
        _staticDataService = staticData;
        _factory = uIFactory;
    }

    private void Start()
    {
        _progressService.Inventory.OnHeroEquip += OnHeroEquip;
        _progressService.Inventory.OnHeroUnEquip += OnHeroUnEquip;
        _button.onClick.AddListener(OnClickAsync);
        _primalSprite = _icon.sprite;
    }

    private void OnDestroy()
    {
        _progressService.Inventory.OnHeroEquip -= OnHeroEquip;
        _progressService.Inventory.OnHeroUnEquip -= OnHeroUnEquip;
        _button.onClick.RemoveListener(OnClickAsync);
    }

    private async void OnClickAsync()
    {
        EquipmentItemWindow window = await _factory.CreateEquipmentInfoWindow(_data);
        window.SetItem(_equipment);
    }

    private void OnHeroEquip(HeroTypeId hero, InventoryItem equipment)
    {
        _data = _staticDataService.ForEquipment(equipment.EquipmentTypeId);
        _equipment = equipment;
        if (_data != null && _data.EquipmentClass == _equipmentClass) 
        {
            _icon.sprite = _data.Icon;
        }
    }

    private void OnHeroUnEquip(HeroTypeId hero, InventoryItem equipment)
    {
        _data = _staticDataService.ForEquipment(equipment.EquipmentTypeId);
        if (_data != null && _data.EquipmentClass == _equipmentClass)
        {
            _icon.sprite = _primalSprite;
        }
    }
}
