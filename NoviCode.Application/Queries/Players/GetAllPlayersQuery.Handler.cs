using MediatR;
using NoviCode.Dtos;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Queries.Players;

public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IReadOnlyCollection<PlayerDto>>
{
	private readonly IGetAllPlayersPersistence _persistence;

	public GetAllPlayersQueryHandler(IGetAllPlayersPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task<IReadOnlyCollection<PlayerDto>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
	{
		var players = await _persistence.GetAll();

		return players
			.Select(PlayerDto.From)
			.ToArray();
	}
}
