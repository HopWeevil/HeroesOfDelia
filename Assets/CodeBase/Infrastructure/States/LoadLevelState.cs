using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Sceneloader;
using CodeBase.Logic;
using CodeBase.Logic.Camera;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LevelStaticData>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly ICharacterFactory _characterFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IUIFactory _uIFactory;

        private LevelStaticData _levelToLoad;

        public LoadLevelState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, ICharacterFactory factory, IPersistentProgressService progressService, IGameFactory gameFactory, IUIFactory uIFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _characterFactory = factory;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _uIFactory = uIFactory;
        }

        public void Enter(LevelStaticData levelStaticData)
        {
            _levelToLoad = levelStaticData;
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _sceneLoader.Load(_levelToLoad.LevelKey, OnLoadedAsync);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnLoadedAsync()
        {
            await InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWorld()
        {
            GameObject hero = await InitHero(_levelToLoad);
            await InitSpawners(_levelToLoad);
            await InitSaveTrigger(_levelToLoad);
            await InitHud(hero);
            CameraFollow(hero);
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData)
        {
            GameObject hero = await _characterFactory.CreateHero(levelData.InitialHeroPosition, _progressService.Progress.SelectedHero);
            return hero;
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _uIFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<IHealth>());
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.EnemyTypeId);
            }
        }

        private async Task InitSaveTrigger(LevelStaticData levelData)
        {
             await _gameFactory.CreateSaveTrigger(levelData.SaveTriggerMarker);
        }
    }
}