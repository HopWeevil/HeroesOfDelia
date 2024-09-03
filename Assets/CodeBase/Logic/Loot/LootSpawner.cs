using CodeBase.Enemy;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;
using DG.Tweening;
using CodeBase.SO;
using UnityEngine.UI;
using System.Threading.Tasks;
using CodeBase.Enums;
using System.Collections.Generic;
using System;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.Loot
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;

        private LootData _lootData;
        private IGameFactory _factory;
        private IRandomService _randomizer;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IGameFactory factory, IRandomService randomService, IStaticDataService staticDataService, IPersistentProgressService progressService)
        {
            _factory = factory;
            _randomizer = randomService;
            _staticDataService = staticDataService;
            _progressService = progressService;
        }

        public void SetLootData(LootData lootData)
        {
            _lootData = lootData;
        }

        private void Start()
        {
            EnemyDeath.Happened += SpawnLoot;
        }

        private async void SpawnLoot()
        {
            EnemyDeath.Happened -= SpawnLoot;

            await SpawnCoins(_lootData);
            await SpawnEquipments(_lootData);
        }

        private async Task SpawnCoins(LootData lootData)
        {
            int numberOfCoins = _randomizer.Next(lootData.CoinsAmount);

            for (int i = 0; i < numberOfCoins; i++)
            {
                ResourceLoot loot = await _factory.CreateResourceLoot(ResourceTypeId.Coin, transform.position);
                loot.Construct(ResourceTypeId.Coin, _progressService.Economy);
                PlayBurstEffect(loot.transform, 1.5f, 1.5f, 1f, 0.5f);
            }
        }

        private async Task SpawnEquipments(LootData lootData)
        {
            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
            {
                int dropChance = lootData.GetDropChanceForRarity(rarity);
                await TrySpawnEquipment(rarity, dropChance);
            }
        }

        private async Task TrySpawnEquipment(Rarity rarity, int lootChance)
        {
            if(lootChance > _randomizer.NextBetweenZeroAndHundred())
            {
                List<EquipmentStaticData> equipments = _staticDataService.GetEquipmentByRarity(rarity);
                EquipmentStaticData equipment = equipments[_randomizer.Next(0, equipments.Count)];

                EquipmentLoot loot = await _factory.CreateEquipmentLoot(equipment.TypeId, transform.position);
                loot.Initialize(equipment.TypeId, _progressService.Equipments);
                PlayBurstEffect(loot.transform, 1.5f, 1.5f, 1f, 0.5f);
            }
        }

        private void PlayBurstEffect(Transform target, float radius, float jumpPower, float height, float duration)
        {
            Vector3 jumpTarget = target.position + new Vector3(_randomizer.Next(-radius, radius), height, _randomizer.Next(-radius, radius));
            target.DOJump(jumpTarget, jumpPower, 1, duration).SetEase(Ease.Flash);
        }
    }
}