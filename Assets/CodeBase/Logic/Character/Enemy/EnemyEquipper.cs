using CodeBase.Infrastructure.Factories;
using CodeBase.Services.StaticData;
using CodeBase.Enums;
using CodeBase.SO;
using System.Linq;
using UnityEngine;
using Zenject;

public class EnemyEquipper : MonoBehaviour
{
    [SerializeField] private Transform _weaponContainer;

    private IGameFactory _gameFactory;
    private IStaticDataService _staticData;

    [Inject]
    private void Construct(IGameFactory gameFactory, IStaticDataService staticData)
    {
        _gameFactory = gameFactory;
        _staticData = staticData;
    }

    public void Equip(EnemyStaticData data)
    {
        if (_weaponContainer.childCount > 0)
        {
            Transform currentWeapon = _weaponContainer.GetChild(0);
            Destroy(currentWeapon.gameObject);
        }

       /* var weaponItem = data.Equipments.FirstOrDefault(cl => _staticData.ForEquipment(cl.EquipmentTypeId).EquipmentClass == EquipmentCategory.Weapon);
        _gameFactory.CreateEquipment(weaponItem.EquipmentTypeId, _weaponContainer);*/
    }
}
