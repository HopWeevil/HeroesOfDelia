using CodeBase.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string Title;
        public string Description;

        public string LevelKey;

        public List<EnemySpawnerData> EnemySpawners;

        public Vector3 InitialHeroPosition;
        public LevelTransferData LevelTransfer;
        public Vector3 SaveTriggerMarker;
    }
}