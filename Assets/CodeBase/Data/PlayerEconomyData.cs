using CodeBase.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EquipmentItem
{
    public EquipmentTypeId EquipmentTypeId;
    public int Level;

    public EquipmentItem(EquipmentTypeId equipmentTypeId, int level)
    {
        EquipmentTypeId = equipmentTypeId;
        Level = level;
    }

    public EquipmentItem(EquipmentTypeId equipmentTypeId)
    {
        EquipmentTypeId = equipmentTypeId;
        Level = UnityEngine.Random.Range(1,100);
    }
}

[Serializable]
public class PlayerEconomyData
{
    private List<HeroTypeId> _buyedHeroes;

    private Dictionary<ResourceTypeId, int> _resourcesAmount;


    public IReadOnlyList<HeroTypeId> BuyedHeroes => _buyedHeroes;
    public IReadOnlyDictionary<ResourceTypeId, int> ResourcesAmount => _resourcesAmount;



    public Action<ResourceTypeId> ResourcesAmountChanged;



    public PlayerEconomyData()
    {
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
        ResourcesAmountChanged?.Invoke(typeId);
    }

    public void DecreaseResourceAmount(ResourceTypeId resource, int value)
    {
        _resourcesAmount[resource] -= value;
        ResourcesAmountChanged?.Invoke(resource);
    }

    public int GetResourceAmount(ResourceTypeId resource) 
    {
        return _resourcesAmount[resource];
    }

   
    public void AddHeroItem(HeroTypeId hero)
    {
        if (_buyedHeroes.Contains(hero) == false)
        {
            _buyedHeroes.Add(hero);
        }       
    }

    public bool IsHeroBuyed(HeroTypeId hero)
    {
        if (_buyedHeroes.Contains(hero))
        {
            return true;
        }
        return false;
    }
}