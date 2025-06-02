namespace TicTacToe.WebAPI.Models;

/// <summary>
/// Represents the response data for a Tic Tac Toe game.
/// </summary>
/// <param name="GameId">The unique identifier for the game.</param>
/// <param name="Board">The game board as an array of 9 strings (empty string for empty cells).</param>
/// <param name="CurrentPlayer">The current player ("X" or "O").</param>
/// <param name="Status">The current game status.</param>
public record GameResponseDto(
    Guid GameId,
    string[] Board,
    string CurrentPlayer,
    string Status);