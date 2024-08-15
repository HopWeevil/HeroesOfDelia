using CodeBase.Enums;
using System.Threading.Tasks;
using UnityEngine;

public interface IWindowService
{
    public Task<GameObject> Open(WindowId windowId);
}
