namespace Snake.Core;

public class Game
{
    private PixelType[,] _board;
    private Position[] _snake;
    private Direction _direction;
    private const int BoardL = 20;
    private const int BoardW = 20;

    public Game()
    {
        _board = new PixelType[BoardL, BoardW];
        Initial();
    }

    public Position[] Snake => _snake;

    public bool Initial()
    {
        for (var i = 0; i < BoardL; i++)
        {
            for (var j = 0; j < BoardW; j++)
            {
                _board[i, j] = PixelType.Empty;
            }
        }

        return true;
    }

    public void Start()
    {
        _snake = new Position[1];
        _snake[0] = new Position(0, 0);
    }

    public void SetDirection(Direction dir)
    {
        _direction = dir;
    }

    public void Move(int moveStep)
    {
        MoveAllBodyPartToNextPartPos();
        switch (_direction)
        {
            case Direction.Right:
                _snake[0].ChangeXBy(moveStep);
                break;
            case Direction.Left:
                _snake[0].ChangeXBy(-moveStep);
                break;
            case Direction.Bottom:
                _snake[0].ChangeYBy(moveStep);
                break;
            case Direction.Top:
                _snake[0].ChangeYBy(-moveStep);
                break;
        }
    }

    private void MoveAllBodyPartToNextPartPos()
    {
        for (int i = _snake.Length - 2; i >= 0; i--)
        {
            _snake[i + 1] = _snake[i];
        }
    }

    public Position GenerateFood(IPosGenerator posGenerator)
    {
        var foodPos = posGenerator.Generate(BoardL, BoardW);

        if (IsOverSnake(foodPos))
            foodPos = GenerateFood(posGenerator);

        _board[foodPos.X, foodPos.Y] = PixelType.Food;
        return foodPos;
    }

    private bool IsOverSnake(Position foodPos)
    {
        foreach (var snakeBodyPos in _snake)
        {
            if (snakeBodyPos == foodPos)
                return true;
        }

        return false;
    }

    public PixelType GetBoardPixelAt(Position pos)
    {
        return _board[pos.X, pos.Y];
    }

    public void RemoveFood(Position foodPos)
    {
        _board[foodPos.X, foodPos.Y] = PixelType.Empty;
    }

    public void GrowSnakeBy(int count)
    {
        var currentLen = _snake.Length;
        var newLen = currentLen + count;

        Position[] newSnake = new Position[newLen];
        
        Array.Copy(_snake,newSnake,currentLen);

        _snake = newSnake;
        
        for (var i = 0; i < _snake.Length-1; i++)
        {
            _snake[i + 1].SetX(_snake[i].X - 1);
            _snake[i + 1].SetY(_snake[i].Y);
        }
    }
}