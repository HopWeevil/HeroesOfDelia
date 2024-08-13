using CodeBase.Enums;
using CodeBase.Hero;
using CodeBase.Services.PersistentProgress;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class ShowcaseHeroFactory : IShowcaseHeroFactory
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ICharacterFactory _characterFactory;
        private readonly DiContainer _container;

        private GameObject _currentHero;

        public event Action<GameObject> OnCreated;

        private ShowcaseHeroFactory(DiContainer container, IPersistentProgressService progressService, ICharacterFactory gameFactory)
        {
            _container = container;
            _progressService = progressService;
            _characterFactory = gameFactory;
        }

        public async Task<GameObject> GetOrCreate()
        {
            if (_currentHero != null)
            {
                return _currentHero;
            }
            else
            {
                return await Create();
            }
        }

        public async Task<GameObject> Create()
        {
            _currentHero = await CreateNonPlayableHero(_progressService.Progress.SelectedHero, Vector3.zero, new Vector3(0, 180, 0));
            OnCreated?.Invoke(_currentHero);
            return _currentHero;
        }

        public async Task<GameObject> Create(HeroTypeId typeId, Vector3 pos, Vector3 eluer)
        {
            _currentHero = await CreateNonPlayableHero(typeId, pos, eluer);
            OnCreated?.Invoke(_currentHero);
            return _currentHero;
        }

        public async Task<GameObject> CreateNonPlayableHero(HeroTypeId heroTypeId, Vector3 at, Vector3 eulers)
        {
            GameObject hero = await _characterFactory.CreateHero(at, heroTypeId);
            hero.transform.position = at;
            hero.transform.Rotate(eulers);
            hero.GetComponent<HeroMover>().enabled = false;
            hero.GetComponent<HeroAttack>().enabled = false;
            return hero;
        }
    }
}