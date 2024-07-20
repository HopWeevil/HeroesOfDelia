using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface IGameFactory
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateSaveTrigger(Vector3 at);
        Task<GameObject> CreateMonster(EnemyTypeId monsterTypeId, Transform parent);
        Task CreateSpawner(string spawnerId, Vector3 at, EnemyTypeId enemyTypeId);
    }
}