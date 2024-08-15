using CodeBase.Enums;
using System;
using System.Collections.Generic;

public class PlayerEquipment
{
    private readonly List<EquipmentItem> _equipmentItems;
    private readonly Dictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>> _heroesEquipment;

    public IReadOnlyList<EquipmentItem> EquipmentItems => _equipmentItems;
    public IReadOnlyDictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>> HeroesEquipment => _heroesEquipment;

    public event Action<HeroTypeId, EquipmentItem> HeroEquip;
    public event Action<HeroTypeId, EquipmentItem> HeroUnEquip;
    public event Action<int> EquipmentItemRemove;
    public event Action<int> EquipmentItemAdd;

    public PlayerEquipment()
    {
        _equipmentItems = new List<EquipmentItem>();
        _heroesEquipment = new Dictionary<HeroTypeId, Dictionary<EquipmentCategory, EquipmentItem>>();
    }

    public void AddInventoryItem(EquipmentItem item)
    {
        _equipmentItems.Add(item);
        EquipmentItemAdd?.Invoke(_equipmentItems.IndexOf(item));
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
        int index = _equipmentItems.IndexOf(item);
        if (index >= 0)
        {
            _equipmentItems.RemoveAt(index);
            EquipmentItemRemove?.Invoke(index);
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

public class PlayerEquipmentService
{
    public PlayerEquipment Equipment {  get; set; }
}