using TicTacToe.WebAPI.Middleware;
using TicTacToe.WebAPI.Models;
using TicTacToe.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Register game services
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

// Configure JSON options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Tic Tac Toe API v1");
    });
    
    // Enable CORS for development
    app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}

// Add global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// API endpoints
var apiGroup = app.MapGroup("/api");

// POST /api/games - Create a new game
apiGroup.MapPost("/games", (IGameService gameService) =>
{
    var response = gameService.CreateGame();
    return Results.Ok(response);
})
.WithName("CreateGame")
.WithSummary("Create a new Tic Tac Toe game")
.WithDescription("Creates a new game and returns the initial game state")
.Produces<GameResponseDto>(200);

// GET /api/games/{gameId} - Get current game state
apiGroup.MapGet("/games/{gameId:guid}", (Guid gameId, IGameService gameService) =>
{
    var response = gameService.GetGame(gameId);
    return Results.Ok(response);
})
.WithName("GetGame")
.WithSummary("Get current game state")
.WithDescription("Retrieves the current state of a game by its ID")
.Produces<GameResponseDto>(200)
.Produces<ErrorResponseDto>(404);

// POST /api/games/{gameId}/moves - Make a move
apiGroup.MapPost("/games/{gameId:guid}/moves", (Guid gameId, MoveRequestDto request, IGameService gameService) =>
{
    var response = gameService.MakeMove(gameId, request.Position);
    return Results.Ok(response);
})
.WithName("MakeMove")
.WithSummary("Make a move in the game")
.WithDescription("Makes a move at the specified position for the current player")
.Produces<MoveGameResponseDto>(200)
.Produces<ErrorResponseDto>(400)
.Produces<ErrorResponseDto>(404)
.Produces<ErrorResponseDto>(422);

app.Run();
