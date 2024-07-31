using CodeBase.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEconomyData
{
    private Dictionary<ResourceTypeId, int> _resourcesAmount;
    private List<EquipmentTypeId> _inventoryItems;
    private List<HeroTypeId> _buyedHeroes;

    public IReadOnlyList<EquipmentTypeId> InventoryItems => _inventoryItems;
    public IReadOnlyList<HeroTypeId> BuyedHeroes => _buyedHeroes;
    public IReadOnlyDictionary<ResourceTypeId, int> ResourcesAmount => _resourcesAmount;

    //public Action<ResourceTypeId, int> ResourcesAmountChanged;
    public Action<ResourceTypeId> ResourcesAmountChanged;

    public PlayerEconomyData()
    {
        _inventoryItems = new List<EquipmentTypeId>();
        _buyedHeroes = new List<HeroTypeId>();
        _resourcesAmount = new Dictionary<ResourceTypeId, int>();
    }

    public void IncreaseResourceAmount(ResourceTypeId typeId, int value)
    {
        if (_resourcesAmount.TryGetValue(typeId, out _) == false) 
        {
            _resourcesAmount.Add(typeId, 0);
        }
      
        _resourcesAmount[typeId] += value;
        //ResourcesAmountChanged?.Invoke(typeId, _resourcesAmount[typeId]);
        ResourcesAmountChanged?.Invoke(typeId);
    }

    public void DecreaseResourceAmount(ResourceTypeId typeId, int value)
    {
        _resourcesAmount[typeId] -= value;
        ResourcesAmountChanged?.Invoke(typeId);
    }

    public int GetResourceAmount(ResourceTypeId typeId) 
    {
        return _resourcesAmount[typeId];
    }

    public void AddInventoryItem(EquipmentTypeId typeId)
    {
        _inventoryItems.Add(typeId);
    }

    public void AddHeroItem(HeroTypeId typeId)
    {
        if (_buyedHeroes.Contains(typeId) == false)
        {
            _buyedHeroes.Add(typeId);
        }       
    }

    public bool IsHeroBuyed(HeroTypeId typeId)
    {
        if (_buyedHeroes.Contains(typeId))
        {
            return true;
        }
        return false;
    }
}