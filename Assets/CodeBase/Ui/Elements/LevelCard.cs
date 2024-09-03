using CodeBase.Infrastructure.States;
using CodeBase.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class LevelCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _preview;
        [SerializeField] private Button _button;

        private IGameStateMachine _gameStateMachine;
        private LevelStaticData _data;

        public void SetLevelData(LevelStaticData data)
        {
            _data = data;
        }

        public void SetInfo()
        {
            _title.text = _data.Title;
            _preview.sprite = _data.Preview;
        }

        [Inject]
        public void Construct(IGameStateMachine stateMachine)
        {
            _gameStateMachine = stateMachine;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(() => _gameStateMachine.Enter<LoadLevelState, LevelStaticData>(_data));
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(() => _gameStateMachine.Enter<LoadLevelState, LevelStaticData>(_data));
        }
    }
}