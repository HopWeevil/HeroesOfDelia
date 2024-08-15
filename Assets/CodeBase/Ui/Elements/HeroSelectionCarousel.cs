using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;

namespace CodeBase.UI.Elements
{
    public class HeroSelectionCarousel : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private float _spawnOffset = 2.0f;
        [SerializeField] private Vector3 _heroViewAngle;

        private HeroTypeId _currentHeroTypeId;
        private List<HeroTypeId> _heroTypeIds;

        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private IShowcaseHeroFactory _demonstationHeroCreator;
        private GameObject _currentHero;
        private bool _isAnimating;

        public HeroTypeId CurrentHeroId => _currentHeroTypeId;
        public event Action<HeroStaticData> HeroChanged;


        [Inject]
        private void Construct(IStaticDataService staticDataService, IPersistentProgressService progressService, IShowcaseHeroFactory heroCreator)
        {
            _staticData = staticDataService;
            _progressService = progressService;
            _demonstationHeroCreator = heroCreator;
        }

        private async void Start()
        {
            InitializeHeroTypeIds();
            await InitializeCurrentHero();
        }

        private void OnEnable()
        {
            _nextButton.onClick.AddListener(OnNextButtonClick);
            _previousButton.onClick.AddListener(OnPreviousButtonClick);
        }

        private void OnDisable()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
            _previousButton.onClick.RemoveListener(OnPreviousButtonClick);
        }

        private void OnDestroy()
        {
            if (_currentHeroTypeId != _progressService.Progress.SelectedHero)
            {
                Destroy(_currentHero);
                _demonstationHeroCreator.Create();
            }
        }

        private void InitializeHeroTypeIds()
        {
            _heroTypeIds = new List<HeroTypeId>((HeroTypeId[])Enum.GetValues(typeof(HeroTypeId)));
            _currentHeroTypeId = _progressService.Progress.SelectedHero;
        }

        private async Task InitializeCurrentHero()
        {
            _currentHero = await _demonstationHeroCreator.GetOrCreate();
            HeroChanged?.Invoke(_staticData.ForHero(_currentHeroTypeId));
        }

        private async void OnNextButtonClick()
        {
            if (_isAnimating) return;

            _isAnimating = true;
            _currentHeroTypeId = GetNextHeroTypeId();
            await UpdateHeroScroll(Vector3.right * _spawnOffset);
        }

        private async void OnPreviousButtonClick()
        {
            if (_isAnimating)
            {
                return;
            }

            _isAnimating = true;
            _currentHeroTypeId = GetPreviousHeroTypeId();
            await UpdateHeroScroll(Vector3.left * _spawnOffset);
        }

        private HeroTypeId GetNextHeroTypeId()
        {
            int currentIndex = _heroTypeIds.IndexOf(_currentHeroTypeId);
            return _heroTypeIds[(currentIndex + 1) % _heroTypeIds.Count];
        }

        private HeroTypeId GetPreviousHeroTypeId()
        {
            int currentIndex = _heroTypeIds.IndexOf(_currentHeroTypeId);
            return _heroTypeIds[(currentIndex - 1 + _heroTypeIds.Count) % _heroTypeIds.Count];
        }

        private async Task UpdateHeroScroll(Vector3 targetPositionOffset)
        {
            GameObject nextHero = await _demonstationHeroCreator.Create(_currentHeroTypeId, targetPositionOffset, _heroViewAngle);
            HeroChanged?.Invoke(_staticData.ForHero(_currentHeroTypeId));
            MoveCurrentHeroTo(-targetPositionOffset);
            MoveNextHeroTo(nextHero, Vector3.zero);
        }

        private void MoveCurrentHeroTo(Vector3 position)
        {
            _currentHero.transform.DOMove(_currentHero.transform.position + position, 0.5f).SetEase(Ease.OutQuad);
        }

        private void MoveNextHeroTo(GameObject nextHero, Vector3 position)
        {
            nextHero.transform.DOMove(position, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                FinalizeHeroChange(nextHero);
            });
        }

        private void FinalizeHeroChange(GameObject nextHero)
        {
            Destroy(_currentHero);
            _currentHero = nextHero;
            _isAnimating = false;          
        }
    }
}