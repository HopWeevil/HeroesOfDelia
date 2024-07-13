using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyTypeId enemyId;

    private IGameFactory _factory;

    [Inject]
    private void Construct(IGameFactory factory)
    {
        _factory = factory;
        _factory.CreateMonster(enemyId, transform);
    }

    private void Start()
    {
        
    }
}
