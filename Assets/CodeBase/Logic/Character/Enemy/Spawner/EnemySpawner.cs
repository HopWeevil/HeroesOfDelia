using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    private EnemyTypeId _enemyId;
    private ICharacterFactory _factory;
    private EnemyDeath _enemyDeath;
    private string _id;
    private bool _slain;
    public EnemyTypeId EnemyTypeId => _enemyId;

    [Inject]
    private void Construct(ICharacterFactory factory)
    {
        _factory = factory;
    }

    public void Initialize(string id, EnemyTypeId enemyTypeId)
    {
        _id = id;
        _enemyId = enemyTypeId;
    }

    private void Start()
    {
        Spawn();
    }

    private async void Spawn()
    {
        var monster = await _factory.CreateEnemy(EnemyTypeId, transform);
        _enemyDeath = monster.GetComponent<EnemyDeath>();
        _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
        if (_enemyDeath != null)
        {
            _enemyDeath.Happened -= Slay;
        }
        _slain = true;
    }
}