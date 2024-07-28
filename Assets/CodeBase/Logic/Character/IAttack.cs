using CodeBase.SO;

public interface IAttack
{
    void InitializeStats(EnemyStaticData enemyStaticData);
    void TryAttack();
}
