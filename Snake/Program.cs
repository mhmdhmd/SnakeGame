
using Snake.Core;

var game = new Game();
game.Start();
game.SetDirection(Direction.Bottom);

while (true)
{
    if(!game.IsFoodExist())
        game.GenerateFood(new PosGenerator());
    PrintBoard(game);
    Thread.Sleep(TimeSpan.FromSeconds(1));
    game.Move(1);
}

void PrintBoard(Game game)
{
    Console.Clear();
    for (var i = 0; i < game.BoardL; i++)
    {
        for (var j = 0; j < game.BoardW; j++)
        {
            if(game.Board[i,j] == PixelType.Body)
                Console.Write("🔵");
            else if(game.Board[i,j]== PixelType.Food)
                Console.Write("🐸");
            else
                Console.Write("⬜");
        }

        Console.WriteLine();
    }
}