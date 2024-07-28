using CodeBase.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEconomyData
{
    public int GoldAmount { get; private set; }
    private List<EquipmentTypeId> _inventoryItems;

    public Action CoinsAmountChanged;

    public IReadOnlyList<EquipmentTypeId> InventoryItems => _inventoryItems;

    public PlayerEconomyData()
    {
        _inventoryItems = new List<EquipmentTypeId>();
    }

    public void IncreaseCoinsAmount(int value)
    {
        GoldAmount += value;
        CoinsAmountChanged?.Invoke();
    }

    public void AddInventoryItem(EquipmentTypeId typeId)
    {
        _inventoryItems.Add(typeId);
        foreach (var item in _inventoryItems) 
        { 
            Debug.Log(item.ToString());
        }
    }
}