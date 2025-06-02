using TicTacToe.CLI;

/// <summary>
/// Tests for the InputParser class.
/// </summary>
public class InputParserTests
{
    [Theory]
    [InlineData("0,0", 0, 0)]
    [InlineData("1,1", 1, 1)]
    [InlineData("2,2", 2, 2)]
    [InlineData("0,2", 0, 2)]
    [InlineData("2,0", 2, 0)]
    public void TryParseCoordinates_ValidInput_ShouldReturnTrueAndCorrectCoordinates(string input, int expectedRow, int expectedCol)
    {
        // Act
        var result = InputParser.TryParseCoordinates(input, out int row, out int col);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedRow, row);
        Assert.Equal(expectedCol, col);
    }

    [Theory]
    [InlineData(" 1,2 ", 1, 2)]
    [InlineData("0, 1", 0, 1)]
    [InlineData(" 2 , 0 ", 2, 0)]
    public void TryParseCoordinates_ValidInputWithWhitespace_ShouldReturnTrueAndCorrectCoordinates(string input, int expectedRow, int expectedCol)
    {
        // Act
        var result = InputParser.TryParseCoordinates(input, out int row, out int col);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedRow, row);
        Assert.Equal(expectedCol, col);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1")]
    [InlineData("1,")]
    [InlineData(",1")]
    [InlineData("1,2,3")]
    [InlineData("a,b")]
    [InlineData("1,a")]
    [InlineData("-1,0")]
    [InlineData("0,-1")]
    [InlineData("3,0")]
    [InlineData("0,3")]
    [InlineData("1.5,2")]
    [InlineData("1;2")]
    public void TryParseCoordinates_InvalidInput_ShouldReturnFalse(string? input)
    {
        // Act
        var result = InputParser.TryParseCoordinates(input, out int row, out int col);

        // Assert
        Assert.False(result);
    }
}