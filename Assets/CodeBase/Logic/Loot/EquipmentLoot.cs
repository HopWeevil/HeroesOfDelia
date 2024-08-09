using CodeBase.Enums;

namespace CodeBase.Logic.Loot
{
    public class EquipmentLoot : Loot
    {
        private EquipmentTypeId _typeId;
        public void Initialize(EquipmentTypeId typeId)
        {
            _typeId = typeId;
        }

        public override void Collect(PlayerEconomyData playerEconomy)
        {
            //playerEconomy.AddInventoryItem(new InventoryItem(_typeId));
            Destroy(gameObject);
        }
    }
}