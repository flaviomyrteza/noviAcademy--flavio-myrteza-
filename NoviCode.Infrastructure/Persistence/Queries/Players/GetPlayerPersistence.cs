using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Queries.Players
{
	public class GetPlayerPersistence : IGetPlayerPersistence
	{
		private readonly AppDbContext _db;

		public GetPlayerPersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task<Player> Get(Guid playerId)
		{
			return await _db.Players.AsNoTracking().FirstAsync(p => p.Id == playerId);
		}

		public async Task<Player?> TryGet(Guid playerId)
		{
			return await _db.Players.AsNoTracking().FirstOrDefaultAsync(p => p.Id == playerId);
		}
	}
}
