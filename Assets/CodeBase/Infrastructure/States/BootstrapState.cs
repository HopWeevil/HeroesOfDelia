using CodeBase.Infrastructure.Sceneloader;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;

        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void EnterLoadLevel()
        {
           // _stateMachine.Enter<LoadProgressState>();
            //_stateMachine.Enter<LoadMetaState>();
        }
    }
}