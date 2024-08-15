
using CodeBase.Data;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;
        private readonly ILoadingCurtain _loadingCurtain;

        public LoadProgressState(IGameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress, ILoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            //_loadingCurtain.Show();
            LoadProgressOrInitNew();
      
            //_gameStateMachine.Enter<LoadLevelState, string>("Location1");
            _gameStateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {
            //_loadingCurtain.Hide();
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
           // inventory.EquipHero(Enums.HeroTypeId.Mage, new EquipmentItem(Enums.EquipmentTypeId.SkeletonStaff), Enums.EquipmentCategory.Weapon);
            return inventory;
        }

        private PlayerEconomyData NewEconomy()
        {
            var progress = new PlayerEconomyData();
            progress.AddHeroItem(Enums.HeroTypeId.Knight);
           // progress.AddHeroItem(Enums.HeroTypeId.Mage);
            progress.IncreaseResourceAmount(Enums.ResourceTypeId.Coin, 5000);
            progress.IncreaseResourceAmount(Enums.ResourceTypeId.Gem, 2000);           
            return progress;
        }
    }
}