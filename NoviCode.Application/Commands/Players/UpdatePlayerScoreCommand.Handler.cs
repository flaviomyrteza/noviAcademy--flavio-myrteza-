using MediatR;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Players;

public class UpdatePlayerScoreCommandHandler : IRequestHandler<UpdatePlayerScoreCommand>
{
	private readonly IGetPlayerPersistence _getPlayerPersistence;
	private readonly IUpdatePlayerScorePersistence _updatePlayerScorePersistence;

	public UpdatePlayerScoreCommandHandler(IGetPlayerPersistence getPlayerPersistence, IUpdatePlayerScorePersistence updatePlayerScorePersistence)
	{
		_getPlayerPersistence = getPlayerPersistence;
		_updatePlayerScorePersistence = updatePlayerScorePersistence;
	}

	public async Task Handle(UpdatePlayerScoreCommand request, CancellationToken cancellationToken)
	{
		var player = await _getPlayerPersistence.Get(request.playerId);

		player.UpdateScore(request.score);

		await _updatePlayerScorePersistence.Update(player);
	}
}
