using System;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': Position.Y -= 1; break;
            case 'D': Position.Y += 1; break;
            case 'L': Position.X -= 1; break;
            case 'R': Position.X += 1; break;
        }
    }
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant = "-")
    {
        Occupant = occupant;
    }
}

public class Board
{
    private const int Size = 6;
    private Cell[,] grid;
    private static Random random = new Random();

    public Board()
    {
        grid = new Cell[Size, Size];
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                grid[y, x] = new Cell();
            }
        }
        PlaceObstacles();
        PlaceGems();
    }

    private void PlaceObstacles()
    {
        int numObstacles = random.Next(5, 11);
        for (int i = 0; i < numObstacles; i++)
        {
            int x, y;
            do
            {
                x = random.Next(Size);
                y = random.Next(Size);
            } while (grid[y, x].Occupant != "-");
            grid[y, x].Occupant = "O";
        }
    }

    private void PlaceGems()
    {
        int numGems = random.Next(5, 11);
        for (int i = 0; i < numGems; i++)
        {
            int x, y;
            do
            {
                x = random.Next(Size);
                y = random.Next(Size);
            } while (grid[y, x].Occupant != "-");
            grid[y, x].Occupant = "G";
        }
    }

    public void Display(Player player1, Player player2)
    {
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                if (player1.Position.X == x && player1.Position.Y == y)
                {
                    Console.Write("P1 ");
                }
                else if (player2.Position.X == x && player2.Position.Y == y)
                {
                    Console.Write("P2 ");
                }
                else
                {
                    Console.Write($"{grid[y, x].Occupant} ");
                }
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X, newY = player.Position.Y;
        switch (direction)
        {
            case 'U': newY -= 1; break;
            case 'D': newY += 1; break;
            case 'L': newX -= 1; break;
            case 'R': newX += 1; break;
        }

        return newX >= 0 && newX < Size && newY >= 0 && newY < Size && grid[newY, newX].Occupant != "O";
    }

    public void CollectGem(Player player)
    {
        if (grid[player.Position.Y, player.Position.X].Occupant == "G")
        {
            player.GemCount++;
            grid[player.Position.Y, player.Position.X].Occupant = "-";
        }
    }
}

public class Game
{
    private Board board;
    private Player player1;
    private Player player2;
    private Player currentTurn;
    private int totalTurns;

    public Game()
    {
        board = new Board();
        player1 = new Player("P1", new Position(0, 0));
        player2 = new Player("P2", new Position(5, 5));
        currentTurn = player1;
        totalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            board.Display(player1, player2);
            Console.WriteLine($"{currentTurn.Name}'s turn. Move (U/D/L/R): ");
            char move = Console.ReadKey().KeyChar;
            Console.WriteLine();
            move = char.ToUpper(move);
            while (!board.IsValidMove(currentTurn, move))
            {
                Console.WriteLine("Invalid move. Try again (U/D/L/R): ");
                move = Console.ReadKey().KeyChar;
                Console.WriteLine();
                move = char.ToUpper(move);
            }
            currentTurn.Move(move);
            board.CollectGem(currentTurn);
            SwitchTurn();
            totalTurns++;
        }

        AnnounceWinner();
    }

    private void SwitchTurn()
    {
        currentTurn = currentTurn == player1 ? player2 : player1;
    }

    private bool IsGameOver()
    {
        return totalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        board.Display(player1, player2);
        Console.WriteLine($"Game Over! P1 collected {player1.GemCount} gems. P2 collected {player2.GemCount} gems.");
        if (player1.GemCount > player2.GemCount)
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (player1.GemCount < player2.GemCount)
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

public class Program
{
    public static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}
