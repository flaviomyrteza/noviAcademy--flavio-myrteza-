namespace NoviCode.Api;

// Request DTO — what the client sends to create a player.
// The response shape is the Application-layer PlayerDto returned by the MediatR handlers.
public record CreatePlayerRequest(string Name, int Score);
public record UpdatePlayerScore(int Score);
