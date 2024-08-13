using CodeBase.Services.StaticData;
using UnityEngine;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Randomizer;
using Zenject;
using CodeBase.Enums;
using CodeBase.Logic.Loot;
using CodeBase.SO;


namespace CodeBase.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly DiContainer _container;
   
        public GameFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticDataService, IRandomService randomService)
        {
            _assets = assets;
            _staticData = staticDataService;
            _randomService = randomService;
            _container = container;
        }

        public async Task<GameObject> CreateEquipment(EquipmentTypeId equipmentType, Transform parent)
        {
            EquipmentStaticData equipment = _staticData.ForEquipment(equipmentType);
            GameObject prefab = await _assets.Load<GameObject>(equipment.PrefabReference);
            return Object.Instantiate(prefab, parent);
        }

        public async Task<ResourceLoot> CreateResourceLoot(ResourceTypeId resourceType, Vector3 at)
        {
            ResourceStaticData resource = _staticData.ForResource(resourceType);
            GameObject prefab = await _assets.Load<GameObject>(resource.PrefabReference);
            ResourceLoot lootPiece = Object.Instantiate(prefab, at, prefab.transform.rotation).GetComponent<ResourceLoot>();
            _container.InjectGameObject(lootPiece.gameObject);
            return lootPiece;
        }

        public async Task<EquipmentLoot> CreateEquipmentLoot(EquipmentTypeId equipmentType, Vector3 at)
        {
            EquipmentStaticData equipment = _staticData.ForEquipment(equipmentType);
            GameObject prefab = await _assets.Load<GameObject>(equipment.DropReference);
            EquipmentLoot lootPiece = Object.Instantiate(prefab, at, prefab.transform.rotation).GetComponent<EquipmentLoot>();
            _container.InjectGameObject(lootPiece.gameObject);
            return lootPiece;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HudPath);
            GameObject hud = Object.Instantiate(prefab);
            _container.InjectGameObject(hud);
            return hud;
        }

        public void Cleanup()
        {
            _assets.Cleanup();
        }

        public async Task CreateSpawner(string spawnerId, Vector3 at, EnemyTypeId enemyTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            EnemySpawner spawner = Object.Instantiate(prefab, at, Quaternion.identity).GetComponent<EnemySpawner>();

            _container.InjectGameObject(spawner.gameObject);
            spawner.Initialize(spawnerId, enemyTypeId);
        }

        public async Task<GameObject> CreateSaveTrigger(Vector3 at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.SaveTrigger);
            GameObject saveTrigger = Object.Instantiate(prefab, at, Quaternion.identity);
            _container.InjectGameObject(saveTrigger);
            return saveTrigger;
        }
    }
}