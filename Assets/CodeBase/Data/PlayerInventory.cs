using CodeBase.Enums;
using System;
using System.Collections.Generic;

public class PlayerInventory
{
    private readonly List<InventoryItem> _inventoryItems;
    private readonly Dictionary<HeroTypeId, Dictionary<EquipmentCategory, InventoryItem>> _heroesEquipment;

    public IReadOnlyList<InventoryItem> InventoryItems => _inventoryItems;
    public IReadOnlyDictionary<HeroTypeId, Dictionary<EquipmentCategory, InventoryItem>> HeroesEquipment => _heroesEquipment;

    public event Action<HeroTypeId, InventoryItem> OnHeroEquip;
    public event Action<HeroTypeId, InventoryItem> OnHeroUnEquip;
    public event Action<int> OnInventoryItemRemove;
    public event Action<int> OnInventoryItemAdd;

    public PlayerInventory()
    {
        _inventoryItems = new List<InventoryItem>();
        _heroesEquipment = new Dictionary<HeroTypeId, Dictionary<EquipmentCategory, InventoryItem>>();
    }

    public void AddInventoryItem(InventoryItem item)
    {
        _inventoryItems.Add(item);
        OnInventoryItemAdd?.Invoke(_inventoryItems.IndexOf(item));
    }

    public void EquipHero(HeroTypeId hero, InventoryItem equipment, EquipmentCategory category)
    {
        if (!_heroesEquipment.TryGetValue(hero, out var equipmentDict))
        {
            equipmentDict = new Dictionary<EquipmentCategory, InventoryItem>();
            _heroesEquipment[hero] = equipmentDict;
        }

        if (equipmentDict.TryGetValue(category, out var oldItem))
        {
            UnequipHero(hero, oldItem, category);
        }

        equipmentDict[category] = equipment;
        RemoveInventoryItem(equipment);
        OnHeroEquip?.Invoke(hero, equipment);
    }

    public void UnequipHero(HeroTypeId hero, InventoryItem equipment, EquipmentCategory category)
    {
        if (_heroesEquipment.TryGetValue(hero, out var equipmentDict) && equipmentDict.Remove(category))
        {
            AddInventoryItem(equipment);
            OnHeroUnEquip?.Invoke(hero, equipment);
        }
    }

    public void RemoveInventoryItem(InventoryItem item)
    {
        int index = _inventoryItems.IndexOf(item);
        if (index >= 0)
        {
            _inventoryItems.RemoveAt(index);
            OnInventoryItemRemove?.Invoke(index);
        }
    }

    public bool IsItemEquipped(HeroTypeId hero, InventoryItem equipment, EquipmentCategory category)
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

    public InventoryItem GetEquippedItem(HeroTypeId hero, EquipmentCategory category)
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