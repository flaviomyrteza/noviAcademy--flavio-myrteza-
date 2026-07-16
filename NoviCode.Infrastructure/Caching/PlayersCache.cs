using System.Diagnostics.CodeAnalysis;

namespace NoviCode.Caching
{
	public interface IPlayersCache
	{
		void AddOrUpdatePlayer(Player player);
		void RemovePlayer(Guid playerId);
		bool TryGetPlayer(Guid playerId, [NotNullWhen(true)] out Player? player);
	}

	public class PlayersCache : IPlayersCache
	{
		private const string _cacheKeyPrefix = "PlayersCache_";
		private readonly ICache _cache;
		private readonly TimeSpan _cacheDuration;

		public PlayersCache(ICache cache)
		{
			_cache = cache;
			_cacheDuration = TimeSpan.FromMinutes(1);
		}

		public void AddOrUpdatePlayer(Player player)
		{
			var cacheKey = GetCacheKey(player.Id);

			_cache.Set(cacheKey, player, _cacheDuration);
		}

		public bool TryGetPlayer(Guid playerId, out Player? player)
		{
			var cacheKey = GetCacheKey(playerId);

			return _cache.TryGet(cacheKey, out player);
		}

		public void RemovePlayer(Guid playerId)
		{
			var cacheKey = GetCacheKey(playerId);
			_cache.Remove(cacheKey);
		}

		private string GetCacheKey(Guid playerId)
			=> $"{_cacheKeyPrefix}{playerId}";
	}
}
