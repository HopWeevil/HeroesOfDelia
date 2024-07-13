using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Sceneloader;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public LoadLevelState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoadedAsync);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnLoadedAsync()
        {
            await InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private async Task InitGameWorld()
        {
            GameObject hero = await InitHero();
            _gameFactory.CreateSpawners();
            await InitHud(hero);
            CameraFollow(hero);
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }

        private async Task<GameObject> InitHero()
        {
            GameObject hero = await _gameFactory.CreateHero(new Vector3(0,0,0));
            return hero;
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();
        }
    }
}