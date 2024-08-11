using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroEquipper : MonoBehaviour
    {
        [SerializeField] private Transform _weaponContainer;

        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        private void OnEnable()
        {
            _progressService.Inventory.HeroEquip += OnHeroEquip;
            _progressService.Inventory.HeroUnEquip += OnHeroUnequip;
        }

        private void OnDisable()
        {
            _progressService.Inventory.HeroEquip -= OnHeroEquip;
            _progressService.Inventory.HeroUnEquip -= OnHeroUnequip;
        }

        private void OnHeroUnequip(HeroTypeId id, EquipmentItem item)
        {
            if (_weaponContainer.childCount > 0)
            {
                Transform currentWeapon = _weaponContainer.GetChild(0);
                Destroy(currentWeapon.gameObject);
            }
        }

        private void OnHeroEquip(HeroTypeId id, EquipmentItem item)
        {
            if (_weaponContainer.childCount > 0)
            {
                Transform currentWeapon = _weaponContainer.GetChild(0);
                Destroy(currentWeapon.gameObject);
            }

            _gameFactory.CreateEquipment(item.EquipmentTypeId, _weaponContainer);
        }

        private void Start()
        {
            
        }
    }
}
