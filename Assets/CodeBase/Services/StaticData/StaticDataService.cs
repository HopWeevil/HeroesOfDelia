using CodeBase.Enums;
using CodeBase.SO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroLabel = "HeroStaticData";
        private const string EnemyLabel = "EnemyStaticData";
        private const string LevelLabel = "LevelStaticData";
        private const string EquipmentLabel = "EquipmentStaticData";
        private const string ResourceLabel = "ResourceStaticData";
        private const string ResourceRewardLabel = "ResourceRewardStaticData";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<HeroTypeId, HeroStaticData> _heroes;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<EquipmentTypeId, EquipmentStaticData> _equipment;
        private Dictionary<ResourceTypeId, ResourceStaticData> _resources;
        private Dictionary<ResourceTypeId, ResourceRewardStaticData> _rewardItems;

        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task Initialize()
        {
            _heroes = (await _assetProvider.LoadAll<HeroStaticData>(HeroLabel)).ToDictionary(x => x.TypeId, x => x);
            _enemies = (await _assetProvider.LoadAll<EnemyStaticData>(EnemyLabel)).ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = (await _assetProvider.LoadAll<LevelStaticData>(LevelLabel)).ToDictionary(x => x.LevelKey, x => x);
            _equipment = (await _assetProvider.LoadAll<EquipmentStaticData>(EquipmentLabel)).ToDictionary(x => x.TypeId, x => x);
            _resources = (await _assetProvider.LoadAll<ResourceStaticData>(ResourceLabel)).ToDictionary(x => x.ResourceTypeId, x => x);
            _rewardItems = (await _assetProvider.LoadAll<ResourceRewardStaticData>(ResourceRewardLabel)).ToDictionary(x => x.ResourceType, x => x);
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

        public List<EquipmentStaticData> GetEquipmentByRarity(Rarity rarity)
        {
            return _equipment.Values.Where(e => e.Rarity == rarity).ToList();
        }

        public List<LevelStaticData> GetAllLevels()
        {
            return _levels.Values.ToList();
        }

        public List<ResourceRewardStaticData> GetAllRewardItems()
        {
            return _rewardItems.Values.ToList();
        }

        public List<HeroStaticData> GetAllHeroes()
        {
            return _heroes.Values.ToList();
        }
    }
}