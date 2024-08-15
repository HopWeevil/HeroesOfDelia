using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class BuyHeroButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;

        public UnityAction Click;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Click?.Invoke();
        }

        public void RefreshButton(HeroTypeId hero)
        {
            bool isHeroSelected = _persistentProgressService.Progress.SelectedHero == hero;
            bool isHeroBought = _persistentProgressService.Economy.IsHeroBuyed(hero);

            gameObject.SetActive(!isHeroSelected && !isHeroBought);
        }

        public void SetHeroPrice(Price price)
        {
            _text.text = price.Value.ToString();
            _icon.sprite = price.Resource.Icon;
        }
    }
}