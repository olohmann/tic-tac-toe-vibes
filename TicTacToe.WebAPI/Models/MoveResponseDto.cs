namespace TicTacToe.WebAPI.Models;

/// <summary>
/// Represents a move made in the game for response data.
/// </summary>
/// <param name="Player">The player who made the move ("X" or "O").</param>
/// <param name="Position">The board position (0-8) where the move was made.</param>
public record MoveResponseDto(string Player, int Position);