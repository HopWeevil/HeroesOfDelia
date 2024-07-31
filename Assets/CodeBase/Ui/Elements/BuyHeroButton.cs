using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class BuyHeroButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private HeroSelectionCarousel _carousel;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticData)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticData;
        }

        private void Start()
        {
            _carousel.OnHeroChanged += OnHeroChanged;
            _button.onClick.AddListener(OnButtonClick);
        }


        private void OnDestroy()
        {
            _carousel.OnHeroChanged -= OnHeroChanged;
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
       //     _persistentProgressService.Economy.AddHeroItem(_carousel.CurrentHeroId);
        }

        private void OnHeroChanged(HeroStaticData obj)
        {
            
        }

        public void UpdateButton()
        {
            Price price = _staticDataService.ForHero(_carousel.CurrentHeroId).Price;
            _text.text = price.Value.ToString();
            _icon.sprite = price.Resource.Icon;

            bool isHeroSelected = _persistentProgressService.Progress.SelectedHero == _carousel.CurrentHeroId;
            bool isHeroBought = _persistentProgressService.Economy.IsHeroBuyed(_carousel.CurrentHeroId);

            gameObject.SetActive(!isHeroSelected && !isHeroBought);
        }
    }
}