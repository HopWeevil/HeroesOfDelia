using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
        public PlayerEconomyData Economy { get; set; }
        public PlayerInventory Inventory { get; set; }

    }
}