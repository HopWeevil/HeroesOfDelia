using CodeBase.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Static Data/Resource")]
    public class ResourceStaticData : ScriptableObject
    {
        public Sprite Icon;

        public ResourceTypeId ResourceTypeId;

        public AssetReferenceGameObject PrefabReference;
    }
}