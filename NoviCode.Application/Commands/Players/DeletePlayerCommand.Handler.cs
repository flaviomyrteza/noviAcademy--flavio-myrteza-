using MediatR;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Commands.Players;

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand>
{
	private readonly IDeletePlayerPersistence _persistence;

	public DeletePlayerCommandHandler(IDeletePlayerPersistence persistence)
	{
		_persistence = persistence;
	}

	public async Task Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
	{
		await _persistence.Delete(request.playerId);
	}
}
