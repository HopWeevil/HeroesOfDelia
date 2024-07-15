using System;
using UnityEngine;
using CodeBase.Enums;

[Serializable]
public class EnemySpawnerData
{
    public string Id;
    public EnemyTypeId MonsterTypeId;
    public Vector3 Position;

    public EnemySpawnerData(string id, EnemyTypeId enemyTypeId, Vector3 position)
    {
        Id = id;
        MonsterTypeId = enemyTypeId;
        Position = position;
    }
}