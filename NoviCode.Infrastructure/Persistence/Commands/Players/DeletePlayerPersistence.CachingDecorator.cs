using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class DeletePlayerPersistenceCachingDecorator : IDeletePlayerPersistence
	{
		private readonly IPlayersCache _cache;
		private readonly IDeletePlayerPersistence _inner;

		public DeletePlayerPersistenceCachingDecorator(IPlayersCache cache, IDeletePlayerPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task Delete(Guid playerId)
		{
			await _inner.Delete(playerId);

			_cache.RemovePlayer(playerId);
		}
	}
}
