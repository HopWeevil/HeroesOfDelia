using CodeBase.Data;

namespace CodeBase.Logic
{
    public interface IStatsReceiver
    {
        void Receive(Stats stats);
    }
}