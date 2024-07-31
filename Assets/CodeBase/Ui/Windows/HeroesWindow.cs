using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class HeroesWindow : WindowBase
    {
        [SerializeField] private BuyHeroButton _buyHero;
        [SerializeField] private HeroSelectButton _heroSelect;

        [SerializeField] private Button _selectHeroButton;
        [SerializeField] private Button _buyHeroButton;
        [SerializeField] private HeroSelectionCarousel _carousel;

        private IPersistentProgressService _progressService;
        private IStaticDataService _staticDataService;

        private void OnEnable()
        {
            _selectHeroButton.onClick.AddListener(OnSelectButtonClick);
            _buyHeroButton.onClick.AddListener(OnBuyButtonClick);
            _carousel.OnHeroChanged += OnHeroChanged;
        }


        private void OnDisable()
        {
            _selectHeroButton.onClick.RemoveListener(OnSelectButtonClick);
            _buyHeroButton.onClick.RemoveListener(OnBuyButtonClick);
        }

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress, IStaticDataService staticDataService)
        {
            _progressService = persistentProgress;
            _staticDataService = staticDataService;
        }

        private void OnSelectButtonClick()
        {
           // _progressService.Progress._selectedHeroId = _carousel.CurrentHeroId;
            _progressService.Progress.ChangeHero(_carousel.CurrentHeroId);
            _heroSelect.UpdateButton();
            _buyHero.UpdateButton();
        }

        private void OnBuyButtonClick()
        {
            _progressService.Economy.AddHeroItem(_carousel.CurrentHeroId);
            Price price = _staticDataService.ForHero(_carousel.CurrentHeroId).Price;
            _progressService.Economy.DecreaseResourceAmount(price.Resource.ResourceTypeId, price.Value);
            _heroSelect.UpdateButton();
            _buyHero.UpdateButton();
        }

        private void OnHeroChanged(HeroStaticData data)
        {
            _heroSelect.UpdateButton();
            _buyHero.UpdateButton();
        }
    }
}