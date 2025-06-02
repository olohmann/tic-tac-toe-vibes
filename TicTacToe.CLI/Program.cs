using TicTacToe.CLI;
using TicTacToe.Domain;

/// <summary>
/// Main entry point for the Tic Tac Toe CLI game.
/// </summary>
public class Program
{
    /// <summary>
    /// Main application entry point.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        var game = new TicTacToeGame();
        var console = new GameConsole(game);

        console.ShowWelcome();

        bool playAgain = true;
        while (playAgain)
        {
            PlayGame(game, console);
            playAgain = console.PromptPlayAgain();

            if (playAgain)
            {
                game.StartNewGame();
                console.ShowWelcome();
            }
        }

        Console.WriteLine("Thanks for playing! 👋");
    }

    /// <summary>
    /// Plays a single game of Tic Tac Toe.
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="console">The console interface.</param>
    private static void PlayGame(TicTacToeGame game, GameConsole console)
    {
        while (game.GetGameState().Status == GameStatus.InProgress)
        {
            console.DisplayBoard();
            console.DisplayGameStatus();

            var input = console.GetMoveInput();

            if (!InputParser.TryParseCoordinates(input, out int row, out int col))
            {
                console.ShowError("Invalid input format. Please enter coordinates as 'row,col' (e.g., '1,2').");
                continue;
            }

            var currentPlayer = game.GetGameState().CurrentPlayer;
            if (game.MakeMove(row, col))
            {
                console.ShowMoveSuccess(row, col, currentPlayer);
            }
            else
            {
                console.ShowError("Invalid move. Position may be occupied or out of bounds.");
            }
        }

        console.DisplayBoard();
        console.ShowGameResult();
        console.WaitForContinue();
    }
}
