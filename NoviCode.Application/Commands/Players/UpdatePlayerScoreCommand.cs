using MediatR;

namespace NoviCode.Commands.Players;

public record UpdatePlayerScoreCommand(Guid playerId, int score) : IRequest;
