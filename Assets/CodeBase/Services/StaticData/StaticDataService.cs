using CodeBase.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string Enemies = "StaticData/Enemies";
        private const string LevelsDataPath = "StaticData/Levels";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;

        public void Load()
        {
            _enemies = Resources.LoadAll<EnemyStaticData>(Enemies).ToDictionary(x => x.EnemyTypeId, x => x);
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath).ToDictionary(x => x.LevelKey, x => x);  
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
    }
}