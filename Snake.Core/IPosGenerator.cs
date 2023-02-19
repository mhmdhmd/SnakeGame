namespace Snake.Core;

public interface IPosGenerator
{
    Position Generate(int min, int max);
}