using CodeBase.Enums;
using CodeBase.SO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        Task Initialize();
        EnemyStaticData ForEnemy(EnemyTypeId id);
        LevelStaticData ForLevel(string sceneKey);
        EquipmentStaticData ForEquipment(EquipmentTypeId id);
        ResourceStaticData ForResource(ResourceTypeId id);
        HeroStaticData ForHero(HeroTypeId id);
        List<EquipmentStaticData> GetEquipmentByRarity(Rarity rarity);
        List<LevelStaticData> GetAllLevels();
        List<HeroStaticData> GetAllHeroes();
        List<ResourceRewardStaticData> GetAllRewardItems();
    }
}