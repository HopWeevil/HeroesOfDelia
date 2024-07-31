using CodeBase.Enums;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface IShowcaseHeroFactory
    {
        event Action<GameObject> OnCreated;
        Task<GameObject> Create();
        Task<GameObject> Create(HeroTypeId typeId, Vector3 pos, Vector3 eluer);
        Task<GameObject> GetOrCreate();
    }
}
