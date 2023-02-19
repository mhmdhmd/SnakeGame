namespace Snake.Core;

public struct Position
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position left, Position right)
    {
        return (left.X == right.X) && (left.Y == right.Y);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }

    public void ChangeXBy(int step)
    {
        X += step;
    }
    
    public void ChangeYBy(int step)
    {
        Y += step;
    }

    public void SetX(int x)
    {
        X = x;
    }

    public void SetY(int y)
    {
        Y = y;
    }
}