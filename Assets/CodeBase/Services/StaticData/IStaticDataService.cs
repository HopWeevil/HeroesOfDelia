using CodeBase.Enums;
using CodeBase.SO;
using System.Collections.Generic;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        EnemyStaticData ForEnemy(EnemyTypeId id);
        LevelStaticData ForLevel(string sceneKey);
        EquipmentStaticData ForEquipment(EquipmentTypeId id);
        ResourceStaticData ForResource(ResourceTypeId id);
        List<EquipmentStaticData> GetEquipmentByRarity(Rarity rarity);
        HeroStaticData ForHero(HeroTypeId id);
        List<LevelStaticData> GetAllLevels();
    }
}