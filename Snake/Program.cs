
using Snake.Core;

var game = new Game();
game.Start();

Task.Run(() =>
{
    while (true)
    {
        var key = Console.ReadKey();
        if(key.Key == ConsoleKey.RightArrow)
            game.SetDirection(Direction.Right);
        else if(key.Key == ConsoleKey.LeftArrow)
            game.SetDirection(Direction.Left);
        else if(key.Key == ConsoleKey.UpArrow)
            game.SetDirection(Direction.Up);
        else if(key.Key == ConsoleKey.DownArrow)
            game.SetDirection(Direction.Down);
        else if(key.Key == ConsoleKey.Spacebar)
            game.GrowSnakeBy(1);
    }
});

while (true)
{
    if(!game.IsFoodExist(out Position foodPos))
        game.GenerateFood(new PosGenerator());
    PrintBoard(game);
    Thread.Sleep(200);
    game.Move(1);
}

void PrintBoard(Game game)
{
    Console.Clear();
    for (var i = 0; i < game.BoardL; i++)
    {
        for (var j = 0; j < game.BoardW; j++)
        {
            if(game.Board[j,i] == PixelType.Body)
                Console.Write("🔵");
            else if(game.Board[j,i]== PixelType.Food)
                Console.Write("🐸");
            else
                Console.Write("⬜");
        }

        Console.WriteLine();
    }
}