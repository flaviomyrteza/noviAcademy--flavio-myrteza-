using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Queries.Players
{
	public class GetPlayerPersistenceCachingDecorator : IGetPlayerPersistence
	{
		private readonly IPlayersCache _cache;
		private readonly IGetPlayerPersistence _inner;

		public GetPlayerPersistenceCachingDecorator(IPlayersCache cache, IGetPlayerPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task<Player> Get(Guid playerId)
		{
			if (_cache.TryGetPlayer(playerId, out var player))
				return player;

			player = await _inner.Get(playerId);

			_cache.AddOrUpdatePlayer(player);

			return player;
		}

		public async Task<Player?> TryGet(Guid playerId)
		{
			if (_cache.TryGetPlayer(playerId, out var player))
				return player;

			player = await _inner.TryGet(playerId);

			if (player is not null)
				_cache.AddOrUpdatePlayer(player);

			return player;
		}
	}
}
