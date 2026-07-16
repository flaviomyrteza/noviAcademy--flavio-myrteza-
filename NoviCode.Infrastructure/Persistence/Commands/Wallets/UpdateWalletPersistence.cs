using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Wallets
{
	public class UpdateWalletPersistence : IUpdateWalletPersistence
	{
		private readonly AppDbContext _db;

		public UpdateWalletPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task Update(Wallet wallet)
		{
			_db.Attach(wallet);

			_db.Entry(wallet).Property(x => x.Balance).IsModified = true;
			_db.Entry(wallet).Property(x => x.IsBlocked).IsModified = true;

			await _db.SaveChangesAsync();
		}
	}
}
