using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Unity.Plastic.Newtonsoft.Json;
using CodeBase.Enums;
using System;
using CodeBase.Logic.EnemySpawners;

public class LevelDataEditor : EditorWindow
{
    private const string EditorScenePath = "Assets/ECR/Gameplay/Board/Editor/StageEditor.unity";
    private const string EnemySpawnMarkerPrefabPath = "Assets/SceneAssets/SpawnMarker.prefab";

    private Tilemap _tilemap;
    private Transform _playerSpawner;

    private string _output;
    private Vector2 _scroll;

    private string stageKey, stageTitle, stageDescription;
    private Vector3 playerSpawnPoint;
    private List<EnemySpawnerEditor> enemySpawners = new List<EnemySpawnerEditor>();
    private string jsonOutput;

    [MenuItem("Tools/Level editor")]
    private static void ShowWindow() =>
        GetWindow<LevelDataEditor>("Level editor").Show();

    private void OnEnable()
    {
        EditorSceneManager.activeSceneChangedInEditMode += ResetWindow;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        ResetWindow();
    }

    private void OnDestroy()
    {
        EditorSceneManager.activeSceneChangedInEditMode -= ResetWindow;
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        ResetWindow();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Stage properties", EditorStyles.boldLabel);
        stageKey = EditorGUILayout.TextField("Stage Key", stageKey);
        stageTitle = EditorGUILayout.TextField("Stage Title", stageTitle);
        stageDescription = EditorGUILayout.TextArea(stageDescription, GUILayout.Height(75));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Player spawn", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Player Spawner"))
        {
            Selection.SetActiveObjectWithContext(_playerSpawner, null);
        }
        EditorGUILayout.Vector3Field("Player Spawn Point", playerSpawnPoint);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Enemy spawners", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Enemy Spawner"))
        {
            var data = new EnemySpawnerEditor();
            OnAddMarker(new CollectionChangeInfo { ChangeType = CollectionChangeType.Add, Value = data }, data);
            enemySpawners.Add(data);
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select", EditorStyles.boldLabel, GUILayout.Width(50));
        EditorGUILayout.LabelField("Enemy Type", EditorStyles.boldLabel, GUILayout.Width(200));
        EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < enemySpawners.Count; i++)
        {
            var spawner = enemySpawners[i];
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select", GUILayout.Width(50)))
            {
                Selection.SetActiveObjectWithContext(spawner.gameObject, null);
            }

            var newEnemyType = (EnemyTypeId)EditorGUILayout.EnumPopup(spawner.enemyType, GUILayout.Width(200));
            if (newEnemyType != spawner.enemyType)
            {
                UpdateSpawnerType(spawner, newEnemyType);
            }

            spawner.gameObject.transform.position = EditorGUILayout.Vector3Field("", spawner.gameObject.transform.position);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                OnRemoveMarker(new CollectionChangeInfo { ChangeType = CollectionChangeType.RemoveIndex, Index = i }, enemySpawners);
                enemySpawners.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate stage JSON", GUILayout.Height(40)))
        {
            var staticData = GenerateStageStaticData();
            GenerateOutput(staticData);
        }

        if (!string.IsNullOrEmpty(_output))
        {
            EditorGUILayout.LabelField("Generated stage static data", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(jsonOutput, GUILayout.Height(150));
        }

        UpdateEnemySpawners();
    }

    private LevelStaticData GenerateStageStaticData()
    {
        var staticData = new LevelStaticData();

        /* staticData.StageKey = stageKey;
            staticData.StageTitle = stageTitle;
            staticData.StageDescription = stageDescription;

            staticData.PlayerSpawnPoint = _playerSpawner.position;
            staticData.BoardTiles = board;*/

        /*    staticData.EnemySpawners = enemySpawners
                .Select(x => new EnemySpawnerData(x.)
                {
                    EnemyType = x.enemyType,
                    Position = x.gameObject.transform.position
                })
                .ToArray();*/


        return staticData;
    }

    private void ResetWindow(Scene previous, Scene current) =>
        ResetWindow();

    private void ResetWindow()
    {
        if (SceneManager.GetActiveScene().name != "StageEditor")
            return;

        _tilemap = FindObjectOfType<Tilemap>();
        _playerSpawner = GameObject.FindGameObjectWithTag("Player").transform;
        enemySpawners.ForEach(sp => DestroyImmediate(sp.gameObject));
        enemySpawners.Clear();

        stageKey = stageTitle = stageDescription = string.Empty;
        _output = string.Empty;
        jsonOutput = _output;

        Repaint();
    }

    private void GenerateOutput<T>(T obj)
    {
        _output = JsonConvert.SerializeObject(obj, Formatting.Indented);
        jsonOutput = _output;
    }

    private void UpdatePlayerSpawner() =>
        playerSpawnPoint = _playerSpawner.position;

    private void OnAddMarker(CollectionChangeInfo info, object value)
    {
        if (info.ChangeType != CollectionChangeType.Add)
            return;

        var data = (EnemySpawnerEditor)info.Value;

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnemySpawnMarkerPrefabPath);
        var marker = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        data.gameObject = marker;
        UpdateSpawnerName(data);
    }

    private void OnRemoveMarker(CollectionChangeInfo info, object value)
    {
        if (info.ChangeType != CollectionChangeType.RemoveIndex)
            return;

        DestroyImmediate(((List<EnemySpawnerEditor>)value)[info.Index].gameObject);
    }

    private void UpdateEnemySpawners()
    {
        var currentMarkers = GameObject.FindGameObjectsWithTag("SpawnMarker")
            .Select(marker => new EnemySpawnerEditor
            {
                enemyType = marker.GetComponent<SpawnMarker>().EnemyTypeId, // Assuming there's a component that holds the enemy type
                gameObject = marker
            }).ToList();

        // Add new markers
        foreach (var marker in currentMarkers)
        {
            if (!enemySpawners.Any(es => es.gameObject == marker.gameObject))
            {
                enemySpawners.Add(marker);
                UpdateSpawnerName(marker);
            }
        }

        // Remove deleted markers
        for (int i = enemySpawners.Count - 1; i >= 0; i--)
        {
            if (!currentMarkers.Any(cm => cm.gameObject == enemySpawners[i].gameObject))
            {
                enemySpawners.RemoveAt(i);
            }
        }

        // Update existing markers
        foreach (var marker in currentMarkers)
        {
            var existingSpawner = enemySpawners.First(es => es.gameObject == marker.gameObject);
            if (existingSpawner.enemyType != marker.enemyType)
            {
                existingSpawner.enemyType = marker.enemyType;
                UpdateSpawnerName(existingSpawner);
            }
        }
    }

    private void UpdateSpawnerType(EnemySpawnerEditor spawner, EnemyTypeId newType)
    {
        if (spawner.gameObject != null)
        {
            spawner.enemyType = newType;
            UpdateSpawnerName(spawner);
            // Update the scene object as well
            var spawnMarker = spawner.gameObject.GetComponent<SpawnMarker>();
            if (spawnMarker != null)
            {
                spawnMarker.EnemyTypeId = newType;
            }
        }
    }

    private void UpdateSpawnerName(EnemySpawnerEditor spawner)
    {
        if (spawner.gameObject != null)
        {
            spawner.gameObject.name = $"Spawner : {spawner.enemyType}";
        }
    }

    private void OnHierarchyChanged()
    {
        UpdateEnemySpawners();
        Repaint();
    }

    private class CollectionChangeInfo
    {
        public CollectionChangeType ChangeType { get; set; }
        public int Index { get; set; }
        public object Value { get; set; }
    }

    private enum CollectionChangeType
    {
        Add,
        RemoveIndex
    }

    [Serializable]
    private class EnemySpawnerEditor
    {
        public EnemyTypeId enemyType;
        public GameObject gameObject;
    }
}

