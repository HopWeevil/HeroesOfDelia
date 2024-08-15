using CodeBase.Data;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "Static Data/Hero")]
    public class HeroStaticData : CharacterStaticData
    {
        public HeroTypeId TypeId;
        public HeroClass HeroClass;
        public Price Price; 
    }
}