using CodeBase.Logic.EnemySpawner;
using UnityEngine.SceneManagement; 
using System.Collections.Generic;
using CodeBase.SO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string HeroInitialPoint = "HeroInitialPoint";
        private const string LevelTransferInitialPoint = "LevelTransferInitialPoint";
        private const string SaveTriggerMarker = "SaveTriggerMarker";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                SpawnMarker[] spawnMarkers = FindObjectsOfType<SpawnMarker>();

                List<EnemySpawnerData> spawnersList = spawnMarkers.Select(x => new EnemySpawnerData(x.EnemyTypeId, x.transform.position)).ToList();
                levelData.EnemySpawners = spawnersList;

                levelData.LevelKey = SceneManager.GetActiveScene().name;

                levelData.InitialHeroPosition = GameObject.Find(HeroInitialPoint).transform.position;

                levelData.LevelTransfer.Position = GameObject.Find(LevelTransferInitialPoint).transform.position;

                levelData.SaveTriggerMarker = GameObject.Find(SaveTriggerMarker).transform.position;
            }

            EditorUtility.SetDirty(target);
        }
    }
}