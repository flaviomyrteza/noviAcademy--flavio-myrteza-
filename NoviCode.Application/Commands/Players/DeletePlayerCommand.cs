using MediatR;

namespace NoviCode.Commands.Players;

public record DeletePlayerCommand(Guid playerId) : IRequest;
