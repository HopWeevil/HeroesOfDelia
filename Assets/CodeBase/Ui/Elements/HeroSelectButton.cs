using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class HeroSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

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
            bool isHeroBought = _persistentProgressService.Economy.IsHeroBuyed(hero);
            gameObject.SetActive(isHeroBought);
        }

        public void SetButtonText(HeroTypeId hero)
        {
            bool isHeroSelected = _persistentProgressService.Progress.SelectedHero == hero;
            _text.text = isHeroSelected ? "Selected" : "Select";
        }
    }
}