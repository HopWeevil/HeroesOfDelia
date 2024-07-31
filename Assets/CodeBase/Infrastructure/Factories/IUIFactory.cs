using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface IUIFactory
    {
        Task CreateUIRoot();
        Task<GameObject> CreateInventory();
        Task<GameObject> CreateMainMenu();
        Task<GameObject> CreateShop();
        Task<GameObject> CreateWindow(string address);
        Task<GameObject> CreateWindow(WindowId windowId);
    }
}