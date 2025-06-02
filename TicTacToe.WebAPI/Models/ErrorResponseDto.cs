namespace TicTacToe.WebAPI.Models;

/// <summary>
/// Represents an error response from the API.
/// </summary>
/// <param name="Error">The error message.</param>
public record ErrorResponseDto(string Error);