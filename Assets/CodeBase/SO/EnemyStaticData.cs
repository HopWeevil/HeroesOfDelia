using CodeBase.Enums;
using System.Collections.Generic;
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