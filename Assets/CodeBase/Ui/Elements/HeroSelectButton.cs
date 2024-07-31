using CodeBase.Services.PersistentProgress;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class HeroSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private HeroSelectionCarousel _carousel;
        [SerializeField] private TMP_Text _text;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
        }

        private void Awake()
        {
            _carousel.OnHeroChanged += OnHeroChanged;
        }

        private void OnDestroy()
        {
            _carousel.OnHeroChanged -= OnHeroChanged;
        }

        private void OnHeroChanged(HeroStaticData obj)
        {
           
           
        }

        public void UpdateButton()
        {     
            bool isHeroSelected = _persistentProgressService.Progress.SelectedHero == _carousel.CurrentHeroId;
            bool isHeroBought = _persistentProgressService.Economy.IsHeroBuyed(_carousel.CurrentHeroId);

            gameObject.SetActive(isHeroBought);
            _text.text = isHeroSelected ? "Selected" : "Select";
        }
    }
}