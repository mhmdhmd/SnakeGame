namespace Snake.Core;

public class Game
{
    public PixelType[,] Board { get; }
    private Position[] _snake;
    private Direction _direction;
    public int BoardL => 20;
    public int BoardW => 20;

    public Game()
    {
        Board = new PixelType[BoardL, BoardW];
        Initial();
    }

    public Position[] Snake => _snake;

    public bool Initial(bool includeFood = false)
    {
        for (var i = 0; i < BoardL; i++)
        {
            for (var j = 0; j < BoardW; j++)
            {
                if (includeFood && Board[i, j] == PixelType.Food) continue;
                
                Board[i, j] = PixelType.Empty;
            }
        }

        return true;
    }

    public void Start()
    {
        _snake = new Position[1];
        _snake[0] = new Position(0, 0);

        ProjectSnakePosOnBoard();
    }

    public void ProjectSnakePosOnBoard()
    {
        Initial(true);
        foreach (var snakePos in _snake)
        {
            Board[snakePos.X, snakePos.Y] = PixelType.Body;
        }
    }

    public void SetDirection(Direction dir)
    {
        _direction = dir;
    }

    public void Move(int moveStep)
    {
        MoveAllBodyPartToNextPartPos();
        var isFoodExist = IsFoodExist(out Position foodPos);
        switch (_direction)
        {
            case Direction.Right:
                _snake[0].ChangeXBy(moveStep);
                break;
            case Direction.Left:
                _snake[0].ChangeXBy(-moveStep);
                break;
            case Direction.Down:
                _snake[0].ChangeYBy(moveStep);
                break;
            case Direction.Up:
                _snake[0].ChangeYBy(-moveStep);
                break;
        }
        
        if(isFoodExist && _snake[0] == foodPos) GrowSnakeBy(1);
        
        ProjectSnakePosOnBoard();
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
        var foodPos = posGenerator.Generate(1, BoardW-1);

        if (IsOverSnake(foodPos))
            foodPos = GenerateFood(posGenerator);

        Board[foodPos.X, foodPos.Y] = PixelType.Food;
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
        return Board[pos.X, pos.Y];
    }

    public void RemoveFood(Position foodPos)
    {
        Board[foodPos.X, foodPos.Y] = PixelType.Empty;
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


        ProjectSnakePosOnBoard();
    }

    public bool IsFoodExist(out Position foodPos)
    {
        for (var i = 0; i < BoardL; i++)
        {
            for (var j = 0; j < BoardW; j++)
            {
                if (Board[i, j] == PixelType.Food)
                {
                    foodPos = new Position(i, j);
                    return true;
                }
            }
        }

        foodPos = new Position(0, 0);
        return false;
    }
}