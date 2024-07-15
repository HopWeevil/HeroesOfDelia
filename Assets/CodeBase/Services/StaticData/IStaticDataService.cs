using CodeBase.Enums;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadMonsters();
        EnemyStaticData ForEnemy(EnemyTypeId id);
        LevelStaticData ForLevel(string sceneKey);
    }
}