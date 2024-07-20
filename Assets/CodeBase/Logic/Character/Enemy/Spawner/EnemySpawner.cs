using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour, ISavedProgress
{
    private EnemyTypeId _enemyId;
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;
    private string _id;
    private bool _slain;
    public EnemyTypeId EnemyTypeId => _enemyId;

    [Inject]
    private void Construct(IGameFactory factory)
    {
        _factory = factory;
    }

    public void Initialize(string id, EnemyTypeId enemyTypeId)
    {
        _id = id;
        _enemyId = enemyTypeId;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.KillData.ClearedSpawners.Contains(_id))
        {
            _slain = true;
        }
        else
        {
            Spawn();
        }
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        if (_slain)
        {
            progress.KillData.ClearedSpawners.Add(_id);
        }
    }

    private async void Spawn()
    {
        var monster = await _factory.CreateMonster(EnemyTypeId, transform);
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