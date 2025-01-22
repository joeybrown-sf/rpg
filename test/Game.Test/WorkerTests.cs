using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Game.Test;

public class TestLogger : ILogger<Worker>
{
    private readonly List<string> _log = [];
    
    public List<string> GetLog() => _log;
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _log.Add(formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }
}

public class WorkerTests
{
    [Fact]
    public async Task Worker_Should_Play_Game_As_Expected()
    {
        var configuration = new RpgConfiguration
        {
            AttackDelay = TimeSpan.Zero,
            HeroHealth = 20,
            HeroMaxAttack = 10,
            HeroMinAttack = 0,
            MonsterHealth = 20,
            MonsterMaxAttack = 10,
            MonsterMinAttack = 0
        };

        var logger = new TestLogger();
        
        var options = Substitute.For<IOptions<RpgConfiguration>>();
        options.Value.Returns(configuration);
        var random = Substitute.For<IRandomNumberGenerator>();
        random.GenerateInclusive(Arg.Any<int>(), Arg.Any<int>()).Returns(5);

        var env = Substitute.For<IRpgEnvironment>();
        
        var worker = new Worker(logger, options, random, env);
        await worker.PlayGame(CancellationToken.None);
        
        var logs = logger.GetLog();

        Assert.Equal(9, logs.Count);
        Assert.Equal("Hero attacked Monster. Monster took 5 points of damage. Monster has 15 health points remaining.", logs[0]);
        Assert.Equal("Monster attacked Hero. Hero took 5 points of damage. Hero has 15 health points remaining.", logs[1]);
        Assert.Equal("Hero attacked Monster. Monster took 5 points of damage. Monster has 10 health points remaining.", logs[2]);
        Assert.Equal("Monster attacked Hero. Hero took 5 points of damage. Hero has 10 health points remaining.", logs[3]);
        Assert.Equal("Hero attacked Monster. Monster took 5 points of damage. Monster has 5 health points remaining.", logs[4]);
        Assert.Equal("Monster attacked Hero. Hero took 5 points of damage. Hero has 5 health points remaining.", logs[5]);
        Assert.Equal("Hero attacked Monster. Monster took 5 points of damage. Monster has 0 health points remaining.", logs[6]);
        Assert.Equal("Hero killed Monster.", logs[7]);
        Assert.Equal("Game over man, GAME OVER!", logs[8]);
        
        env.Received().Exit();
    }
}