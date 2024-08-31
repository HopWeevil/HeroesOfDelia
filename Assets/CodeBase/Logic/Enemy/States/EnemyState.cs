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

        public virtual void Enter() { }
        public virtual void Exit() { }
        
        public virtual void Tick() { }
        public abstract bool ShouldTransit();
    }
}