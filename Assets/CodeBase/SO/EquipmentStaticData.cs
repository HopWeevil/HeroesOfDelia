using CodeBase.Data;
using CodeBase.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Static Data/Equipment")]
    public class EquipmentStaticData : ScriptableObject
    {
        public string Title;

        public string Description;

        public Sprite Icon;

        public EquipmentTypeId EquipmentTypeId;

        public Rarity Rarity;

        public EquipmentCategory EquipmentClass;

        public AssetReferenceGameObject PrefabReference;

        public AssetReferenceGameObject DropReference;

        public List<StatsBonus> Bonuses;
    }
}

public enum BonusType
{
    Health,
    AttackCooldown,
    AttackDistance,
    AttackSplash,
    Damage,
    MoveSpeed,
    Armor,
}

[System.Serializable]
public class StatsBonus
{
    public BonusType Type;
    public float Value;

    public void Apply(Stats stats, int level)
    {
        /*switch (Type)
        {
            case BonusType.Damage:
                stats.Damage += Value * level;
                break;
            case BonusType.MoveSpeed:
                stats.MoveSpeed += Value * level;
                break;
            case BonusType.Armor:
                stats.Armor += Value * level;
                break;
            case BonusType.AttackSpeed:
                stats.AttackSpeed += Value * level;
                break;
            case BonusType.AttackDistance:
                stats.AttackDistance += Value * level;
                break;
            case BonusType.Health:
                stats.Hp += Value * level;
                break;
            case BonusType.AttackSplash:
                stats.AttackSplash += Value * level;
                break;
        }*/
    }
}