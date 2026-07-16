using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
	public class CreateWalletPersistence : ICreateWalletPersistence
	{
		private readonly AppDbContext _db;

		public CreateWalletPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task Add(Wallet wallet)
		{
			_db.Wallets.Add(wallet);

			await _db.SaveChangesAsync();
		}
	}
}
