using CodeBase.Logic.Loot;
using CodeBase.Services.PersistentProgress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LootCollector : MonoBehaviour
{
    [SerializeField] private float _attractionSpeed = 2f;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private int _maxLootObjects = 10;
    [SerializeField] private int _updateInterval = 10;
    [SerializeField] private LayerMask _lootMask;

    private HashSet<Loot> _attractedLoot;
    private Collider[] _lootColliders;
    private int _currentIntervalCount = 0;

    private IPersistentProgressService _persistentProgressService;

    private void Start()
    {
        _lootColliders = new Collider[_maxLootObjects];
        _attractedLoot = new HashSet<Loot>();
    }

    [Inject]
    private void Construct(IPersistentProgressService persistentProgressService)
    {
        _persistentProgressService = persistentProgressService;
    }

    private void Update()
    {
        _currentIntervalCount++;
        if (_currentIntervalCount >= _updateInterval)
        {
            _currentIntervalCount = 0;
            CollectNearbyLoot();
        }
    }

    private void CollectNearbyLoot()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _lootColliders, _lootMask);

        for (int i = 0; i < numColliders; i++)
        {
            if (_lootColliders[i].TryGetComponent(out Loot loot) && !_attractedLoot.Contains(loot))
            {
                _attractedLoot.Add(loot);
                StartCoroutine(AttractLoot(loot));

            }
        }
    }

    private IEnumerator AttractLoot(Loot loot)
    {
        while (Vector3.Distance(loot.transform.position, transform.position) > 0.1f)
        {
            loot.transform.position = Vector3.MoveTowards(loot.transform.position, transform.position, _attractionSpeed * Time.deltaTime);
            yield return null;
        }
        _attractedLoot.Remove(loot);
        loot.Collect(_persistentProgressService.Economy);

    }
}