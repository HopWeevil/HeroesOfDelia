namespace CodeBase.Data
{
    public class Stats
    {
        public float Hp { get; private set; }
        public float Damage { get; private set; }
        public float MoveSpeed { get; private set; }
        public float Armor { get; private set; }
        public float AttackCooldown { get; private set; }

        public float AttackSplash { get; private set; }
        public float AttackDistance { get; private set; }

        public Stats(int hp, float damage, float moveSpeed, float armor, float attackCooldown, float attackSplash, float attackDistance)
        {
            Hp = hp;
            Damage = damage;
            MoveSpeed = moveSpeed;
            Armor = armor;
            AttackCooldown = attackCooldown;
            AttackSplash = attackSplash;
            AttackDistance = attackDistance;
        }

        public void ApplyStatsBonuses(StatsBonus[] bonuses, int itemLevel)
        {
            foreach (var bonus in bonuses)
            {
                CalculateBonusesEffects(bonus, itemLevel, true);
            }
        }

        public void CalculateBonusesEffects(StatsBonus bonus, int level, bool isApplying)
        {
            switch (bonus.Type)
            {
                case BonusType.Damage:
                    Damage += bonus.Value * level;
                    break;
                case BonusType.MoveSpeed:
                    MoveSpeed += bonus.Value * level;
                    break;
                case BonusType.Armor:
                    Armor += bonus.Value * level;
                    break;
                case BonusType.AttackCooldown:
                    AttackCooldown += bonus.Value * level;
                    break;
                case BonusType.AttackDistance:
                    AttackDistance += bonus.Value * level;
                    break;
                case BonusType.Health:
                    Hp += bonus.Value * level;
                    break;
                case BonusType.AttackSplash:
                    AttackSplash += bonus.Value * level;
                    break;
            }
        }
    }
}