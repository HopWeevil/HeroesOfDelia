using CodeBase.Enums;
using System;
using System.Collections.Generic;

public class PlayerInventory
{
    private readonly List<EquipmentItem> _inventoryItems;
    private readonly Dictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>> _heroesEquipment;

    public IReadOnlyList<EquipmentItem> InventoryItems => _inventoryItems;
    public IReadOnlyDictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>> HeroesEquipment => _heroesEquipment;

    public event Action<HeroTypeId, EquipmentItem> HeroEquip;
    public event Action<HeroTypeId, EquipmentItem> HeroUnEquip;
    public event Action<int> InventoryItemRemove;
    public event Action<int> InventoryItemAdd;

    public PlayerInventory()
    {
        _inventoryItems = new List<EquipmentItem>();
        _heroesEquipment = new Dictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>>();
    }

    public void AddInventoryItem(EquipmentItem item)
    {
        _inventoryItems.Add(item);
        InventoryItemAdd?.Invoke(_inventoryItems.IndexOf(item));
    }

    public void EquipHero(HeroTypeId hero, EquipmentItem equipment, EquipmentCategory category)
    {
        if (!_heroesEquipment.TryGetValue(hero, out var equipmentDict))
        {
            equipmentDict = new Dictionary<EquipmentCategory, EquipmentItem>();
            _heroesEquipment[hero] = equipmentDict;
        }

        if (equipmentDict.TryGetValue(category, out var oldItem))
        {
            UnequipHero(hero, oldItem, category);
        }

        equipmentDict[category] = equipment;
        RemoveInventoryItem(equipment);
        HeroEquip?.Invoke(hero, equipment);
    }

    public void UnequipHero(HeroTypeId hero, EquipmentItem equipment, EquipmentCategory category)
    {
        if (_heroesEquipment.TryGetValue(hero, out var equipmentDict) && equipmentDict.Remove(category))
        {
            AddInventoryItem(equipment);
            HeroUnEquip?.Invoke(hero, equipment);
        }
    }

    public void RemoveInventoryItem(EquipmentItem item)
    {
        int index = _inventoryItems.IndexOf(item);
        if (index >= 0)
        {
            _inventoryItems.RemoveAt(index);
            InventoryItemRemove?.Invoke(index);
        }
    }

    public bool IsItemEquipped(HeroTypeId hero, EquipmentItem equipment, EquipmentCategory category)
    {
        if (_heroesEquipment.TryGetValue(hero, out var equipmentDict))
        {
            if (equipmentDict.TryGetValue(category, out var equippedItem))
            {
                return equippedItem.Equals(equipment);
            }
        }
        return false;
    }

    public EquipmentItem GetEquippedItem(HeroTypeId hero, EquipmentCategory category)
    {
        if (_heroesEquipment.TryGetValue(hero, out var equipmentDict))
        {
            if (equipmentDict.TryGetValue(category, out var item))
            {
                return item;
            }
        }
        return null;
    }
}