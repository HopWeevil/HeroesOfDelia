using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeBase.Enums;
using CodeBase.SO;
using CodeBase.Logic.EnemySpawners;

public class LevelDataEditor : EditorWindow
{
    private const string EnemySpawnMarkerPrefabPath = "Assets/SceneAssets/SpawnMarker.prefab";

    private Transform _playerSpawner;
    private string stageKey, stageTitle, stageDescription;
    private Vector3 playerSpawnPoint;
    private List<EnemySpawnerEditor> enemySpawners = new List<EnemySpawnerEditor>();

    private List<LevelStaticData> levels = new List<LevelStaticData>();
    private LevelStaticData selectedLevel;

    [MenuItem("Tools/Level editor")]
    private static void ShowWindow() => GetWindow<LevelDataEditor>("Level editor").Show();

    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        LoadAllLevels();
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
    }

    private void OnGUI()
    {
        DrawLevelSelection();
        if (selectedLevel != null)
        {
            DrawStageProperties();
            DrawPlayerSpawnerSection();
            DrawEnemySpawnersSection();

            if (GUILayout.Button("Save Level Data"))
            {
                SaveLevelData();
            }

            UpdateEnemySpawners();
        }
    }

    private void ShowLoadLevelConfirmation(LevelStaticData level)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            if (EditorUtility.DisplayDialog(
                "Confirm Load Level",
                $"Are you sure you want to load the level \"{level.Title}\"? This will switch to the corresponding scene.",
                "Yes",
                "No"))
            {
                LoadLevel(level);
                LoadLevelData(level);
            }
        }
    }

    private void LoadLevel(LevelStaticData level)
    {
        string scenePath = $"Assets/Scenes/{level.LevelKey}.unity";

        if (System.IO.File.Exists(scenePath))
        {
            EditorSceneManager.OpenScene(scenePath);
            selectedLevel = level;
        }
        else
        {
            EditorUtility.DisplayDialog("Error", $"The scene \"{scenePath}\" does not exist.", "OK");
        }
    }

    private void DrawLevelSelection()
    {
        EditorGUILayout.LabelField("Select Level to Edit", EditorStyles.boldLabel);
        foreach (var level in levels)
        {
            if (GUILayout.Button(level.Title))
            {
                ShowLoadLevelConfirmation(level);
            }
        }
        EditorGUILayout.Space();
    }

    private void DrawStageProperties()
    {
        EditorGUILayout.LabelField("Stage properties", EditorStyles.boldLabel);
        stageKey = EditorGUILayout.TextField("Stage Key", stageKey);
        stageTitle = EditorGUILayout.TextField("Stage Title", stageTitle);
        stageDescription = EditorGUILayout.TextArea(stageDescription, GUILayout.Height(75));
        EditorGUILayout.Space();
    }

    private void DrawPlayerSpawnerSection()
    {
        EditorGUILayout.LabelField("Player spawn", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Player Spawner"))
        {
            Selection.SetActiveObjectWithContext(_playerSpawner, null);
        }
        EditorGUILayout.Vector3Field("Player Spawn Point", playerSpawnPoint);
        EditorGUILayout.Space();
    }

    private void DrawEnemySpawnersSection()
    {
        EditorGUILayout.LabelField("Enemy spawners", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Enemy Spawner"))
        {
            AddEnemySpawner();
        }
        EditorGUILayout.Space();
        DrawEnemySpawnerList();
    }

    private void DrawEnemySpawnerList()
    {
        EditorGUILayout.BeginVertical();
        DrawEnemySpawnerListHeader();

        foreach (var spawner in enemySpawners)
        {
            if (spawner?.GameObject == null) continue;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select", GUILayout.Width(50)))
            {
                Selection.SetActiveObjectWithContext(spawner.GameObject, null);
            }

            var newEnemyType = (EnemyTypeId)EditorGUILayout.EnumPopup(spawner.EnemyType, GUILayout.Width(200));
            if (newEnemyType != spawner.EnemyType)
            {
                UpdateSpawnerType(spawner, newEnemyType);
            }

            var previousPosition = spawner.GameObject.transform.position;

            spawner.GameObject.transform.position = EditorGUILayout.Vector3Field("", spawner.GameObject.transform.position);

            if (previousPosition != spawner.GameObject.transform.position)
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveEnemySpawner(enemySpawners.IndexOf(spawner));
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }


    private void DrawEnemySpawnerListHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select", EditorStyles.boldLabel, GUILayout.Width(50));
        EditorGUILayout.LabelField("Enemy Type", EditorStyles.boldLabel, GUILayout.Width(200));
        EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
    }

    private void LoadAllLevels()
    {
        levels = AssetDatabase.FindAssets("t:LevelStaticData")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelStaticData>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();
    }

    private void LoadLevelData(LevelStaticData level)
    {
        selectedLevel = level;
        stageKey = level.LevelKey;
        stageTitle = level.Title;
        stageDescription = level.Description;
        playerSpawnPoint = level.InitialHeroPosition;

        enemySpawners.Clear();
        enemySpawners.AddRange(
            FindObjectsOfType<SpawnMarker>()
                .Select(marker => new EnemySpawnerEditor
                {

                    EnemyType = marker.EnemyTypeId,
                    GameObject = new GameObject($"Spawner: {marker.EnemyTypeId}")
                })
        );
    }

    private void SaveLevelData()
    {
        if (selectedLevel == null) return;

        selectedLevel.LevelKey = stageKey;
        selectedLevel.Title = stageTitle;
        selectedLevel.Description = stageDescription;
        selectedLevel.InitialHeroPosition = playerSpawnPoint;

        selectedLevel.EnemySpawners = enemySpawners
            .Select(spawner => new EnemySpawnerData(spawner.EnemyType, spawner.GameObject.transform.position))
            .ToList();

        EditorUtility.SetDirty(selectedLevel);
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }

    private void AddEnemySpawner()
    {
        var spawnerData = new EnemySpawnerEditor();
        InstantiateEnemySpawner(spawnerData);
        enemySpawners.Add(spawnerData);

    }

    private void InstantiateEnemySpawner(EnemySpawnerEditor spawnerData)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(EnemySpawnMarkerPrefabPath);
        if (prefab != null)
        {
            spawnerData.GameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            UpdateSpawnerName(spawnerData);
        }
        else
        {
            Debug.LogError("Prefab not found at path: " + EnemySpawnMarkerPrefabPath);
        }
    }

    private void RemoveEnemySpawner(int index)
    {
        if (index >= 0 && index < enemySpawners.Count)
        {
            DestroyImmediate(enemySpawners[index].GameObject);
            enemySpawners.RemoveAt(index);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

    private void UpdateEnemySpawners()
    {
        var currentMarkers = FindObjectsOfType<SpawnMarker>()
            .Select(marker => new EnemySpawnerEditor
            {
                EnemyType = marker.GetComponent<SpawnMarker>().EnemyTypeId,
                GameObject = marker.gameObject
            }).ToList();

        var markersToAdd = currentMarkers.Where(marker => !enemySpawners.Any(es => es.GameObject == marker.GameObject)).ToList();
        var markersToRemove = enemySpawners.Where(es => !currentMarkers.Any(cm => cm.GameObject == es.GameObject)).ToList();
        var markersToUpdate = currentMarkers.Where(marker => enemySpawners.Any(es => es.GameObject == marker.GameObject)).ToList();

        foreach (var marker in markersToAdd)
        {
            enemySpawners.Add(marker);
            UpdateSpawnerName(marker);
        }

        foreach (var marker in markersToRemove)
        {
            DestroyImmediate(marker.GameObject);
            enemySpawners.Remove(marker);
        }

        foreach (var marker in markersToUpdate)
        {
            var existingSpawner = enemySpawners.First(es => es.GameObject == marker.GameObject);
            if (existingSpawner.EnemyType != marker.EnemyType)
            {
                existingSpawner.EnemyType = marker.EnemyType;
                UpdateSpawnerName(existingSpawner);
            }
        }
    }

    private void UpdateSpawnerType(EnemySpawnerEditor spawner, EnemyTypeId newType)
    {
        if (spawner?.GameObject != null)
        {
            var spawnMarker = spawner.GameObject.GetComponent<SpawnMarker>();
            if (spawnMarker != null)
            {
                spawnMarker.EnemyTypeId = newType;
                EditorUtility.SetDirty(spawnMarker);

                spawner.EnemyType = newType;
                UpdateSpawnerName(spawner);
            }
        }
    }
    private void UpdateSpawnerName(EnemySpawnerEditor spawner)
    {
        if (spawner?.GameObject != null)
        {
            if (spawner.GameObject.name != $"Spawner : {spawner.EnemyType}")
            {
                spawner.GameObject.name = $"Spawner : {spawner.EnemyType}";
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

        }
    }

    private void OnHierarchyChanged()
    {
        UpdateEnemySpawners();
        Repaint();
    }

    [System.Serializable]
    private class EnemySpawnerEditor
    {
        public EnemyTypeId EnemyType;
        public GameObject GameObject;
    }
}
