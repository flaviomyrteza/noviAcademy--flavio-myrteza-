using System.Diagnostics.CodeAnalysis;

namespace NoviCode.Caching
{
	public interface IWalletsCache
	{
		void AddOrUpdateWallet(Wallet wallet);
		bool TryGetWallet(Guid walletId, [NotNullWhen(true)] out Wallet? wallet);
	}

	public class WalletsCache : IWalletsCache
	{
		private const string _cacheKeyPrefix = "WalletsCache_";
		private readonly ICache _cache;
		private readonly TimeSpan _cacheDuration;

		public WalletsCache(ICache cache)
		{
			_cache = cache;
			_cacheDuration = TimeSpan.FromMinutes(1);
		}

		public void AddOrUpdateWallet(Wallet wallet)
		{
			var cacheKey = GetCacheKey(wallet.Id);

			_cache.Set(cacheKey, wallet, _cacheDuration);
		}

		public bool TryGetWallet(Guid walletId, out Wallet? wallet)
		{
			var cacheKey = GetCacheKey(walletId);

			return _cache.TryGet(cacheKey, out wallet);
		}

		private string GetCacheKey(Guid walletId)
			=> $"{_cacheKeyPrefix}{walletId}";
	}
}
