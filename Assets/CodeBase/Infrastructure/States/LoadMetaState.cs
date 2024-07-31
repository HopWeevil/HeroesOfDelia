using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Sceneloader;
using CodeBase.Logic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IGameStateMachine _stateMachine;
        private readonly ILoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IShowcaseHeroFactory _heroCreator;

        public LoadMetaState(IUIFactory factory, ISceneLoader sceneLoader, IAssetProvider assetProvider, IGameStateMachine stateMachine, ILoadingCurtain curtain, IGameFactory gameFactory, IShowcaseHeroFactory heroCreator)
        {
            _uiFactory = factory;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _heroCreator = heroCreator;
        }

        public async void Enter()
        {
            _curtain.Show();
            _sceneLoader.Load("Meta", OnLoadedAsync);

            await _assetProvider.Load<GameObject>("Knight");
            await _assetProvider.Load<GameObject>("Barbarian");
            await _assetProvider.Load<GameObject>("Rogue");
            await _assetProvider.Load<GameObject>("Mage");
        }
        public void Exit()
        {
            
        }

        private async void OnLoadedAsync()
        {
            await InitUIRoot();
            GameObject menu = await InitMainMenu();
            await InitHeroSpinner(menu);

            _curtain.Hide();
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private async Task<GameObject> InitMainMenu()
        {
            return await _uiFactory.CreateMainMenu();
        }

        private async Task InitHeroSpinner(GameObject menu)
        {
            Debug.Log("Crete");
            await _heroCreator.Create();
        }
    }
}