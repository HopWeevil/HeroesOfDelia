using CodeBase.Enums;

namespace CodeBase.Logic.Loot
{
    public class ResourceLoot : Loot
    {
        private ResourceTypeId _resourceType;

        public void Initialize(ResourceTypeId typeId)
        {
            _resourceType = typeId;
        }

        public override void Collect(PlayerEconomyData playerEconomy)
        {
            playerEconomy.IncreaseCoinsAmount(1);
            Destroy(gameObject);
        }
    }
}