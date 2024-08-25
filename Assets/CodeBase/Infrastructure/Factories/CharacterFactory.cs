using CodeBase.Services.StaticData;
using UnityEngine;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
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
using System.Linq;
using CodeBase.Data;

namespace CodeBase.Infrastructure.Factories
{
    public class CharacterFactory : ICharacterFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly DiContainer _container;

        private GameObject _hero;

        public CharacterFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticDataService, IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _staticData = staticDataService;
            _progressService = persistentProgressService;
            _container = container;
        }

        public async Task WarmUp()
        {
            foreach (HeroStaticData data in _staticData.GetAllHeroes())
            {
                await _assets.Load<GameObject>(data.PrefabReference);
            }
        }

        public void CleanUp()
        {
            foreach (HeroStaticData data in _staticData.GetAllHeroes())
            {
                _assets.Release(data.PrefabReference);
            }
        }

        public async Task<GameObject> CreateHero(Vector3 at, HeroTypeId heroTypeId = HeroTypeId.Knight)
        {
            HeroStaticData data = _staticData.ForHero(heroTypeId);
            GameObject prefab = await _assets.Load<GameObject>(data.PrefabReference);

            _hero = Object.Instantiate(prefab, at, Quaternion.identity);
            _container.InjectGameObject(_hero);

            Stats stats = new Stats(data.Hp, data.Damage, data.MoveSpeed, data.Armor, data.AttackCooldown, data.Cleavage, data.EffectiveDistance);

            if (_progressService.Equipments.HeroesEquipment.TryGetValue(data.TypeId, out var equipments))
            {
                foreach (var item in equipments)
                {
                    StatsBonus[] bonuses = _staticData.ForEquipment(item.Value.EquipmentTypeId).Bonuses.ToArray();
                    stats.ApplyStatsBonuses(bonuses, item.Value.Level);
                }
            }

            _hero.GetComponentsInChildren<IStatsReceiver>().ToList().ForEach(receiver => receiver.Receive(stats));
            await _hero.GetComponent<HeroEquipper>().TryEquip(heroTypeId);
            return _hero;
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData data = _staticData.ForEnemy(enemyTypeId);

            GameObject prefab = await _assets.Load<GameObject>(data.PrefabReference);
            GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
            _container.InjectGameObject(enemy.gameObject);

            IHealth health = enemy.GetComponent<IHealth>();
            enemy.GetComponent<ActorUI>().Construct(health);

            EnemyStateMachine stateMachine = enemy.GetComponentInChildren<EnemyStateMachine>();
            stateMachine.Construct(_hero);


            enemy.GetComponent<RotateToHero>()?.Construct(_hero.transform);

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLootData(data.LootData);

            Stats stats = new Stats(data.Hp, data.Damage, data.MoveSpeed, data.Armor, data.AttackCooldown, data.Cleavage, data.EffectiveDistance);

            enemy.GetComponentsInChildren<IStatsReceiver>().ToList().ForEach(receiver => receiver.Receive(stats));

            return enemy;
        }
    }
}