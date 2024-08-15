using CodeBase.Enums;
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
using UnityEngine.SceneManagement;

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
        private readonly IStaticDataService _staticDataService;

        private const string sceneName = "Location1";
        private LevelStaticData _levelToLoad;

        public LoadLevelState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain, ICharacterFactory factory, IPersistentProgressService progressService, IStaticDataService staticDataService, IGameFactory gameFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _characterFactory = factory;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;
        }

        public void Enter(LevelStaticData levelStaticData)
        {
            _levelToLoad = levelStaticData;
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

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWorld()
        {
            //LevelStaticData levelData = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);

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
            GameObject hud = await _gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<IHealth>());
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                Debug.Log(spawnerData.Id);
                await _gameFactory.CreateSpawner(spawnerData.Id, spawnerData.Position, spawnerData.EnemyTypeId);
            }
        }

        private async Task InitSaveTrigger(LevelStaticData levelData)
        {
             await _gameFactory.CreateSaveTrigger(levelData.SaveTriggerMarker);
        }
    }
}