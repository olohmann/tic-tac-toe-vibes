using TicTacToe.Domain;
using TicTacToe.WebAPI.Models;

namespace TicTacToe.WebAPI.Services;

/// <summary>
/// Service for mapping between domain objects and DTOs.
/// </summary>
public static class MappingService
{
    /// <summary>
    /// Converts a position (0-8) to row and column coordinates (0-2).
    /// </summary>
    /// <param name="position">The board position (0-8).</param>
    /// <returns>A tuple containing (row, col).</returns>
    public static (int row, int col) PositionToCoordinates(int position)
    {
        var row = position / 3;
        var col = position % 3;
        return (row, col);
    }

    /// <summary>
    /// Converts row and column coordinates (0-2) to a position (0-8).
    /// </summary>
    /// <param name="row">The row (0-2).</param>
    /// <param name="col">The column (0-2).</param>
    /// <returns>The board position (0-8).</returns>
    public static int CoordinatesToPosition(int row, int col) => row * 3 + col;

    /// <summary>
    /// Converts the domain board to a string array for the API.
    /// </summary>
    /// <param name="board">The domain board (3x3 char array).</param>
    /// <returns>A string array representing the board (9 elements).</returns>
    public static string[] BoardToStringArray(char[,] board)
    {
        var result = new string[9];
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                var position = CoordinatesToPosition(row, col);
                result[position] = board[row, col] == ' ' ? "" : board[row, col].ToString();
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the domain player to a string.
    /// </summary>
    /// <param name="player">The domain player.</param>
    /// <returns>The player as a string ("X" or "O").</returns>
    public static string PlayerToString(Player player) => player == Player.X ? "X" : "O";

    /// <summary>
    /// Converts the domain game status to a string.
    /// </summary>
    /// <param name="status">The domain game status.</param>
    /// <returns>The status as a string.</returns>
    public static string StatusToString(GameStatus status) => status switch
    {
        GameStatus.InProgress => "InProgress",
        GameStatus.X_Won => "XWins",
        GameStatus.O_Won => "OWins",
        GameStatus.Draw => "Draw",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown game status")
    };

    /// <summary>
    /// Maps a game state and game ID to a game response DTO.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="gameState">The game state.</param>
    /// <returns>The game response DTO.</returns>
    public static GameResponseDto ToGameResponseDto(Guid gameId, GameState gameState)
    {
        return new GameResponseDto(
            gameId,
            BoardToStringArray(gameState.GetBoard()),
            PlayerToString(gameState.CurrentPlayer),
            StatusToString(gameState.Status));
    }

    /// <summary>
    /// Maps a game state, game ID, and last move to a move game response DTO.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="gameState">The game state.</param>
    /// <param name="lastMove">The last move made.</param>
    /// <returns>The move game response DTO.</returns>
    public static MoveGameResponseDto ToMoveGameResponseDto(Guid gameId, GameState gameState, Move lastMove)
    {
        var lastMoveDto = new MoveResponseDto(
            PlayerToString(lastMove.Player),
            CoordinatesToPosition(lastMove.Row, lastMove.Col));

        return new MoveGameResponseDto(
            gameId,
            BoardToStringArray(gameState.GetBoard()),
            PlayerToString(gameState.CurrentPlayer),
            StatusToString(gameState.Status),
            lastMoveDto);
    }
}