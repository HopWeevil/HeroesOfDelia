using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        PlayerProgress Progress { get; set; }
        PlayerEconomyData Economy { get; set; }

        PlayerInventory Inventory { get; set; }
    }
}