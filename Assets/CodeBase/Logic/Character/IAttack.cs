using CodeBase.SO;

public interface IAttack
{
    void InitializeStats(CharacterStaticData characterStaticData);
    void TryAttack();
}
