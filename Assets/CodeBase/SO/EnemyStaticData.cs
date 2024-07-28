using CodeBase.Enums;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static Data/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        [Range(1, 100)]
        public int Hp = 50;

        [Range(1, 30)]
        public float Damage = 10;

        [Range(0.2f, 3)]
        public float AttackCooldown = 1;

        [Range(0.5f, 1)]
        public float EffectiveDistance = .5f;

        [Range(0.5f, 1)]
        public float Cleavage = 0.5f;

        [Range(0, 10)]
        public float MoveSpeed = 3;

        [Range(1, 100)]
        public int MaxGold;

        [Range(1, 100)]
        public int MinGold;

        public AssetReferenceGameObject PrefabReference;

        public LootData LootData;
    }

    [System.Serializable]
    public class LootData
    {
        public MinMaxRange CoinsAmount;
        public int CommonDropChance;
        public int RareDropChance;
        public int EpicDropChance;
        public int LegendaryDropChance;
        public int MythicDropChance;

        public int GetDropChanceForRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => CommonDropChance,
                Rarity.Rare => RareDropChance,
                Rarity.Epic => EpicDropChance,
                Rarity.Legendary => LegendaryDropChance,
                Rarity.Mythic => MythicDropChance,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
    }

    [System.Serializable]
    public struct MinMaxRange
    {
        [Range(1, 100)]
        public int MinValue;
        [Range(1, 100)]
        public int MaxValue;
    }
}