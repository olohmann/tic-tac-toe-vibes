using TicTacToe.Domain;
using TicTacToe.WebAPI.Services;

namespace TicTacToe.WebAPI.Tests.Services;

/// <summary>
/// Tests for the MappingService class.
/// </summary>
public class MappingServiceTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(2, 0, 2)]
    [InlineData(3, 1, 0)]
    [InlineData(4, 1, 1)]
    [InlineData(5, 1, 2)]
    [InlineData(6, 2, 0)]
    [InlineData(7, 2, 1)]
    [InlineData(8, 2, 2)]
    public void PositionToCoordinates_ShouldReturnCorrectRowAndCol(int position, int expectedRow, int expectedCol)
    {
        // Act
        var (row, col) = MappingService.PositionToCoordinates(position);

        // Assert
        Assert.Equal(expectedRow, row);
        Assert.Equal(expectedCol, col);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(0, 2, 2)]
    [InlineData(1, 0, 3)]
    [InlineData(1, 1, 4)]
    [InlineData(1, 2, 5)]
    [InlineData(2, 0, 6)]
    [InlineData(2, 1, 7)]
    [InlineData(2, 2, 8)]
    public void CoordinatesToPosition_ShouldReturnCorrectPosition(int row, int col, int expectedPosition)
    {
        // Act
        var position = MappingService.CoordinatesToPosition(row, col);

        // Assert
        Assert.Equal(expectedPosition, position);
    }

    [Fact]
    public void BoardToStringArray_EmptyBoard_ShouldReturnArrayOfEmptyStrings()
    {
        // Arrange
        var board = new char[3, 3];
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            board[i, j] = ' ';

        // Act
        var result = MappingService.BoardToStringArray(board);

        // Assert
        Assert.Equal(9, result.Length);
        Assert.All(result, s => Assert.Equal("", s));
    }

    [Fact]
    public void BoardToStringArray_WithMoves_ShouldReturnCorrectStringArray()
    {
        // Arrange
        var board = new char[3, 3];
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            board[i, j] = ' ';
        
        board[0, 0] = 'X';
        board[1, 1] = 'O';
        board[2, 2] = 'X';

        // Act
        var result = MappingService.BoardToStringArray(board);

        // Assert
        Assert.Equal("X", result[0]);  // position 0
        Assert.Equal("", result[1]);   // position 1
        Assert.Equal("", result[2]);   // position 2
        Assert.Equal("", result[3]);   // position 3
        Assert.Equal("O", result[4]);  // position 4
        Assert.Equal("", result[5]);   // position 5
        Assert.Equal("", result[6]);   // position 6
        Assert.Equal("", result[7]);   // position 7
        Assert.Equal("X", result[8]);  // position 8
    }

    [Theory]
    [InlineData(Player.X, "X")]
    [InlineData(Player.O, "O")]
    public void PlayerToString_ShouldReturnCorrectString(Player player, string expected)
    {
        // Act
        var result = MappingService.PlayerToString(player);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(GameStatus.InProgress, "InProgress")]
    [InlineData(GameStatus.X_Won, "XWins")]
    [InlineData(GameStatus.O_Won, "OWins")]
    [InlineData(GameStatus.Draw, "Draw")]
    public void StatusToString_ShouldReturnCorrectString(GameStatus status, string expected)
    {
        // Act
        var result = MappingService.StatusToString(status);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToGameResponseDto_ShouldMapCorrectly()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameState = new GameState();

        // Act
        var result = MappingService.ToGameResponseDto(gameId, gameState);

        // Assert
        Assert.Equal(gameId, result.GameId);
        Assert.Equal(9, result.Board.Length);
        Assert.All(result.Board, s => Assert.Equal("", s));
        Assert.Equal("X", result.CurrentPlayer);
        Assert.Equal("InProgress", result.Status);
    }

    [Fact]
    public void ToMoveGameResponseDto_ShouldMapCorrectly()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameState = new GameState();
        gameState.TryMakeMove(1, 1); // Make a move to have a last move
        var lastMove = gameState.MoveHistory.Last();

        // Act
        var result = MappingService.ToMoveGameResponseDto(gameId, gameState, lastMove);

        // Assert
        Assert.Equal(gameId, result.GameId);
        Assert.Equal(9, result.Board.Length);
        Assert.Equal("X", result.Board[4]); // position 4 should have X
        Assert.Equal("O", result.CurrentPlayer); // should be O's turn now
        Assert.Equal("InProgress", result.Status);
        Assert.Equal("X", result.LastMove.Player);
        Assert.Equal(4, result.LastMove.Position);
    }
}
