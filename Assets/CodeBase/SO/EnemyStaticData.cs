using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static Data/Enemy")]
    public class EnemyStaticData : CharacterStaticData
    {
        public LootData LootData;

        public EnemyTypeId EnemyTypeId;
    }
}