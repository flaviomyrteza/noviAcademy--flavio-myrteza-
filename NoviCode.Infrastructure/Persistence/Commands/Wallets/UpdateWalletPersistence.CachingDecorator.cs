using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
	public class UpdateWalletPersistenceCachingDecorator : IUpdateWalletPersistence
	{
		private readonly IWalletsCache _cache;
		private readonly IUpdateWalletPersistence _inner;

		public UpdateWalletPersistenceCachingDecorator(IWalletsCache cache, IUpdateWalletPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task Update(Wallet wallet)
		{
			await _inner.Update(wallet);

			_cache.AddOrUpdateWallet(wallet);
		}
	}
}
