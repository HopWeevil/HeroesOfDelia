using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "ResourceReward", menuName = "Static Data/ResourceReward")]
    public class ResourceRewardStaticData : ScriptableObject
    {
        public ResourceTypeId ResourceType;
        public int Amount;
    }
}