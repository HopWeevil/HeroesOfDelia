using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class HeroStatsInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _attackCooldown;
    [SerializeField] private TMP_Text _attackDistance;
    [SerializeField] private TMP_Text _attackDamage;
    [SerializeField] private TMP_Text _attackSplash;
    [SerializeField] private TMP_Text _armor;
    [SerializeField] private TMP_Text _moveSpeed;

    private IPersistentProgressService _progressService;
    private IStaticDataService _staticData;

    private string _healthFormat;
    private string _attackSpeedFormat;
    private string _attackDistanceFormat;
    private string _attackDamageFormat;
    private string _attackSplashFormat;
    private string _armorFormat;
    private string _moveSpeedFormat;

    [Inject]
    private void Construct(IPersistentProgressService progressService, IStaticDataService staticData)
    {
        _progressService = progressService;
        _staticData = staticData;
    }

    private void OnEnable()
    {
        _progressService.Equipments.HeroEquip += OnHeroEquip;
        _progressService.Equipments.HeroUnEquip += OnHeroUnequip;
    }

    private void OnDisable()
    {
        _progressService.Equipments.HeroEquip -= OnHeroEquip;
        _progressService.Equipments.HeroUnEquip -= OnHeroUnequip;
    }

    private void Start()
    {
        _healthFormat = _health.text;
        _attackSpeedFormat = _attackCooldown.text;
        _attackDistanceFormat = _attackDistance.text;
        _attackDamageFormat = _attackDamage.text;
        _attackSplashFormat = _attackSplash.text;
        _armorFormat = _armor.text;
        _moveSpeedFormat = _moveSpeed.text;

        Stats stats = CalculateStats(_progressService.Progress.SelectedHero);
        UpdateStats(stats);
    }

    private void OnHeroUnequip(HeroTypeId id, EquipmentItem item)
    {
        Stats stats = CalculateStats(id);
        UpdateStats(stats);
    }

    private void OnHeroEquip(HeroTypeId id, EquipmentItem equipment)
    {
        Stats stats = CalculateStats(id);
        UpdateStats(stats);
    }

    private Stats CalculateStats(HeroTypeId id)
    {
        CharacterStaticData data = _staticData.ForHero(id);

        Stats stats = new Stats(data.Hp, data.Damage, data.MoveSpeed, data.Armor, data.AttackCooldown, data.Cleavage, data.EffectiveDistance);

        if (_progressService.Equipments.HeroesEquipment.TryGetValue(id, out var items))
        {
            foreach (var item in items)
            {
                StatsBonus[] bonuses = _staticData.ForEquipment(item.Value.EquipmentTypeId).Bonuses.ToArray();
                stats.ApplyStatsBonuses(bonuses, item.Value.Level);
            }
        }
      
        return stats;
    }

    private void UpdateStats(Stats stats)
    {
        _attackDistance.text = string.Format(_attackDistanceFormat, stats.AttackDistance);
        _attackDamage.text = string.Format(_attackDamageFormat, stats.Damage);
        _attackCooldown.text = string.Format(_attackSpeedFormat, stats.AttackCooldown);
        _attackSplash.text = string.Format(_attackSplashFormat, stats.AttackSplash);
        _health.text = string.Format(_healthFormat, stats.Hp);
        _moveSpeed.text = string.Format(_moveSpeedFormat, stats.MoveSpeed);
        _armor.text = string.Format(_armorFormat, stats.Armor);
    }
}
