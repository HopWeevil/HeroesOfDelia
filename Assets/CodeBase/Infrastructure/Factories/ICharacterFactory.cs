using CodeBase.Enums;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface ICharacterFactory
    {
        Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        Task<GameObject> CreateHero(Vector3 at, HeroTypeId heroTypeId = HeroTypeId.Knight);
    }
}