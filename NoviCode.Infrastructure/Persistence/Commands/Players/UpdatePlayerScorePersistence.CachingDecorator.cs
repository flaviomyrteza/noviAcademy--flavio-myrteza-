using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class UpdatePlayerScorePersistenceCachingDecorator : IUpdatePlayerScorePersistence
	{
		private readonly IPlayersCache _cache;
		private readonly IUpdatePlayerScorePersistence _inner;

		public UpdatePlayerScorePersistenceCachingDecorator(IPlayersCache cache, IUpdatePlayerScorePersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task Update(Player player)
		{
			await _inner.Update(player);

			_cache.AddOrUpdatePlayer(player);
		}
	}
}
