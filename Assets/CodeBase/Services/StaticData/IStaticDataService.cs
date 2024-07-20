using CodeBase.Enums;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        EnemyStaticData ForEnemy(EnemyTypeId id);
        LevelStaticData ForLevel(string sceneKey);
    }
}