
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
public class LevelStaticData : ScriptableObject
{
    public string LevelKey;

    public List<EnemySpawnerData> EnemySpawners;

    public Vector3 InitialHeroPosition;
    public LevelTransferData LevelTransfer;
}