using CodeBase.Infrastructure.Factories;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class LevelSelectionWindow : WindowBase
    {
        [SerializeField] private RectTransform _levelsContainer;

        private IStaticDataService _staticData;  
        private IUIFactory _uIFactory;

        [Inject]
        private void Construct(IStaticDataService staticData, IUIFactory factory)
        {
            _staticData = staticData;
            _uIFactory = factory;
        }

        private void Start()
        {
            CreateLevelsCards();
        }

        private async void CreateLevelsCards()
        {
            foreach (LevelStaticData data in _staticData.GetAllLevels())
            {
                await _uIFactory.CreateLevelCard(data, _levelsContainer);
            }
        }
    }
}