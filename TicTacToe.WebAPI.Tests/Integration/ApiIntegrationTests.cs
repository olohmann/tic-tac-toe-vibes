using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TicTacToe.WebAPI.Models;

namespace TicTacToe.WebAPI.Tests.Integration;

/// <summary>
/// Integration tests for the Tic Tac Toe Web API.
/// </summary>
public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task CreateGame_ShouldReturnValidGameResponse()
    {
        // Act
        var response = await _client.PostAsync("/api/games", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var game = JsonSerializer.Deserialize<GameResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(game);
        Assert.NotEqual(Guid.Empty, game.GameId);
        Assert.Equal(9, game.Board.Length);
        Assert.All(game.Board, s => Assert.Equal("", s));
        Assert.Equal("X", game.CurrentPlayer);
        Assert.Equal("InProgress", game.Status);
    }

    [Fact]
    public async Task GetGame_ExistingGame_ShouldReturnGameState()
    {
        // Arrange - Create a game first
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/games/{createdGame!.GameId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var game = JsonSerializer.Deserialize<GameResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(game);
        Assert.Equal(createdGame.GameId, game.GameId);
        Assert.Equal(createdGame.Board, game.Board);
        Assert.Equal(createdGame.CurrentPlayer, game.CurrentPlayer);
        Assert.Equal(createdGame.Status, game.Status);
    }

    [Fact]
    public async Task GetGame_NonExistingGame_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/games/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(error);
        Assert.Equal("Game not found", error.Error);
    }

    [Fact]
    public async Task MakeMove_ValidMove_ShouldReturnMoveResponse()
    {
        // Arrange - Create a game
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        var moveRequest = new MoveRequestDto(4);

        // Act
        var response = await _client.PostAsJsonAsync($"/api/games/{createdGame!.GameId}/moves", moveRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var moveResponse = JsonSerializer.Deserialize<MoveGameResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(moveResponse);
        Assert.Equal(createdGame.GameId, moveResponse.GameId);
        Assert.Equal("X", moveResponse.Board[4]);
        Assert.Equal("O", moveResponse.CurrentPlayer);
        Assert.Equal("InProgress", moveResponse.Status);
        Assert.Equal("X", moveResponse.LastMove.Player);
        Assert.Equal(4, moveResponse.LastMove.Position);
    }

    [Fact]
    public async Task MakeMove_NonExistingGame_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var moveRequest = new MoveRequestDto(4);

        // Act
        var response = await _client.PostAsJsonAsync($"/api/games/{nonExistentId}/moves", moveRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(error);
        Assert.Equal("Game not found", error.Error);
    }

    [Fact]
    public async Task MakeMove_InvalidPosition_ShouldReturnBadRequest()
    {
        // Arrange - Create a game
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        var moveRequest = new MoveRequestDto(10); // Invalid position

        // Act
        var response = await _client.PostAsJsonAsync($"/api/games/{createdGame!.GameId}/moves", moveRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(error);
        Assert.Contains("Position must be between 0 and 8", error.Error);
    }

    [Fact]
    public async Task MakeMove_PositionAlreadyOccupied_ShouldReturnBadRequest()
    {
        // Arrange - Create a game and make a move
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        var moveRequest = new MoveRequestDto(4);
        await _client.PostAsJsonAsync($"/api/games/{createdGame!.GameId}/moves", moveRequest); // First move

        // Act - Try to make move to same position
        var response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", moveRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(error);
        Assert.Equal("Invalid move: position already occupied", error.Error);
    }

    [Fact]
    public async Task MakeMove_GameAlreadyFinished_ShouldReturnUnprocessableEntity()
    {
        // Arrange - Create a game and play to completion
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        // Play a game to completion (X wins on diagonal)
        await _client.PostAsJsonAsync($"/api/games/{createdGame!.GameId}/moves", new MoveRequestDto(0)); // X
        await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(3)); // O
        await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(4)); // X
        await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(6)); // O
        await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(8)); // X wins

        // Act - Try to make another move
        var response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(1));

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponseDto>(content, _jsonOptions);
        
        Assert.NotNull(error);
        Assert.Equal("Game already finished.", error.Error);
    }

    [Fact]
    public async Task CompleteGameWorkflow_ShouldWorkCorrectly()
    {
        // Arrange - Create a game
        var createResponse = await _client.PostAsync("/api/games", null);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdGame = JsonSerializer.Deserialize<GameResponseDto>(createContent, _jsonOptions);

        // Act & Assert - Play a complete game
        
        // Move 1: X at position 0
        var move1Response = await _client.PostAsJsonAsync($"/api/games/{createdGame!.GameId}/moves", new MoveRequestDto(0));
        Assert.Equal(HttpStatusCode.OK, move1Response.StatusCode);
        var move1 = JsonSerializer.Deserialize<MoveGameResponseDto>(await move1Response.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("X", move1!.Board[0]);
        Assert.Equal("O", move1.CurrentPlayer);
        Assert.Equal("InProgress", move1.Status);

        // Move 2: O at position 3
        var move2Response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(3));
        Assert.Equal(HttpStatusCode.OK, move2Response.StatusCode);
        var move2 = JsonSerializer.Deserialize<MoveGameResponseDto>(await move2Response.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("O", move2!.Board[3]);
        Assert.Equal("X", move2.CurrentPlayer);
        Assert.Equal("InProgress", move2.Status);

        // Move 3: X at position 4
        var move3Response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(4));
        Assert.Equal(HttpStatusCode.OK, move3Response.StatusCode);
        var move3 = JsonSerializer.Deserialize<MoveGameResponseDto>(await move3Response.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("X", move3!.Board[4]);
        Assert.Equal("O", move3.CurrentPlayer);
        Assert.Equal("InProgress", move3.Status);

        // Move 4: O at position 6
        var move4Response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(6));
        Assert.Equal(HttpStatusCode.OK, move4Response.StatusCode);
        var move4 = JsonSerializer.Deserialize<MoveGameResponseDto>(await move4Response.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("O", move4!.Board[6]);
        Assert.Equal("X", move4.CurrentPlayer);
        Assert.Equal("InProgress", move4.Status);

        // Move 5: X at position 8 (winning move)
        var move5Response = await _client.PostAsJsonAsync($"/api/games/{createdGame.GameId}/moves", new MoveRequestDto(8));
        Assert.Equal(HttpStatusCode.OK, move5Response.StatusCode);
        var move5 = JsonSerializer.Deserialize<MoveGameResponseDto>(await move5Response.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("X", move5!.Board[8]);
        Assert.Equal("XWins", move5.Status);

        // Verify final game state
        var finalStateResponse = await _client.GetAsync($"/api/games/{createdGame.GameId}");
        Assert.Equal(HttpStatusCode.OK, finalStateResponse.StatusCode);
        var finalState = JsonSerializer.Deserialize<GameResponseDto>(await finalStateResponse.Content.ReadAsStringAsync(), _jsonOptions);
        Assert.Equal("XWins", finalState!.Status);
    }
}