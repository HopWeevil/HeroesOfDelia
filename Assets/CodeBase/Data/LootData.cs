using CodeBase.Enums;
using System;

namespace CodeBase.SO
{
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
}