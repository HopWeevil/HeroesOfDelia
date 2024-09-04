using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enums;
using UnityEngine;
using CodeBase.Logic.Loot;

namespace CodeBase.Infrastructure.Factories
{
    public interface IGameFactory
    {
        Task<GameObject> CreateSaveTrigger(Vector3 at);
        Task CreateSpawner(Vector3 at, EnemyTypeId enemyTypeId);
        Task<ResourceLoot> CreateResourceLoot(ResourceTypeId resourceType, Vector3 at);
        Task<EquipmentLoot> CreateEquipmentLoot(EquipmentTypeId equipmentType, Vector3 at);
        Task TryCreateEquipment(EquipmentTypeId equipmentType, Transform parent);
        Task WarmUp();
        void CleanUp();
    }
}