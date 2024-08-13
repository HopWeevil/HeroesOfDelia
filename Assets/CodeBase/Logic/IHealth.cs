using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        event Action HealthChanged;

        void TakeDamage(float damage);

        float GetCurrent();
        float GetMax();
    }
}