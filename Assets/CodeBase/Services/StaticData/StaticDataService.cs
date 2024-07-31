using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Logic.Loot;
using CodeBase.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroesDataPath = "StaticData/Heroes";
        private const string EnemiesDataPath = "StaticData/Enemies";
        private const string LevelsDataPath = "StaticData/Levels";
        private const string EquipmentDataPath = "StaticData/Equipment";
        private const string ResourceDataPath = "StaticData/Resource";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<HeroTypeId, HeroStaticData> _heroes;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<EquipmentTypeId, EquipmentStaticData> _equipment;
        private Dictionary<ResourceTypeId, ResourceStaticData> _resources;

        public void Load()
        {
            _heroes = Resources.LoadAll<HeroStaticData>(HeroesDataPath).ToDictionary(x => x.HeroTypeId, x => x);
            _enemies = Resources.LoadAll<EnemyStaticData>(EnemiesDataPath).ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath).ToDictionary(x => x.LevelKey, x => x);
            _equipment = Resources.LoadAll<EquipmentStaticData>(EquipmentDataPath).ToDictionary(x => x.EquipmentTypeId, x => x);
            _resources = Resources.LoadAll<ResourceStaticData>(ResourceDataPath).ToDictionary(x => x.ResourceTypeId, x => x);
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            if (_levels.TryGetValue(sceneKey, out LevelStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }
        public HeroStaticData ForHero(HeroTypeId id)
        {
            if (_heroes.TryGetValue(id, out HeroStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public EnemyStaticData ForEnemy(EnemyTypeId id)
        {
            if (_enemies.TryGetValue(id, out EnemyStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public EquipmentStaticData ForEquipment(EquipmentTypeId id)
        {
            if (_equipment.TryGetValue(id, out EquipmentStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }
        public List<EquipmentStaticData> GetEquipmentByRarity(Rarity rarity)
        {
            return _equipment.Values.Where(e => e.Rarity == rarity).ToList();
        }

        public ResourceStaticData ForResource(ResourceTypeId id)
        {
            if (_resources.TryGetValue(id, out ResourceStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }
    }
}