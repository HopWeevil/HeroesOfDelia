using CodeBase.Infrastructure.Services.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DebugSaver : MonoBehaviour
{
    private ISaveLoadService _saveLoadService;
    private void Start()
    {
        DiContainer container = FindObjectOfType<SceneContext>().Container.ParentContainers[0];
        _saveLoadService = container.Resolve<ISaveLoadService>();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T)) 
        { 
            _saveLoadService.SaveProgress();
        }
    }
}
