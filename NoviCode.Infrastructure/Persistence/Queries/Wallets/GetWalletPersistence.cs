using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Queries.Wallets
{
	public class GetWalletPersistence : IGetWalletPersistence
	{
		private readonly AppDbContext _db;

		public GetWalletPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task<Wallet?> TryGet(Guid walletId)
		{
			return await _db.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == walletId);
		}
	}
}
