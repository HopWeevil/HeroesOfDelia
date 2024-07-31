using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class ShowcaseHeroFactory : IShowcaseHeroFactory
    {
        private GameObject _currentHero;

        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;

        public event Action<GameObject> OnCreated;

        [Inject]
        private ShowcaseHeroFactory(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
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
            _currentHero = await _gameFactory.CreateNonPlayableHero(_progressService.Progress.SelectedHero, Vector3.zero, new Vector3(0, 180, 0));
            OnCreated?.Invoke(_currentHero);
            return _currentHero;
        }

        public async Task<GameObject> Create(HeroTypeId typeId, Vector3 pos, Vector3 eluer)
        {
            _currentHero = await _gameFactory.CreateNonPlayableHero(typeId, pos, eluer);
            OnCreated?.Invoke(_currentHero);
            return _currentHero;
        }
    }
}