using CodeBase.Enums;
using CodeBase.Services.Notification;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class HeroesWindow : WindowBase
    {
        [SerializeField] private BuyHeroButton _buyHero;
        [SerializeField] private HeroSelectButton _heroSelect;
        [SerializeField] private HeroSelectionCarousel _carousel;

        private IPersistentProgressService _progressService;
        private IStaticDataService _staticDataService;
        private IPopupMessageService _popupMessageService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgress, IStaticDataService staticDataService, IPopupMessageService messageService)
        {
            _progressService = persistentProgress;
            _staticDataService = staticDataService;
            _popupMessageService = messageService;
        }

        private void OnEnable()
        {
            _carousel.HeroChanged += OnHeroChanged;
            _buyHero.Click += OnBuyHeroClicked;
            _heroSelect.Click += OnSelectHeroClicked;
        }

        private void OnDisable()
        {
            _carousel.HeroChanged -= OnHeroChanged;
            _buyHero.Click -= OnBuyHeroClicked;
            _heroSelect.Click -= OnSelectHeroClicked;
        }

        private void OnSelectHeroClicked()
        {
            _progressService.Progress.ChangeHero(_carousel.CurrentHeroId);
            UpdateButtonsForHero(_staticDataService.ForHero(_carousel.CurrentHeroId));
        }

        private void OnBuyHeroClicked()
        {
            HeroStaticData hero = _staticDataService.ForHero(_carousel.CurrentHeroId);

            if (hero.Price.Value <= _progressService.Economy.ResourcesAmount[hero.Price.Resource.ResourceTypeId])
            {
                _progressService.Economy.AddHeroItem(hero.TypeId);
                _progressService.Economy.DecreaseResourceAmount(hero.Price.Resource.ResourceTypeId, hero.Price.Value);
                _popupMessageService.ShowMessage("Hero purchased successfully", Color.white);
            }
            else
            {
                _popupMessageService.ShowMessage("Not enough " + hero.Price.Resource.ResourceTypeId.ToString(), Color.red);
            }

            UpdateButtonsForHero(hero);
        }

        private void OnHeroChanged(HeroStaticData hero)
        {
            UpdateButtonsForHero(hero);
        }

        private void UpdateButtonsForHero(HeroStaticData hero)
        {
            _heroSelect.RefreshButton(hero.TypeId);
            _heroSelect.SetButtonText(hero.TypeId);
            _buyHero.RefreshButton(hero.TypeId);
            _buyHero.SetHeroPrice(hero.Price);
        }
    }
}