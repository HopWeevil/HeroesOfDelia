using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroEquipper : MonoBehaviour
    {
        [SerializeField] private Transform _weaponContainer;

        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;
        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
        }

        private void Start()
        {
            _progressService.Equipments.HeroEquip += OnHeroEquip;
            _progressService.Equipments.HeroUnEquip += OnHeroUnequip;
        }

        private void OnDestroy()
        {
            _progressService.Equipments.HeroEquip -= OnHeroEquip;
            _progressService.Equipments.HeroUnEquip -= OnHeroUnequip;
        }

        public async Task TryEquip(HeroTypeId hero)
        {
            if (_progressService.Equipments.HeroesEquipment.TryGetValue(hero, out var equipment))
            {
                await EquipWeapon(equipment[EquipmentCategory.Weapon].EquipmentTypeId);
            }
        }

        private async void OnHeroEquip(HeroTypeId id, EquipmentItem item)
        {
            if (IsWeapon(item))
            {
                ClearWeaponSlot();
                await EquipWeapon(item.EquipmentTypeId);
            }
        }

        private void OnHeroUnequip(HeroTypeId id, EquipmentItem item)
        {
            if (IsWeapon(item))
            {
                ClearWeaponSlot();
            }
        }

        private bool IsWeapon(EquipmentItem item)
        {
            var equipmentData = _staticData.ForEquipment(item.EquipmentTypeId);
            return equipmentData?.Category == EquipmentCategory.Weapon;
        }

        private void ClearWeaponSlot()
        {
            if (_weaponContainer.childCount > 0)
            {
                Transform currentWeapon = _weaponContainer.GetChild(0);
                Destroy(currentWeapon.gameObject);
            }
        }

        private async Task EquipWeapon(EquipmentTypeId equipmentTypeId)
        {
            await _gameFactory.TryCreateEquipment(equipmentTypeId, _weaponContainer);
        }
    }
}
