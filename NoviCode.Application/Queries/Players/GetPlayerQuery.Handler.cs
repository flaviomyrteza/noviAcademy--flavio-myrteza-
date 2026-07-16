using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Queries.Players;

public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, PlayerDto?>
{
	private readonly IGetPlayerPersistence _persistence;

	public GetPlayerQueryHandler(IGetPlayerPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task<PlayerDto?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
	{
		var player = await _persistence.TryGet(request.PlayerId);

		return player is null ? null : PlayerDto.From(player);
	}
}
