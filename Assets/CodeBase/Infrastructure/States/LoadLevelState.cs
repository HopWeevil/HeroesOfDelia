﻿using CodeBase.Enums;
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
            _staticDataService = staticDataService;
        }

        public void Enter(string sceneName)
        {
            Debug.Log("LoadLevelState");
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
            LevelStaticData levelData = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);

            GameObject hero = await InitHero(levelData);
            await InitSpawners(levelData);
            await InitSaveTrigger(levelData);
            await InitHud(hero);
            CameraFollow(hero);
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData)
        {
            GameObject hero = await _gameFactory.CreateHero(levelData.InitialHeroPosition, HeroTypeId.Knight);
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