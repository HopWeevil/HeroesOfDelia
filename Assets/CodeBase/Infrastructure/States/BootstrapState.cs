using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Ads;
using CodeBase.Services.IAP;
using CodeBase.Services.StaticData;
using System.Threading.Tasks;
using Unity.Services.Core;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;

        public BootstrapState(IGameStateMachine stateMachine, IStaticDataService staticData, IAssetProvider assetProvider, IAdsService adsService, IIAPService iapService)
        {
            _stateMachine = stateMachine;
            _staticData = staticData;
            _assetProvider = assetProvider;
            _adsService = adsService;
            _iapService = iapService;
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
            await UnityServices.InitializeAsync(new InitializationOptions());
            await _staticData.Initialize();
            _assetProvider.Initialize();
            _adsService.Initialize();
            _iapService.Initialize();
        }
    }
}