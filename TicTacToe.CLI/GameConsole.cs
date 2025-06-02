using TicTacToe.Domain;

namespace TicTacToe.CLI;

/// <summary>
/// Handles console-based user interface for the Tic Tac Toe game.
/// </summary>
public class GameConsole
{
    private readonly TicTacToeGame _game;

    /// <summary>
    /// Initializes a new instance of the GameConsole class.
    /// </summary>
    /// <param name="game">The game instance to interact with.</param>
    public GameConsole(TicTacToeGame game)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
    }

    /// <summary>
    /// Displays the welcome message and game instructions.
    /// </summary>
    public void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("🎮 Welcome to Tic Tac Toe! 🎮");
        Console.WriteLine();
        Console.WriteLine("How to play:");
        Console.WriteLine("• Enter coordinates as 'row,col' (e.g., '1,2')");
        Console.WriteLine("• Coordinates range from 0,0 (top-left) to 2,2 (bottom-right)");
        Console.WriteLine("• X goes first, then players alternate");
        Console.WriteLine("• Get 3 in a row (horizontal, vertical, or diagonal) to win!");
        Console.WriteLine();
    }

    /// <summary>
    /// Displays the current game board with position indicators.
    /// </summary>
    public void DisplayBoard()
    {
        var state = _game.GetGameState();
        var board = state.GetBoard();

        Console.WriteLine("Current Board:");
        Console.WriteLine("   0   1   2");

        for (int row = 0; row < 3; row++)
        {
            Console.Write($"{row}  ");
            for (int col = 0; col < 3; col++)
            {
                var cell = board[row, col];
                var display = cell == ' ' ? '·' : cell;
                Console.Write(display);

                if (col < 2)
                    Console.Write(" | ");
            }
            Console.WriteLine();

            if (row < 2)
                Console.WriteLine("   ---------");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Displays the current game status and information.
    /// </summary>
    public void DisplayGameStatus()
    {
        var state = _game.GetGameState();

        Console.WriteLine($"Game Status: {GetStatusText(state.Status)}");

        if (state.Status == GameStatus.InProgress)
        {
            Console.WriteLine($"Current Player: {state.CurrentPlayer}");
        }

        Console.WriteLine($"Moves Made: {state.MoveHistory.Count}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prompts the user for their next move and returns the input.
    /// </summary>
    /// <returns>The user's input string.</returns>
    public string? GetMoveInput()
    {
        var state = _game.GetGameState();
        Console.Write($"Player {state.CurrentPlayer}, enter your move (row,col): ");
        return Console.ReadLine();
    }

    /// <summary>
    /// Displays an error message for invalid moves.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a success message for valid moves.
    /// </summary>
    /// <param name="row">The row of the move.</param>
    /// <param name="col">The column of the move.</param>
    /// <param name="player">The player who made the move.</param>
    public void ShowMoveSuccess(int row, int col, Player player)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Player {player} placed at ({row},{col})");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Displays the final game result.
    /// </summary>
    public void ShowGameResult()
    {
        var state = _game.GetGameState();

        Console.ForegroundColor = state.Status switch
        {
            GameStatus.X_Won or GameStatus.O_Won => ConsoleColor.Yellow,
            GameStatus.Draw => ConsoleColor.Cyan,
            _ => ConsoleColor.White
        };

        var message = state.Status switch
        {
            GameStatus.X_Won => "🎉 Player X Wins! 🎉",
            GameStatus.O_Won => "🎉 Player O Wins! 🎉",
            GameStatus.Draw => "🤝 It's a Draw! 🤝",
            _ => "Game in progress..."
        };

        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Prompts the user to play again and returns their choice.
    /// </summary>
    /// <returns>True if the user wants to play again, false otherwise.</returns>
    public bool PromptPlayAgain()
    {
        Console.Write("Would you like to play again? (y/n): ");
        var input = Console.ReadLine()?.Trim().ToLowerInvariant();
        return input == "y" || input == "yes";
    }

    /// <summary>
    /// Waits for the user to press any key to continue.
    /// </summary>
    public void WaitForContinue()
    {
        Console.WriteLine("Press any key to continue...");
        try
        {
            Console.ReadKey(true);
        }
        catch (InvalidOperationException)
        {
            // Handle case when console input is redirected
            Console.ReadLine();
        }
    }

    private static string GetStatusText(GameStatus status) => status switch
    {
        GameStatus.InProgress => "In Progress",
        GameStatus.X_Won => "X Won",
        GameStatus.O_Won => "O Won",
        GameStatus.Draw => "Draw",
        _ => "Unknown"
    };
}