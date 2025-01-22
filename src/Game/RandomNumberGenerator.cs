namespace Game;

public interface IRandomNumberGenerator
{
    int GenerateInclusive(int min, int max);
}

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new();
    public int GenerateInclusive(int min, int max) => _random.Next(min, max + 1);
}