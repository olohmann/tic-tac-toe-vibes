namespace TicTacToe.WebAPI.Models;

/// <summary>
/// Represents a request to make a move in a Tic Tac Toe game.
/// </summary>
/// <param name="Position">The board position (0-8) where to place the move.</param>
public record MoveRequestDto(int Position);