using CodeBase.Services.StaticData;
using System.Collections.Generic;
using UnityEngine;
using CodeBase.Infrastructure.States;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Randomizer;
using CodeBase.Services.PersistentProgress;
using Zenject;
using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Enemy.StateMachine;
using CodeBase.UI;
using CodeBase.Logic;
using CodeBase.Logic.Loot;
using CodeBase.SO;
using CodeBase.Hero;

namespace CodeBase.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticDataService;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IGameStateMachine _stateMachine;
        private readonly DiContainer _container;

        private GameObject _hero;
   
        public GameFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticDataService, IRandomService randomService, IPersistentProgressService persistentProgressService, IGameStateMachine stateMachine)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _randomService = randomService;
            _progressService = persistentProgressService;
            _stateMachine = stateMachine;
            _container = container;

        }

        public async Task<GameObject> CreateHero(Vector3 at, HeroTypeId heroTypeId = HeroTypeId.Knight)
        {
            HeroStaticData heroData = _staticDataService.ForHero(heroTypeId);
           

            GameObject prefab = await _assets.Load<GameObject>(heroData.PrefabReference);
            _hero = Object.Instantiate(prefab, at, Quaternion.identity);
            _container.InjectGameObject(_hero);
           
            IHealth health = _hero.GetComponent<IHealth>();
            health.Current = heroData.Hp;
            health.Max = heroData.Hp;
            Debug.Log(health);
            // hero.GetComponent<ActorUI>().Construct(health);

            _hero.GetComponent<HeroMover>().SetStats(heroData.MoveSpeed);

            IAttack attack = _hero.GetComponent<IAttack>();
            attack.InitializeStats(heroData);


            return _hero;
        }

        public async Task<GameObject> CreateEquipment(EquipmentTypeId equipmentType, Transform parent)
        {
            EquipmentStaticData equipment = _staticDataService.ForEquipment(equipmentType);
            GameObject prefab = await _assets.Load<GameObject>(equipment.PrefabReference);
            return Object.Instantiate(prefab, parent);
        }

        public async Task<GameObject> CreateNonPlayableHero(HeroTypeId heroTypeId, Vector3 at, Vector3 eulers)
        {
            HeroStaticData heroData = _staticDataService.ForHero(heroTypeId);
            GameObject prefab = await _assets.Load<GameObject>(heroData.PrefabReference);
            GameObject hero = _container.InstantiatePrefab(prefab);
            hero.transform.position = at;
            hero.transform.Rotate(eulers);
            hero.GetComponent<HeroMover>().enabled = false;
            hero.GetComponent<HeroAttack>().enabled = false;
            return hero;
        }

        public async Task<ResourceLoot> CreateResourceLoot(ResourceTypeId resourceType, Vector3 at)
        {
            ResourceStaticData resource = _staticDataService.ForResource(resourceType);
            GameObject prefab = await _assets.Load<GameObject>(resource.PrefabReference);
            ResourceLoot lootPiece = Object.Instantiate(prefab, at, prefab.transform.rotation).GetComponent<ResourceLoot>();
            _container.InjectGameObject(lootPiece.gameObject);
            return lootPiece;
        }

        public async Task<EquipmentLoot> CreateEquipmentLoot(EquipmentTypeId equipmentType, Vector3 at)
        {
            EquipmentStaticData equipment = _staticDataService.ForEquipment(equipmentType);
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

        public async Task<GameObject> CreateMonster(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticDataService.ForEnemy(enemyTypeId);

            GameObject prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);
            GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            IHealth health = enemy.GetComponent<IHealth>();
            health.Current = enemyData.Hp;
            health.Max = enemyData.Hp;

            enemy.GetComponent<ActorUI>().Construct(health);

            EnemyStateMachine stateMachine = enemy.GetComponentInChildren<EnemyStateMachine>();
            stateMachine.Construct(_hero);

            enemy.GetComponent<EnemyMover>().SetStats(enemyData.MoveSpeed);

            IAttack attack = enemy.GetComponent<IAttack>();
            attack.InitializeStats(enemyData);

            enemy.GetComponent<RotateToHero>()?.Construct(_hero.transform);

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.Initialize(enemyData.LootData);

            _container.InjectGameObject(enemy.gameObject);

            return enemy;
        }
    }
}