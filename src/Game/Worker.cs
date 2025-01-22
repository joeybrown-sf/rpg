using Microsoft.Extensions.Options;

namespace Game;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRpgEnvironment _env;
    private readonly RpgConfiguration _config;
    private readonly Monster _monster;
    private readonly Hero _hero;

    public Worker(ILogger<Worker> logger, IOptions<RpgConfiguration> options,
        IRandomNumberGenerator randomNumberGenerator, IRpgEnvironment env)
    {
        _logger = logger;
        _env = env;
        _config = options.Value;
        _monster = new Monster(randomNumberGenerator, _config.MonsterHealth, _config.MonsterMinAttack,
            _config.MonsterMaxAttack);
        _hero = new Hero(randomNumberGenerator, _config.HeroHealth, _config.HeroMinAttack, _config.HeroMaxAttack);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await PlayGame(stoppingToken);
    }

    public async Task PlayGame(CancellationToken stoppingToken)
    {
        ICharacter attacker = _hero;
        ICharacter target = _monster;
        while (!stoppingToken.IsCancellationRequested)
        {
            var attackResult = attacker.Attack(target);
            _logger.LogAttackResult(attacker, target, attackResult);
            if (attackResult.Fatality)
            {
                _logger.LogFatality(attacker, target);
                break;
            }

            await Task.Delay(_config.AttackDelay, stoppingToken);
            (attacker, target) = (target, attacker);
        }

        _logger.LogGameOver();
        _env.Exit();
    }
}

public static class WorkerLoggerExtensions
{
    public static void LogAttackResult(this ILogger<Worker> logger, ICharacter attacker, ICharacter target,
        AttackResult result)
    {
        logger.LogInformation(
            "{attacker} attacked {target}. {target} took {damage} points of damage. {target} has {targetHealth} health points remaining.",
            attacker.Descriptor(),
            target.Descriptor(),
            target.Descriptor(),
            result.Damage,
            target.Descriptor(),
            result.RemainingHealth
        );
    }

    public static void LogFatality(this ILogger<Worker> logger, ICharacter attacker, ICharacter target)
    {
        logger.LogInformation(
            "{attacker} killed {target}.", attacker.Descriptor(), target.Descriptor()
        );
    }

    public static void LogGameOver(this ILogger<Worker> logger)
    {
        logger.LogInformation("Game over man, GAME OVER!");
    }
}