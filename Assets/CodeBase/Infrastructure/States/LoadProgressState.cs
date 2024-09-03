using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Data;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        private readonly ILoadingCurtain _loadingCurtain;

        public LoadProgressState(IGameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
      
            _gameStateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {

        }

        private void LoadProgressOrInitNew()
        {
            //_progressService.Progress = _saveLoadProgress.LoadProgress() ?? NewProgress();
            _progressService.Progress =  NewProgress();
            _progressService.Economy = NewEconomy();
            _progressService.Equipments = NewInventory();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress();
            return progress;
        }

        private PlayerEquipment NewInventory()
        {
            var inventory = new PlayerEquipment();
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.Sword));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.Sword));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.Axe));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.SkeletonCrossbow));      
            return inventory;
        }

        private PlayerEconomyData NewEconomy()
        {
            var progress = new PlayerEconomyData();
            progress.AddHeroItem(Enums.HeroTypeId.Knight);
            progress.IncreaseResourceAmount(Enums.ResourceTypeId.Coin, 5000);
            progress.IncreaseResourceAmount(Enums.ResourceTypeId.Gem, 2000);           
            return progress;
        }
    }
}