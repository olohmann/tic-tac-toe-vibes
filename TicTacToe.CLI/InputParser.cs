namespace TicTacToe.CLI;

/// <summary>
/// Parses user input for game moves into row and column coordinates.
/// </summary>
public static class InputParser
{
    /// <summary>
    /// Attempts to parse user input into row and column coordinates.
    /// Supports coordinate-based format: "0,0" to "2,2".
    /// </summary>
    /// <param name="input">The user input string.</param>
    /// <param name="row">The parsed row coordinate (0-2).</param>
    /// <param name="col">The parsed column coordinate (0-2).</param>
    /// <returns>True if parsing was successful, false otherwise.</returns>
    public static bool TryParseCoordinates(string? input, out int row, out int col)
    {
        row = -1;
        col = -1;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        var parts = input.Trim().Split(',');
        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[0].Trim(), out row) || !int.TryParse(parts[1].Trim(), out col))
            return false;

        return row >= 0 && row <= 2 && col >= 0 && col <= 2;
    }
}