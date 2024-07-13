using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    [SerializeField] private GameObject _heroPrefab;
    public override void InstallBindings()
    {
        GameObject hero = Container.InstantiatePrefab(_heroPrefab);

        Container.Bind<GameObject>().FromInstance(hero).AsSingle().NonLazy();
    }
}
