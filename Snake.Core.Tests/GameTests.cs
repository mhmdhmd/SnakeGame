using NSubstitute;

namespace Snake.Core.Tests;

[TestFixture]
public class GameTests
{
    [Test]
    public void Game_Initial_EmptyBoard()
    {
        var game = new Game();

        var isEmpty = game.Initial();
        
        Assert.IsTrue(isEmpty);
    }

    [Test]
    public void Game_Start_SnakeHas1BodyAtTopLeftCorner()
    {
        var game = new Game();

        game.Start();
        var snakeLen = game.Snake.Length;
        Assert.That(1, Is.EqualTo(snakeLen));

        var expectedPosition = new Position(0, 0);
        var actualPosition = game.Snake[0];
        
        Assert.That(actualPosition, Is.EqualTo(expectedPosition));
    }

    [Test]
    public void Game_Move_MoveSnakeBodyToSelectedDirection()
    {
        var game = new Game();
        game.Start();
        
        const int moveStep = 1;

        game.SetDirection(Direction.Right);
        var lastPosition = game.Snake[0];
        game.Move(moveStep);
        var expectedPosition = new Position(lastPosition.X + 1, lastPosition.Y);
        var actualPosition = game.Snake[0];
        Assert.That(actualPosition, Is.EqualTo(expectedPosition));
        
        game.SetDirection(Direction.Bottom);
        lastPosition = game.Snake[0];
        game.Move(moveStep);
        expectedPosition = new Position(lastPosition.X, lastPosition.Y + 1);
        actualPosition = game.Snake[0];
        Assert.That(actualPosition, Is.EqualTo(expectedPosition));
        
        game.SetDirection(Direction.Left);
        lastPosition = game.Snake[0];
        game.Move(moveStep);
        expectedPosition = new Position(lastPosition.X-1, lastPosition.Y);
        actualPosition = game.Snake[0];
        Assert.That(actualPosition, Is.EqualTo(expectedPosition));
        
        game.SetDirection(Direction.Top);
        lastPosition = game.Snake[0];
        game.Move(moveStep);
        expectedPosition = new Position(lastPosition.X, lastPosition.Y-1);
        actualPosition = game.Snake[0];
        Assert.That(actualPosition, Is.EqualTo(expectedPosition));
    }

    [Test]
    public void Game_AddRandomFood()
    {
        var game = new Game();
        game.Start();

        IPosGenerator posGenerator = Substitute.For<IPosGenerator>();
        var expectedFoodPos = new Position(10, 10);
        posGenerator.Generate(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(expectedFoodPos);
        Position actualFoodPos = game.GenerateFood(posGenerator);
        
        Assert.That(actualFoodPos, Is.EqualTo(expectedFoodPos));

        PixelType boardPixel = game.GetBoardPixelAt(expectedFoodPos);
        Assert.That(boardPixel, Is.EqualTo(PixelType.Food));
    }

    [Test]
    public void Game_MoveSnakeToEatFood_GrowSnakeByOne()
    {
        var game = new Game();
        game.Start();
        
        IPosGenerator posGenerator = Substitute.For<IPosGenerator>();
        var foodPos = new Position(10, 10);
        posGenerator.Generate(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(foodPos);
        Position actualFoodPos = game.GenerateFood(posGenerator);
        
        game.SetDirection(Direction.Right);
        game.Move(10);
        game.SetDirection( Direction.Bottom);
        game.Move(10);

        var snakeHeadPos = game.Snake[0];
        Assert.That(snakeHeadPos, Is.EqualTo(actualFoodPos));

        game.RemoveFood(foodPos);
        Assert.That(game.GetBoardPixelAt(actualFoodPos), Is.EqualTo(PixelType.Empty));
        
        game.GrowSnakeBy(1);
        Assert.That(game.Snake.Length, Is.EqualTo(2));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(9,10)));
    }

    [Test]
    public void Game_MoveSnake_GrowIt_CheckForMovement()
    {
        var game = new Game();
        game.Start();
        
        game.SetDirection(Direction.Right);
        game.Move(10);
        game.GrowSnakeBy(5);

        game.SetDirection( Direction.Bottom);
        game.Move(1);
        Assert.That(game.Snake[0], Is.EqualTo(new Position(10,1)));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(10,0)));
        Assert.That(game.Snake[2], Is.EqualTo(new Position(9,0)));
        
        game.Move(1);
        Assert.That(game.Snake[0], Is.EqualTo(new Position(10,2)));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(10,1)));
        Assert.That(game.Snake[2], Is.EqualTo(new Position(10,0)));
        
        game.SetDirection(Direction.Left);
        game.Move(1);
        Assert.That(game.Snake[0], Is.EqualTo(new Position(9,2)));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(10,2)));
        Assert.That(game.Snake[2], Is.EqualTo(new Position(10,1)));
        
        game.Move(1);
        Assert.That(game.Snake[0], Is.EqualTo(new Position(8,2)));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(9,2)));
        Assert.That(game.Snake[2], Is.EqualTo(new Position(10,2)));
        
        game.SetDirection(Direction.Bottom);
        game.Move(1);
        Assert.That(game.Snake[0], Is.EqualTo(new Position(8,3)));
        Assert.That(game.Snake[1], Is.EqualTo(new Position(8,2)));
        Assert.That(game.Snake[2], Is.EqualTo(new Position(9,2)));
    }

    [Test]
    public void Game_MoveSnake_BoardTrackSnakePos()
    {
        var game = new Game();
        game.Start();

        PixelType posOfSnake = game.GetBoardPixelAt(game.Snake[0]);
        Assert.That(posOfSnake, Is.EqualTo(PixelType.Body));
        
        game.SetDirection(Direction.Right);
        game.Move(10);
        game.GrowSnakeBy(5);
        posOfSnake = game.GetBoardPixelAt(game.Snake[2]);
        Assert.That(posOfSnake, Is.EqualTo(PixelType.Body));
        
        game.SetDirection( Direction.Bottom);
        game.Move(1);
        posOfSnake = game.GetBoardPixelAt(game.Snake[0]);
        Assert.That(posOfSnake, Is.EqualTo(PixelType.Body));
    }

    [Test]
    public void Game_GenerateFoodAndEmptyBoard_FoodShouldNotDisappear()
    {
        var game = new Game();
        game.Start();

        IPosGenerator posGenerator = Substitute.For<IPosGenerator>();
        var expectedFoodPos = new Position(10, 10);
        posGenerator.Generate(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(expectedFoodPos);
        Position actualFoodPos = game.GenerateFood(posGenerator);
        Assert.That(actualFoodPos, Is.EqualTo(expectedFoodPos));

        game.ProjectSnakePosOnBoard();
        PixelType foodPosPixelType = game.GetBoardPixelAt(actualFoodPos);
        Assert.That(foodPosPixelType, Is.EqualTo(PixelType.Food));
    }

    [Test]
    public void Game_FoodExist_ReturnTrue()
    {
        var game = new Game();
        game.Start();

        bool isFoodExistOnBoard = game.IsFoodExist();
        Assert.IsFalse(isFoodExistOnBoard);

        game.GenerateFood(new PosGenerator());
        isFoodExistOnBoard = game.IsFoodExist();
        Assert.IsTrue(isFoodExistOnBoard);
    }
}