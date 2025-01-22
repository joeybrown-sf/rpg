using NSubstitute;

namespace Game.Test;

public class CharacterTests
{
    [InlineData(2, 3)]
    [InlineData(6, 9)]
    [Theory]
    public void Hero_Offensive_Attack_Should_Be_Within_Min_Max(int minAttack, int maxAttack)
    {
        var target = Substitute.For<ICharacter>();
        var random = Substitute.For<IRandomNumberGenerator>();
        random.GenerateInclusive(Arg.Any<int>(), Arg.Any<int>()).Returns(1);
        var attacker = new Hero(random, health: 10, minAttackDamage: minAttack, maxAttackDamage: maxAttack);

        attacker.Attack(target);
        random.Received(1).GenerateInclusive(Arg.Is<int>(x => x == minAttack), Arg.Is<int>(x => x == maxAttack));
    }
    
    [InlineData(2, 3)]
    [InlineData(6, 9)]
    [Theory]
    public void Monster_Offensive_Attack_Should_Be_Within_Min_Max(int minAttack, int maxAttack)
    {
        var target = Substitute.For<ICharacter>();
        var random = Substitute.For<IRandomNumberGenerator>();
        random.GenerateInclusive(Arg.Any<int>(), Arg.Any<int>()).Returns(1);
        var attacker = new Monster(random, health: 10, minAttackDamage: minAttack, maxAttackDamage: maxAttack);

        attacker.Attack(target);
        random.Received(1).GenerateInclusive(Arg.Is<int>(x => x == minAttack), Arg.Is<int>(x => x == maxAttack));
    }
    
    [InlineData(20, 3, 17)]
    [InlineData(20, 40, 0)]
    [Theory]
    public void Hero_Attacked_Health_Should_Be_Proportional(int initialHealth, int attackDamage, int expectedHealth)
    {
        var random = Substitute.For<IRandomNumberGenerator>();
        random.GenerateInclusive(Arg.Any<int>(), Arg.Any<int>()).Returns(attackDamage);
        
        var target = new Hero(random, health: initialHealth, minAttackDamage: 10, maxAttackDamage: 10);
        var result = target.Attacked(attackDamage);
        Assert.Equal(expectedHealth, result.RemainingHealth);
        Assert.Equal(attackDamage, result.Damage);

        if (expectedHealth == 0)
        {
            Assert.True(result.Fatality);
        }
    }
    
    [InlineData(20, 3, 17)]
    [InlineData(20, 40, 0)]
    [Theory]
    public void Monster_Attacked_Health_Should_Be_Proportional(int initialHealth, int attackDamage, int expectedHealth)
    {
        var random = Substitute.For<IRandomNumberGenerator>();
        random.GenerateInclusive(Arg.Any<int>(), Arg.Any<int>()).Returns(attackDamage);
        
        var target = new Monster(random, health: initialHealth, minAttackDamage: 10, maxAttackDamage: 10);
        var result = target.Attacked(attackDamage);
        Assert.Equal(expectedHealth, result.RemainingHealth);
        Assert.Equal(attackDamage, result.Damage);

        if (expectedHealth == 0)
        {
            Assert.True(result.Fatality);
        }
    }

    [Fact]
    public void Hero_Descriptor()
    {
        var random = Substitute.For<IRandomNumberGenerator>();
        var sut = new Hero(random, 1, 1, 1);
        Assert.Equal("Hero", sut.Descriptor());
    }
    
    [Fact]
    public void Monster_Descriptor()
    {
        var random = Substitute.For<IRandomNumberGenerator>();
        var sut = new Monster(random, 1, 1, 1);
        Assert.Equal("Monster", sut.Descriptor());
    }
}