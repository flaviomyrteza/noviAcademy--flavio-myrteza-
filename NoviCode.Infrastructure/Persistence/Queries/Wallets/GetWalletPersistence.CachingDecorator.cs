using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Queries.Wallets
{
	public class GetWalletPersistenceCachingDecorator : IGetWalletPersistence
	{
		private readonly IWalletsCache _cache;
		private readonly IGetWalletPersistence _inner;

		public GetWalletPersistenceCachingDecorator(IWalletsCache cache, IGetWalletPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task<Wallet?> TryGet(Guid walletId)
		{
			if (_cache.TryGetWallet(walletId, out var wallet))
				return wallet;

			wallet = await _inner.TryGet(walletId);

			if (wallet is not null)
				_cache.AddOrUpdateWallet(wallet);

			return wallet;
		}
	}
}
