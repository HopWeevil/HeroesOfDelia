using CodeBase.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
    }
}