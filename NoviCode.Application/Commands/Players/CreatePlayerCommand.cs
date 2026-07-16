using MediatR;
using NoviCode.Dtos;

namespace NoviCode.Commands.Players;

// Command: create a player. Returns the created player.
public record CreatePlayerCommand(string Name, int Score) : IRequest<PlayerDto>;
