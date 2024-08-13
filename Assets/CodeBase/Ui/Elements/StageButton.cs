using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StageButton : MonoBehaviour
{
    private Button _button;
    private IGameStateMachine _gameStateMachine;

    [Inject]
    private void Construct(IGameStateMachine stateMachine)
    {
        _gameStateMachine = stateMachine;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _gameStateMachine.Enter<LoadLevelState, string>("Location1"));
    }
}
