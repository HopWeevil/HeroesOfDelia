using UnityEngine;

namespace CodeBase.Enemy.States
{
    public abstract class EnemyState : MonoBehaviour
    {
        protected private GameObject _target;

        public void Initialize(GameObject target)
        {
            _target = target;
        }

        public abstract void Enter();
        public abstract void Exit();
        
        public abstract void Tick();
        public abstract bool ShouldTransit();
    }
}