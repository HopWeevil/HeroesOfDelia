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

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
   
        public GameFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticDataService, IRandomService randomService, IPersistentProgressService persistentProgressService, IGameStateMachine stateMachine)
        {
            _assets = assets;
            _staticDataService = staticDataService;
            _randomService = randomService;
            _progressService = persistentProgressService;
            _stateMachine = stateMachine;
            _container = container;

        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Knight);
            _hero = InstantiateRegistered(prefab, at);
            _container.InjectGameObject(_hero);
            return _hero;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
            return hud;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _assets.Cleanup();
        }

        public async Task CreateSpawner(string spawnerId, Vector3 at, EnemyTypeId enemyTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            EnemySpawner spawner = InstantiateRegistered(prefab, at).GetComponent<EnemySpawner>();

            _container.InjectGameObject(spawner.gameObject);
            spawner.Initialize(spawnerId, enemyTypeId);
        }

        public async Task<GameObject> CreateSaveTrigger(Vector3 at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.SaveTrigger);
            GameObject saveTrigger = InstantiateRegistered(prefab, at);
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

            /*LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLootValue(monsterData.MinLoot, monsterData.MaxLoot);*/

            return enemy;
        }


        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}