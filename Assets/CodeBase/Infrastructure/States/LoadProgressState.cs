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

            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.SortSword));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.LongSword));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.KingSword));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.LumberjackAxe));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.BattleAxe));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.WarAxe));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.VikingHelmet));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.SkeletonStaff));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.SkeletonCrossbow));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.HeavyCrossbow));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.WizzardStaff));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.BootsOfSpeed));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.FrostRing));
            inventory.AddInventoryItem(new EquipmentItem(Enums.EquipmentTypeId.UltimateRing));
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