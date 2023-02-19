namespace Snake.Core;

public class PosGenerator : IPosGenerator
{
    private readonly Random _random;

    public PosGenerator()
    {
        _random = new Random();
    }
    public Position Generate(int min, int max)
    {
        return new Position(_random.Next(min, max), _random.Next(min, max));
    }
}