using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    private EnemyTypeId _enemyId;
    private ICharacterFactory _factory;
    public EnemyTypeId EnemyTypeId => _enemyId;

    [Inject]
    private void Construct(ICharacterFactory factory)
    {
        _factory = factory;
    }

    public void Initialize(EnemyTypeId enemyTypeId)
    {
        _enemyId = enemyTypeId;
    }

    private void Start()
    {
        Spawn();
    }

    private async void Spawn()
    {
        var monster = await _factory.CreateEnemy(EnemyTypeId, transform);
    }
}