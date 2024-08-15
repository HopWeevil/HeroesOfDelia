using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using System.Threading.Tasks;
using UnityEngine;

public class WindowService : IWindowService
{
    private readonly IUIFactory _uiFactory;

    public WindowService(IUIFactory uiFactory)
    {
        _uiFactory = uiFactory;
    }

    public Task<GameObject> Open(WindowId windowId)
    {
        return null;
    }
}