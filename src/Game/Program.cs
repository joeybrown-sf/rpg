using Game;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddSerilog();
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
    builder.Services.AddSingleton<IRpgEnvironment, RpgEnvironment>();
    builder.Services.Configure<RpgConfiguration>(x =>
    {
        x.AttackDelay = builder.Configuration.GetValue<TimeSpan>("AttackDelay");
        x.HeroHealth = builder.Configuration.GetValue<int>("Hero:Health");
        x.HeroMinAttack = builder.Configuration.GetValue<int>("Hero:MinAttack");
        x.HeroMaxAttack = builder.Configuration.GetValue<int>("Hero:MaxAttack");
        x.MonsterHealth = builder.Configuration.GetValue<int>("Monster:Health");
        x.MonsterMinAttack = builder.Configuration.GetValue<int>("Monster:MinAttack");
        x.MonsterMaxAttack = builder.Configuration.GetValue<int>("Monster:MaxAttack");
    });

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}