using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Logic.Loot
{
    public abstract class Loot : MonoBehaviour
    {
        public abstract void Collect(PlayerEconomyData playerEconomy);
    }
}
