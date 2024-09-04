using CodeBase.SO;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.SO
{
    public class CharacterStaticData : ScriptableObject
    {
        [Range(1, 100)]
        public int Hp = 50;

        [Range(1, 100)]
        public float Damage = 10;

        [Range(0, 10)]
        public float MoveSpeed = 3;

        [Range(0, 10)]
        public float Armor = 3;

        [Range(0.2f, 3)]
        public float AttackCooldown = 1;

        [Range(0.5f, 1)]
        public float EffectiveDistance = 0.5f;

        [Range(0.5f, 1)]
        public float Cleavage = 0.5f;

        public AssetReferenceGameObject PrefabReference;
    }
}
