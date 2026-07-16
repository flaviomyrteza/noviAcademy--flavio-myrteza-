using NoviCode.Caching;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
	public class CreateWalletPersistenceCachingDecorator : ICreateWalletPersistence
	{
		private readonly IWalletsCache _cache;
		private readonly ICreateWalletPersistence _inner;

		public CreateWalletPersistenceCachingDecorator(IWalletsCache cache, ICreateWalletPersistence inner)
		{
			_cache = cache;
			_inner = inner;
		}

		public async Task Add(Wallet wallet)
		{
			await _inner.Add(wallet);

			_cache.AddOrUpdateWallet(wallet);
		}
	}
}
