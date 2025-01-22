namespace Game;

public class AttackResult
{
    public int RemainingHealth { get; init; }
    public int Damage { get; init; }
    public bool Fatality => RemainingHealth <= 0;
}

public interface ICharacter
{
    AttackResult Attack(ICharacter target);
    AttackResult Attacked(int damage);
    string Descriptor();
}

public abstract class Character(IRandomNumberGenerator random, int health, int minAttackDamage, int maxAttackDamage)
    : ICharacter
{
    private int _health = health;

    public AttackResult Attack(ICharacter character)
    {
        var damage = random.GenerateInclusive(minAttackDamage, maxAttackDamage);
        return character.Attacked(damage);
    }

    public AttackResult Attacked(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
        }

        return new AttackResult
        {
            Damage = damage,
            RemainingHealth = _health
        };
    }

    public abstract string Descriptor();
}

public class Hero(IRandomNumberGenerator random, int health, int minAttackDamage, int maxAttackDamage)
    : Character(random, health, minAttackDamage, maxAttackDamage)
{
    public override string Descriptor() => "Hero";
}

public class Monster(IRandomNumberGenerator random, int health, int minAttackDamage, int maxAttackDamage)
    : Character(random, health, minAttackDamage, maxAttackDamage)
{
    public override string Descriptor() => "Monster";
}