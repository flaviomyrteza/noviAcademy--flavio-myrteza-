using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Queries.Players;

// Query: fetch a player by id. Returns null if it does not exist.
public record GetPlayerQuery(Guid PlayerId) : IRequest<PlayerDto?>;
