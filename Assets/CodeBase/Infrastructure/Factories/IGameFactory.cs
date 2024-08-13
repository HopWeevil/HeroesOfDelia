using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using CodeBase.Logic.Loot;
using CodeBase.Logic;

namespace CodeBase.Infrastructure.Factories
{
    public interface IGameFactory
    {
        Task<GameObject> CreateHud();
        Task<GameObject> CreateSaveTrigger(Vector3 at);
        Task CreateSpawner(string spawnerId, Vector3 at, EnemyTypeId enemyTypeId);
        Task<ResourceLoot> CreateResourceLoot(ResourceTypeId resourceType, Vector3 at);
        Task<EquipmentLoot> CreateEquipmentLoot(EquipmentTypeId equipmentType, Vector3 at);
        Task<GameObject> CreateEquipment(EquipmentTypeId equipmentType, Transform parent);
    }
}