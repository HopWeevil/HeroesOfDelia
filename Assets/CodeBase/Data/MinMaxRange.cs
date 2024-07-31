using System;
using UnityEngine;

namespace CodeBase.SO
{
    [System.Serializable]
    public struct MinMaxRange
    {
        [Range(1, 100)]
        public int MinValue;
        [Range(1, 100)]
        public int MaxValue;
    }
}