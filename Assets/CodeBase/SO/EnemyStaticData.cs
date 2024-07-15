using CodeBase.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Static Data/Enemy")]
public class EnemyStaticData : ScriptableObject
{
    public EnemyTypeId EnemyTypeId;

    [Range(1, 100)]
    public int Hp = 50;

    [Range(1, 30)]
    public float Damage = 10;

    [Range(0.2f, 3)]
    public float AttackCooldown = 1;

    [Range(0.5f, 1)]
    public float EffectiveDistance = .5f;

    [Range(0.5f, 1)]
    public float Cleavage = 0.5f;

    [Range(0, 10)]
    public float MoveSpeed = 3;

    [Range(1, 100)]
    public int MaxGold;

    [Range(1, 100)]
    public int MinGold;

    public AssetReferenceGameObject PrefabReference;
}