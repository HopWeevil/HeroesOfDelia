using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroStats : MonoBehaviour
    {
        public float Attack;
        public float Speed;
        public float Defense;


        private HeroStaticData _staticData;
        private IPersistentProgressService _progressService;
        private IStaticDataService _staticDataService;
        public void SetStaticData(HeroStaticData staticData)
        {
            _staticData = staticData;
        }

        [Inject]
        private void Construct(IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _progressService = progressService;
            _staticDataService = staticData;
        }

        private void Start()
        {
            if (_progressService.Equipments.HeroesEquipment.TryGetValue(_progressService.Progress.SelectedHero, out var heroEquipment))
            {
                foreach (var item in heroEquipment.Values)
                {
                    Equip(_staticDataService.ForEquipment(item.EquipmentTypeId));
                }
            }

            _progressService.Equipments.HeroEquip += OnEquip;
            _progressService.Equipments.HeroUnEquip += OnUnequip;
        }

        private void OnUnequip(HeroTypeId id, EquipmentItem item)
        {
            Unequip(_staticDataService.ForEquipment(item.EquipmentTypeId));
        }

        private void OnEquip(HeroTypeId id, EquipmentItem item)
        {
            Equip(_staticDataService.ForEquipment(item.EquipmentTypeId));
        }

        public void Equip(EquipmentStaticData equipment)
        {
            foreach (var bonus in equipment.Bonuses)
            {
                ApplyBonus(bonus);
            }
        }

        private void ApplyBonus(StatsBonus bonus)
        {
            switch (bonus.Type)
            {
              /*  case BonusType.Attack:
                    Attack += bonus.Value;
                    break;
                case BonusType.Speed:
                    Speed += bonus.Value;
                    break;
                case BonusType.Armor:
                    Defense += bonus.Value;
                    break;*/
            }
        }

        public void Unequip(EquipmentStaticData equipment)
        {
            foreach (var bonus in equipment.Bonuses)
            {
                RemoveBonus(bonus);
            }
        }

        private void RemoveBonus(StatsBonus bonus)
        {
            switch (bonus.Type)
            {
               /* case BonusType.Attack:
                    Attack -= bonus.Value;
                    break;
                case BonusType.Speed:
                    Speed -= bonus.Value;
                    break;
                case BonusType.Armor:
                    Defense -= bonus.Value;
                    break;*/
            
            }
        }
    }
}
public interface IBonusReceiver
{
    void ApplyBonus(StatsBonus bonus);
    void RemoveBonus(StatsBonus bonus);
}

