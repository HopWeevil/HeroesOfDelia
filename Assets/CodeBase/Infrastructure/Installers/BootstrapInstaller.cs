using UnityEngine;
using Zenject;
using CodeBase.Services.Input;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.Services.Randomizer;
using CodeBase.Infrastructure.Sceneloader;
using CodeBase.Infrastructure.Factories;
using CodeBase.Logic;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _coroutineRunnerPrefab;
        [SerializeField] private GameObject _loadingScreenPrebab;

        public override void InstallBindings()
        {
            BindLoadingCurtain();
            BindCoroutineRunner();
            BindStateMachine();
            BindStatesFactory();
            BindInputService();
            BindPersistentProgressService();
            BindSaveLoadService();
            BindAssetProvider();
            BindStaticDataService();
            BindBindRandomizeService();
            BindHeroShowcaseFactory();
            BindGameFactory();
            BindUIFactory();
            BindSceneLoader();
        }

        private void BindSceneLoader()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }

        private void BindBindRandomizeService()
        {
            Container.Bind<IRandomService>().To<RandomService>().AsSingle();
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            Container.BindInstance(staticDataService).AsSingle();
        }

        private void BindAssetProvider()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }

        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
        }

        private void BindUIFactory()
        {
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
        private void BindHeroShowcaseFactory()
        {
            Container.BindInterfacesAndSelfTo<ShowcaseHeroFactory>().AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }

        private void BindStatesFactory()
        {
            Container.BindInterfacesAndSelfTo<StatesFactory>().AsSingle();
        }
        private void BindLoadingCurtain()
        {
            Container.Bind<ILoadingCurtain>().To<LoadingCurtain>().FromComponentInNewPrefab(_loadingScreenPrebab).AsSingle().NonLazy();
        }

        private void BindCoroutineRunner()
        {
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunnerPrefab).AsSingle().NonLazy();
        }

        private void BindInputService()
        {
            if (Application.isEditor)
            {
                Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            }
            else
            {
                Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
            }
        }
    }
}