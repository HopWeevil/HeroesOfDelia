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
    }
}