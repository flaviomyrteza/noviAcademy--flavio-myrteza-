using Microsoft.EntityFrameworkCore;
using NoviCode.Persistence.Context;
using NoviCode.Services.Infrastructure;

namespace NoviCode.Persistence.Commands.Players
{
	public class UpdatePlayerScorePersistence : IUpdatePlayerScorePersistence
	{
		private readonly AppDbContext _db;

		public UpdatePlayerScorePersistence(AppDbContext db)
		{
			_db = db;
		}

		public async Task Update(Player player)
		{
			_db.Attach(player);

			_db.Entry(player).Property(x => x.Score).IsModified = true;

			await _db.SaveChangesAsync();
		}
	}
}
