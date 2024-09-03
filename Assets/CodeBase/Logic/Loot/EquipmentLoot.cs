using CodeBase.Enums;

namespace CodeBase.Logic.Loot
{
    public class EquipmentLoot : Loot
    {
        private EquipmentTypeId _typeId;
        private PlayerEquipment _equipment;

        public void Initialize(EquipmentTypeId typeId, PlayerEquipment playerEquipment)
        {
            _typeId = typeId;
            _equipment = playerEquipment;
        }

        public override void Collect()
        {
            _equipment.AddInventoryItem(new EquipmentItem(_typeId));
            Destroy(gameObject);
        }
    }
}