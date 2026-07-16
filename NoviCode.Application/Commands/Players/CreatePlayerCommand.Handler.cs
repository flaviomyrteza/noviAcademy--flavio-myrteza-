using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Players;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, PlayerDto>
{
	private readonly ICreatePlayerPersistence _persistence;

	public CreatePlayerCommandHandler(ICreatePlayerPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task<PlayerDto> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
	{
		var player = Player.CreateNew(Guid.NewGuid(), request.Name, request.Score);

		await _persistence.Add(player);

		return PlayerDto.From(player);
	}
}
