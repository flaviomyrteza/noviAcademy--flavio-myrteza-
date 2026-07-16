using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Queries.Players;

// Query: list all players.
public record GetAllPlayersQuery : IRequest<IReadOnlyCollection<PlayerDto>>;
