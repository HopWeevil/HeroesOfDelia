using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;

        public BootstrapState(IGameStateMachine stateMachine, IStaticDataService staticData, IAssetProvider assetProvider)
        {
            _stateMachine = stateMachine;
            _staticData = staticData;
            _assetProvider = assetProvider;
        }

        public void Enter()
        {
            InitializeServices();

            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {

        }

        private void InitializeServices()
        {
            _staticData.Initialize();
            _assetProvider.Initialize();
        }
    }
}