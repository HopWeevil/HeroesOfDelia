using CodeBase.Enums;

namespace CodeBase.Logic.Loot
{
    public class ResourceLoot : Loot
    {
        private ResourceTypeId _resourceType;
        private PlayerEconomyData _data;

        public void Construct(ResourceTypeId typeId, PlayerEconomyData data)
        {
            _resourceType = typeId;
            _data = data;
        }

        public override void Collect()
        {
            _data.IncreaseResourceAmount(_resourceType, 1);
            Destroy(gameObject);
        }
    }
}