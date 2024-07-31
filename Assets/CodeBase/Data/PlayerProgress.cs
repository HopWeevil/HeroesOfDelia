using CodeBase.Enums;
using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        private HeroTypeId _selectedHeroId;
        public Action<HeroTypeId> HeroSelected;

        public HeroTypeId SelectedHero => _selectedHeroId;
        public void ChangeHero(HeroTypeId typeId)
        {
            _selectedHeroId = typeId;
            HeroSelected?.Invoke(typeId);
        }

        public State HeroState;
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;
        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
        }
    }
}