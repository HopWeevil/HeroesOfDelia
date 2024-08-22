using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeBase.Enums;
using CodeBase.Logic;
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
    private static void ShowWindow() =>
        GetWindow<LevelDataEditor>("Level editor").Show();

    private void OnEnable()
    {
        EditorSceneManager.activeSceneChangedInEditMode += ResetWindow;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        LoadAllLevels();
        ResetWindow();
    }

    private void OnDisable()
    {
        EditorSceneManager.activeSceneChangedInEditMode -= ResetWindow;
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
    }

    private void OnGUI()
    {
        //enemySpawners.Clear();
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
        bool saveChanges = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        if (saveChanges)
        {
            bool result = EditorUtility.DisplayDialog(
                "Confirm Load Level",
                $"Are you sure you want to load the level \"{level.Title}\"? This will switch to the corresponding scene.",
                "Yes",
                "No"
            );

            if (result)
            {
                LoadLevel(level);
                LoadLevelData(level); //
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
            EditorUtility.DisplayDialog(
                "Error",
                $"The scene \"{scenePath}\" does not exist.",
                "OK"
            );
        }
    }

    private void DrawLevelSelection()
    {
        EditorGUILayout.LabelField("Select Level to Edit", EditorStyles.boldLabel);
        foreach (var level in levels)
        {
            if (GUILayout.Button(level.Title))
            {
                ShowLoadLevelConfirmation(level); // Call the confirmation dialog
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
        EditorGUILayout.BeginVertical(); // Ensure vertical layout starts and ends correctly

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select", EditorStyles.boldLabel, GUILayout.Width(50));
        EditorGUILayout.LabelField("Enemy Type", EditorStyles.boldLabel, GUILayout.Width(200));
        EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        foreach (var spawner in enemySpawners)
        {
            if (spawner == null || spawner.GameObject == null)
            {
                continue; // Skip if spawner or its game object is null
            }

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

            spawner.GameObject.transform.position = EditorGUILayout.Vector3Field("", spawner.GameObject.transform.position);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveEnemySpawner(enemySpawners.IndexOf(spawner)); // Use index for removal
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical(); // Ensure vertical layout ends correctly
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

        SpawnMarker[] markers = FindObjectsOfType<SpawnMarker>();
        foreach(SpawnMarker marker in markers)
        {
            Debug.Log(marker.gameObject.name);
            var spawner = new EnemySpawnerEditor
            {
                EnemyType = marker.EnemyTypeId,
                GameObject = new GameObject($"Spawner: {marker.EnemyTypeId}")
            };
            enemySpawners.Add(spawner);
        }
    }

    private void SaveLevelData()
    {
        if (selectedLevel == null) return;

        selectedLevel.LevelKey = stageKey;
        selectedLevel.Title = stageTitle;
        selectedLevel.Description = stageDescription;
        selectedLevel.InitialHeroPosition = playerSpawnPoint;

        List<EnemySpawnerData> spawners = new List<EnemySpawnerData>();
        foreach (var spawner in enemySpawners)
        {
            spawners.Add(new EnemySpawnerData(spawner.EnemyType, spawner.GameObject.transform.position));
        }
        selectedLevel.EnemySpawners = spawners;

        EditorUtility.SetDirty(selectedLevel);
        AssetDatabase.SaveAssets();

       // EditorUtility.SetDirty(Efidir);
        // Save the current scene after making modifications
       //if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveOpenScenes();
        }
    }

    private void ResetWindow(Scene previous, Scene current) => ResetWindow();

    private void ResetWindow()
    {
        ClearEnemySpawners();
        Repaint();
    }

    private void ClearEnemySpawners()
    {
        enemySpawners.ForEach(sp => DestroyImmediate(sp.GameObject));
        enemySpawners.Clear();
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
            Undo.RegisterCreatedObjectUndo(spawnerData.GameObject, "Create Enemy Spawner");
            UpdateSpawnerName(spawnerData);
        }
        else
        {
            Debug.LogError("Prefab not found at path: " + EnemySpawnMarkerPrefabPath);
        }
    }

    private void RemoveEnemySpawner(int index)
    {
        DestroyImmediate(enemySpawners[index].GameObject);
        enemySpawners.RemoveAt(index);
    }

    private void UpdateEnemySpawners()
    {
        var currentMarkers = FindCurrentMarkers();

        AddNewSpawners(currentMarkers);
        RemoveDeletedSpawners(currentMarkers);
        UpdateExistingSpawners(currentMarkers);
    }

    private List<EnemySpawnerEditor> FindCurrentMarkers() =>
        GameObject.FindGameObjectsWithTag("SpawnMarker")
            .Select(marker => new EnemySpawnerEditor
            {
                EnemyType = marker.GetComponent<SpawnMarker>().EnemyTypeId,
                GameObject = marker
            }).ToList();

    private void AddNewSpawners(List<EnemySpawnerEditor> currentMarkers)
    {
        foreach (var marker in currentMarkers)
        {
            if (!enemySpawners.Any(es => es.GameObject == marker.GameObject))
            {
                enemySpawners.Add(marker);
                UpdateSpawnerName(marker);
            }
        }
    }

    private void RemoveDeletedSpawners(List<EnemySpawnerEditor> currentMarkers)
    {
        enemySpawners.RemoveAll(es => !currentMarkers.Any(cm => cm.GameObject == es.GameObject));
    }

    private void UpdateExistingSpawners(List<EnemySpawnerEditor> currentMarkers)
    {
        foreach (var marker in currentMarkers)
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
        if (spawner.GameObject != null)
        {
            spawner.EnemyType = newType;
            UpdateSpawnerName(spawner);

            var spawnMarker = spawner.GameObject.GetComponent<SpawnMarker>();
            if (spawnMarker != null)
            {
                spawnMarker.EnemyTypeId = newType;
            }
        }
    }

    private void UpdateSpawnerName(EnemySpawnerEditor spawner)
    {
        if (spawner.GameObject != null)
        {
            spawner.GameObject.name = $"Spawner : {spawner.EnemyType}";
            Undo.RegisterCreatedObjectUndo(spawner.GameObject, "Change spawner type");
            //EditorUtility.SetDirty(spawner.GameObject);
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
