using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Ads;
using CodeBase.Services.StaticData;
using System.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;
        private readonly IAdsService _adsService;

        public BootstrapState(IGameStateMachine stateMachine, IStaticDataService staticData, IAssetProvider assetProvider, IAdsService adsService)
        {
            _stateMachine = stateMachine;
            _staticData = staticData;
            _assetProvider = assetProvider;
            _adsService = adsService;
        }

        public async void Enter()
        {
            await InitializeServices();

            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {

        }

        private async Task InitializeServices()
        {
            await _staticData.Initialize();
            _assetProvider.Initialize();
            _adsService.Initialize();
        }
    }
}