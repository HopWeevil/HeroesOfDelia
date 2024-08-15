using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private EquipmentCategory _equipmentCategory;
    [SerializeField] private RectTransform _itemContainer;

    private IPersistentProgressService _progressService;
    private IStaticDataService _staticDataService;
    private IUIFactory _factory;

    private EquipmentItemView _currentItemView;

    [Inject]
    private void Construct(IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uIFactory)
    {
        _progressService = progressService;
        _staticDataService = staticData;
        _factory = uIFactory;
    }

    private void OnEnable()
    {
        _progressService.Equipments.HeroEquip += OnHeroEquip;
        _progressService.Equipments.HeroUnEquip += OnHeroUnEquip;
    }

    private void OnDisable()
    {
        _progressService.Equipments.HeroEquip -= OnHeroEquip;
        _progressService.Equipments.HeroUnEquip -= OnHeroUnEquip;
    }

    private async void Start()
    {
        await LoadEquippedItem();
    }

    private async void OnHeroEquip(HeroTypeId hero, EquipmentItem equipment)
    {
        if (IsMatchingCategory(equipment))
        {
            _currentItemView = await _factory.CreateEquipmentItemView(_itemContainer, equipment);
        }
    }

    private void OnHeroUnEquip(HeroTypeId hero, EquipmentItem equipment)
    {
        if (IsMatchingCategory(equipment))
        {
            DestroyCurrentView();
        }
    }

    private async Task LoadEquippedItem()
    {
        EquipmentItem item = _progressService.Equipments.GetEquippedItem(_progressService.Progress.SelectedHero, _equipmentCategory);
        if (item != null)
        {
            _currentItemView = await _factory.CreateEquipmentItemView(_itemContainer, item);
        }
    }

    private bool IsMatchingCategory(EquipmentItem equipment)
    {
        EquipmentStaticData data = _staticDataService.ForEquipment(equipment.EquipmentTypeId);
        return data != null && data.Category == _equipmentCategory;
    }

    private void DestroyCurrentView()
    {
        if (_currentItemView != null)
        {
            Destroy(_currentItemView.gameObject);
        }
    }
}